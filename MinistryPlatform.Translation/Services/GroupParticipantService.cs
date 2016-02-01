using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Crossroads.Utilities.Extensions;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class GroupParticipantService : IGroupParticipantService
    {
        private readonly IDbConnection _dbConnection;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly IApiUserService _apiUserService;

        public GroupParticipantService(IDbConnection dbConnection,
                                       IConfigurationWrapper configurationWrapper,
                                       IMinistryPlatformService ministryPlatformService,
                                       IApiUserService apiUserService)

        {
            _dbConnection = dbConnection;
            _configurationWrapper = configurationWrapper;
            _ministryPlatformService = ministryPlatformService;
            _apiUserService = apiUserService;
        }

        public int Get(int groupId, int participantId)
        {
            var searchString = string.Format(",{0},{1}", groupId, participantId);
            var token = _apiUserService.GetToken();
            var groupParticipant = _ministryPlatformService.GetPageViewRecords("GroupParticipantsById", token, searchString).FirstOrDefault();
            return groupParticipant != null ? groupParticipant.ToInt("Group_Participant_ID") : 0;
        }

        public List<GroupServingParticipant> GetServingParticipants(List<int> participants, long from, long to, int loggedInContactId)
        {
            var connection = _dbConnection;
            try
            {
                connection.Open();

                var command = CreateSqlCommand(participants, from, to);
                command.Connection = connection;
                var reader = command.ExecuteReader();
                var groupServingParticipants = new List<GroupServingParticipant>();
                var rowNumber = 0;
                var defaultDeadlinePassedMessage = _configurationWrapper.GetConfigIntValue("DefaultDeadlinePassedMessage");
                while (reader.Read())
                {
                    var rowContactId = reader.GetInt32(reader.GetOrdinal("Contact_ID"));
                    var loggedInUser = (loggedInContactId == rowContactId);
                    rowNumber = rowNumber + 1;
                    var participant = new GroupServingParticipant();
                    participant.ContactId = rowContactId;
                    participant.EventType = reader.GetString(reader.GetOrdinal("Event_Type"));
                    participant.EventTypeId = reader.GetInt32(reader.GetOrdinal("Event_Type_ID"));
                    participant.GroupRoleId = reader.GetInt32(reader.GetOrdinal("Group_Role_ID"));
                    participant.DomainId = reader.GetInt32(reader.GetOrdinal("Domain_ID"));
                    participant.EventId = reader.GetInt32(reader.GetOrdinal("Event_ID"));
                    participant.EventStartDateTime = (DateTime) reader["Event_Start_Date"];
                    participant.EventTitle = reader.GetString(reader.GetOrdinal("Event_Title"));                   
                    participant.Room = SafeString(reader, "Room");
                    participant.GroupId = reader.GetInt32(reader.GetOrdinal("Group_ID"));
                    participant.GroupName = reader.GetString(reader.GetOrdinal("Group_Name"));
                    participant.GroupPrimaryContactEmail = reader.GetString(reader.GetOrdinal("Primary_Contact_Email"));
                    participant.OpportunityId = reader.GetInt32(reader.GetOrdinal("Opportunity_ID"));
                    participant.OpportunityMaximumNeeded = SafeInt(reader, "Maximum_Needed");
                    participant.OpportunityMinimumNeeded = SafeInt(reader, "Minimum_Needed");
                    participant.OpportunityRoleTitle = reader.GetString(reader.GetOrdinal("Role_Title"));
                    participant.OpportunityShiftEnd = GetTimeSpan(reader, "Shift_End");
                    participant.OpportunityShiftStart = GetTimeSpan(reader, "Shift_Start");
                    participant.OpportunitySignUpDeadline = (SafeInt32(reader, "Sign_Up_Deadline") ?? 0);
                    participant.DeadlinePassedMessage = (SafeInt32(reader, "Deadline_Passed_Message_ID") ?? defaultDeadlinePassedMessage);
                    participant.OpportunityTitle = reader.GetString(reader.GetOrdinal("Opportunity_Title"));
                    participant.ParticipantNickname = reader.GetString(reader.GetOrdinal("Nickname"));
                    participant.ParticipantEmail = SafeString(reader, "Email_Address");
                    participant.ParticipantId = reader.GetInt32(reader.GetOrdinal("Participant_ID"));
                    participant.ParticipantLastName = reader.GetString(reader.GetOrdinal("Last_Name"));
                    participant.RowNumber = rowNumber;
                    participant.Rsvp = GetRsvp(reader, "Rsvp");
                    participant.LoggedInUser = loggedInUser;
                    groupServingParticipants.Add(participant);
                }
                return
                    groupServingParticipants.OrderBy(g => g.EventStartDateTime)
                        .ThenBy(g => g.GroupName)
                        .ThenByDescending(g => g.LoggedInUser)
                        .ThenBy(g => g.ParticipantNickname)
                        .ToList();
            }
            finally
            {
                connection.Close();
            }
        }

        private static IDbCommand CreateSqlCommand(IReadOnlyList<int> participants, long from, long to)
        {
            var fromDate = from == 0 ? DateTime.Today : from.FromUnixTime();
            var toDate = to == 0 ? DateTime.Today.AddDays(29) : to.FromUnixTime();

            const string query = @"SELECT *
                    FROM MinistryPlatform.dbo.vw_crds_Serving_Participants v 
                    WHERE ( {0} ) 
                    AND Event_Start_Date >= @from 
                    AND Event_Start_Date <= @to
                    AND Event_Start_Date >= Participant_Start_Date
                    AND (Event_Start_Date <= Participant_End_Date OR Participant_End_Date IS NULL)";

            var participantSqlParameters = participants.Select((s, i) => "@participant" + i.ToString()).ToArray();
            var participantParameters =
                participants.Select((s, i) => string.Format("(v.Participant_ID = @participant{0})", i)).ToList();
            var participantWhere = string.Join(" OR ", participantParameters);

            using (IDbCommand command = new SqlCommand(string.Format(query, participantWhere)))
            {
                command.Parameters.Add(new SqlParameter("@from", fromDate) {DbType = DbType.DateTime});
                command.Parameters.Add(new SqlParameter("@to", toDate) {DbType = DbType.DateTime});

                //Add values to each participant parameter
                command.CommandType = CommandType.Text;
                for (var i = 0; i < participantParameters.Count; i++)
                {
                    var sqlParameter = new SqlParameter(participantSqlParameters[i], participants[i]);
                    command.Parameters.Add(sqlParameter);
                }
                return command;
            }
        }

        private static bool? GetRsvp(IDataRecord record, string columnName)
        {
            var ordinal = record.GetOrdinal(columnName);
            bool? rsvp = false;
            if (record.IsDBNull(ordinal))
            {
                rsvp = null;
            }
            else if (record.GetInt32(ordinal) == 1)
            {
                rsvp = true;
            }
            return rsvp;
        }

        private static TimeSpan? GetTimeSpan(IDataRecord record, string columnName)
        {
            var columnIndex = record.GetOrdinal(columnName);
            var reader = record as SqlDataReader;
            if (reader == null)
            {
                throw new Exception("The DataReader is not a SqlDataReader");
            }

            return !record.IsDBNull(columnIndex) ? reader.GetTimeSpan(columnIndex) : (TimeSpan?) null;
        }

        private static string SafeString(IDataRecord record, string fieldName)
        {
            try
            {
                var ordinal = record.GetOrdinal(fieldName);
                return !record.IsDBNull(ordinal) ? record.GetString(ordinal) : null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private static int? SafeInt(IDataRecord record, string fieldName)
        {
            var ordinal = record.GetOrdinal(fieldName);
            return !record.IsDBNull(ordinal) ? record.GetInt16(ordinal) : (int?) null;
        }

        private static int? SafeInt32(IDataRecord record, string fieldName)
        {
            var ordinal = record.GetOrdinal(fieldName);
            return !record.IsDBNull(ordinal) ? record.GetInt32(ordinal) : (int?) null;
        }
    }
}