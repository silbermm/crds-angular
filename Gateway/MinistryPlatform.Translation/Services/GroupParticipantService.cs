using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class GroupParticipantService : IGroupParticipantService
    {
        //private IDbConnection _mpDbConnection;

        //public GroupParticipantService(IDbConnection mpDbConnection)
        //{
        //    this._mpDbConnection = mpDbConnection;
        //}
        public List<GroupServingParticipant> GetServingParticipants()
        {
            var user = Environment.GetEnvironmentVariable("MP_API_DB_USER");
            var password = Environment.GetEnvironmentVariable("MP_API_DB_PASSWORD");
            var mpConnectionString =
                ConfigurationManager.ConnectionStrings["MinistryPlatformDatabaseSource"].ToString()
                    .Replace("%MP_DB_USER%", user)
                    .Replace("%MP_DB_PASSWORD%", password);

            IDbConnection iconn = new SqlConnection(mpConnectionString);

            try
            {
                iconn.Open();
                const string query =
                    "SELECT *, Row_Number() Over ( Order By v.Event_Start_Date ) As AndyC FROM MinistryPlatform.dbo.vw_crds_Serving_Participants v WHERE v.Participant_ID IN ( 994377, 1446320, 1446324, 2057353 ) ORDER BY Event_Start_Date, Group_Name, Contact_ID";
                IDbCommand icommand = new SqlCommand(query, (SqlConnection) iconn);
                var reader = icommand.ExecuteReader();
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
                    participant.OpportunityShiftEnd = implGetTimeSpan(reader, "Shift_End");
                    participant.OpportunityShiftStart = implGetTimeSpan(reader, "Shift_Start");
                    participant.OpportunityTitle = reader.GetString(reader.GetOrdinal("Opportunity_Title"));
                    participant.ParticipantNickname = reader.GetString(reader.GetOrdinal("Nickname"));
                    participant.ParticipantEmail = reader.GetString(reader.GetOrdinal("Email_Address"));
                    participant.ParticipantId = reader.GetInt32(reader.GetOrdinal("Participant_ID"));
                    participant.ParticipantLastName = reader.GetString(reader.GetOrdinal("Last_Name"));
                    //participant.RowNumber = (int)reader[reader.GetOrdinal("RowNumber")];
                    participant.RowNumber = reader.GetInt64(reader.GetOrdinal("AndyC"));
                    participant.Rsvp = getNullableBool(reader, "Rsvp");
                    groupServingParticipants.Add(participant);
                }
                return groupServingParticipants;
            }
            finally
            {
                iconn.Close();
            }
            

            //var conn = new SqlConnection(mpConnection);
            //SqlDataReader rdr = null;
            //try
            //{
            //    conn.Open();
            //    var command =
            //        new SqlCommand(
            //            "SELECT * FROM MinistryPlatform.dbo.vw_crds_Serving_Participants v WHERE v.Participant_ID IN ( 994377, 1446320, 1446324, 2057353 ) ",
            //            conn);

            //    rdr = command.ExecuteReader();

            //    var list = new List<GroupServingParticipant>();
            //    while (rdr.Read())
            //    {
            //        //Console.WriteLine(rdr[0]);
            //        var s = new GroupServingParticipant();
            //        s.DomainId = rdr.GetInt32(rdr.GetOrdinal("Domain_ID"));
            //        s.EventId = rdr.GetInt32(rdr.GetOrdinal("Event_ID"));
            //        s.EventStartDateTime = (DateTime) rdr["Event_Start_Date"];
            //        s.EventTitle = rdr.GetString(rdr.GetOrdinal("Event_Title"));
            //        s.GroupId = rdr.GetInt32(rdr.GetOrdinal("Group_ID"));
            //        s.GroupName = rdr.GetString(rdr.GetOrdinal("Group_Name"));
            //        s.GroupPrimaryContactEmail = rdr.GetString(rdr.GetOrdinal("Primary_Contact_Email"));
            //        s.OpportunityId = rdr.GetInt32(rdr.GetOrdinal("Opportunity_ID"));
            //        s.OpportunityMaximumNeeded = Convert.ToInt16(rdr["Maximum_Needed"]);
            //        s.OpportunityMinimumNeeded = Convert.ToInt16(rdr["Minimum_Needed"]);
            //        s.OpportunityRoleTitle = rdr.GetString(rdr.GetOrdinal("Role_Title"));
            //        s.OpportunityShiftEnd = rdr.GetTimeSpan(rdr.GetOrdinal("Shift_End"));
            //        s.OpportunityShiftStart = rdr.GetTimeSpan(rdr.GetOrdinal("Shift_Start"));
            //        //s.OpportunitySignUpDeadline = rdr.GetInt32(rdr.GetOrdinal("Sign_Up_Deadline_ID"));
            //        s.OpportunityTitle = rdr.GetString(rdr.GetOrdinal("Opportunity_Title"));
            //        s.ParticipantNickname = rdr.GetString(rdr.GetOrdinal("Nickname"));
            //        s.ParticipantEmail = rdr.GetString(rdr.GetOrdinal("Email_Address"));
            //        s.ParticipantId = rdr.GetInt32(rdr.GetOrdinal("Participant_ID"));
            //        s.ParticipantLastName = rdr.GetString(rdr.GetOrdinal("Last_Name"));
            //        //bool? nullBoolean = null;
            //        //bool? result = rdr.IsDBNull(rdr.GetOrdinal("Rsvp")) ? nullBoolean : (bool)rdr["Rsvp"];
            //        //var thisThing = rdr.GetInt32(rdr.GetOrdinal("Rsvp"));
            //        //var rsvp = Rsvp(rdr);
            //        s.Rsvp = Rsvp(rdr);
            //        list.Add(s);
            //    }
            //    return list;
            //}
            //finally
            //{
            //    // close the reader
            //    if (rdr != null)
            //    {
            //        rdr.Close();
            //    }

            //    // 5. Close the connection
            //    if (conn != null)
            //    {
            //        conn.Close();
            //    }
            //}
        }

        private static bool? Rsvp(SqlDataReader rdr)
        {
            bool? rsvp = false;
            if (rdr.IsDBNull(rdr.GetOrdinal("Rsvp")))
            {
                rsvp = null;
            }
            else if (rdr.GetInt32(rdr.GetOrdinal("Rsvp")) == 1)
            {
                rsvp = true;
            }
            return rsvp;
        }

        private static bool? getNullableBool(IDataReader rdr, string columnName)
        {
            var ordinal = rdr.GetOrdinal(columnName);
            bool? rsvp = false;
            if (rdr.IsDBNull(ordinal))
            {
                rsvp = null;
            }
            else if (rdr.GetInt32(ordinal) == 1)
            {
                rsvp = true;
            }
            return rsvp;
        }

        private static TimeSpan implGetTimeSpan(IDataReader dr, string columnName)
        {
            var columnIndex = dr.GetOrdinal(columnName);
            var reader = dr as SqlDataReader;
            if (reader == null) throw new Exception("The DataReader is not a SqlDataReader");

            var myTimeSpan = reader.GetTimeSpan(columnIndex);
            return myTimeSpan;
        }
    }
}