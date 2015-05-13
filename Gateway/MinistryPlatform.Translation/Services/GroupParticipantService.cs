using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using Crossroads.Utilities.Extensions;

namespace MinistryPlatform.Translation.Services
{
    public class GroupParticipantService : IGroupParticipantService
    {
        private IDbConnection _dbConnection;

        public GroupParticipantService(IDbConnection dbConnection)
        {
            this._dbConnection = dbConnection;
        }
        public List<GroupServingParticipant> GetServingParticipants(List<int> participants, long from , long to)
        {
            var connection = _dbConnection;
            try
            {
                connection.Open();

                var command = CreateSqlCommand(participants, from, to);
                command.Connection = connection;
                var reader = command.ExecuteReader();
                var groupServingParticipants = new List<GroupServingParticipant>();
                while (reader.Read())
                {
                    var participant = new GroupServingParticipant();
                    participant.ContactId = reader.GetInt32(reader.GetOrdinal("Contact_ID"));
                    participant.EventType = reader.GetString(reader.GetOrdinal("Event_Type"));
                    participant.EventTypeId = reader.GetInt32(reader.GetOrdinal("Event_Type_ID"));
                    participant.GroupRoleId = reader.GetInt32(reader.GetOrdinal("Group_Role_ID"));
                    participant.DomainId = reader.GetInt32(reader.GetOrdinal("Domain_ID"));
                    participant.EventId = reader.GetInt32(reader.GetOrdinal("Event_ID"));
                    participant.EventStartDateTime = (DateTime) reader["Event_Start_Date"];
                    participant.EventTitle = reader.GetString(reader.GetOrdinal("Event_Title"));
                    participant.GroupId = reader.GetInt32(reader.GetOrdinal("Group_ID"));
                    participant.GroupName = reader.GetString(reader.GetOrdinal("Group_Name"));
                    participant.GroupPrimaryContactEmail = reader.GetString(reader.GetOrdinal("Primary_Contact_Email"));
                    participant.OpportunityId = reader.GetInt32(reader.GetOrdinal("Opportunity_ID"));
                    participant.OpportunityMaximumNeeded = SafeInt(reader, "Maximum_Needed");
                    participant.OpportunityMinimumNeeded = SafeInt(reader, "Minimum_Needed");
                    participant.OpportunityRoleTitle = reader.GetString(reader.GetOrdinal("Role_Title"));
                    participant.OpportunityShiftEnd = GetTimeSpan(reader, "Shift_End");
                    participant.OpportunityShiftStart = GetTimeSpan(reader, "Shift_Start");
                    participant.OpportunityTitle = reader.GetString(reader.GetOrdinal("Opportunity_Title"));
                    participant.ParticipantNickname = reader.GetString(reader.GetOrdinal("Nickname"));
                    participant.ParticipantEmail = SafeString(reader, "Email_Address");
                    participant.ParticipantId = reader.GetInt32(reader.GetOrdinal("Participant_ID"));
                    participant.ParticipantLastName = reader.GetString(reader.GetOrdinal("Last_Name"));
                    participant.RowNumber = reader.GetInt64(reader.GetOrdinal("RowNumber"));
                    participant.Rsvp = GetRsvp(reader, "Rsvp");
                    groupServingParticipants.Add(participant);
                }
                return groupServingParticipants;
            }
            finally
            {
                connection.Close();
            }
        }

        private IDbCommand CreateSqlCommand(IEnumerable<int> participants, long from, long to)
        {
            var sb = new StringBuilder();
            var i = 1;

            var fromDate = from == 0 ? DateTime.Today : from.FromUnixTime();
            var toDate = to == 0 ? DateTime.Today.AddDays(28) : to.FromUnixTime();
               

            var query =
                @"SELECT *, 
                        Row_Number() Over ( Order By v.Event_Start_Date ) As RowNumber 
                    FROM MinistryPlatform.dbo.vw_crds_Serving_Participants v 
                    WHERE v.Participant_ID IN ( @participants ) 
                    AND Event_Start_Date between @from and @to
                    ORDER BY Event_Start_Date, Group_Name, Contact_ID";

            IDbCommand command = new SqlCommand();
            command.CommandType = CommandType.Text;

            foreach (var participant in participants)
            {
                sb.Append("@ParticipantId" + i + ",");
                var c = new SqlParameter("@ParticipantId" + i, participant) {DbType = DbType.Int32};
                command.Parameters.Add(c);
                i++;
            }

            command.Parameters.Add(new SqlParameter("@from", fromDate) {DbType = DbType.DateTime});
            command.Parameters.Add(new SqlParameter("@to", toDate) { DbType = DbType.DateTime });

            var tmp = sb.Remove(sb.Length - 1, 1);
            query = query.Replace("@participants", tmp.ToString());
            command.CommandText = query;
            

            return command;
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

        private static TimeSpan GetTimeSpan(IDataRecord record, string columnName)
        {
            var columnIndex = record.GetOrdinal(columnName);
            var reader = record as SqlDataReader;
            if (reader == null) throw new Exception("The DataReader is not a SqlDataReader");

            var myTimeSpan = reader.GetTimeSpan(columnIndex);
            return myTimeSpan;
        }

        private string SafeString(IDataRecord record, string fieldName)
        {
            var ordinal = record.GetOrdinal(fieldName);
            return !record.IsDBNull(ordinal) ? record.GetString(ordinal) : null;
        }

        private int? SafeInt(IDataRecord record, string fieldName)
        {
            var ordinal = record.GetOrdinal(fieldName);
            return !record.IsDBNull(ordinal) ? record.GetInt16(ordinal) : (int?) null;
        }
    }
}