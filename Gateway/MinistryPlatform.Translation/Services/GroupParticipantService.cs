using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class GroupParticipantService : IGroupParticipantService
    {
        public List<GroupServingParticipant> GetServingParticipants(List<int> participants)
        {
            var connection = DbConnection();
            try
            {
                connection.Open();

                var command = CreateSqlCommand(participants);
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
                    participant.OpportunityMaximumNeeded = Convert.ToInt16(reader["Maximum_Needed"]);
                    participant.OpportunityMinimumNeeded = Convert.ToInt16(reader["Minimum_Needed"]);
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

        private IDbCommand CreateSqlCommand(IEnumerable<int> participants)
        {
            var sb = new StringBuilder();
            var i = 1;
            var query =
                @"SELECT *, 
                        Row_Number() Over ( Order By v.Event_Start_Date ) As RowNumber 
                    FROM MinistryPlatform.dbo.vw_crds_Serving_Participants v 
                    WHERE v.Participant_ID IN ( @participants ) 
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

            var tmp = sb.Remove(sb.Length - 1, 1);
            query = query.Replace("@participants", tmp.ToString());
            command.CommandText = query;

            return command;
        }

        private static IDbConnection DbConnection()
        {
            var user = Environment.GetEnvironmentVariable("MP_API_DB_USER");
            var password = Environment.GetEnvironmentVariable("MP_API_DB_PASSWORD");
            var mpConnectionString =
                ConfigurationManager.ConnectionStrings["MinistryPlatformDatabaseSource"].ToString()
                    .Replace("%MP_DB_USER%", user)
                    .Replace("%MP_DB_PASSWORD%", password);

            IDbConnection connection = new SqlConnection(mpConnectionString);
            return connection;
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
    }
}