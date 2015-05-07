using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services
{
    public class GroupParticipantService
    {
        public List<GroupServingParticipant> GetServingParticipants()
        {
            var user = Environment.GetEnvironmentVariable("MP_API_DB_USER");
            var password = Environment.GetEnvironmentVariable("MP_API_DB_PASSWORD");
            var connStr = ConfigurationManager.ConnectionStrings["MinistryPlatformDatabaseSource"].ToString().Replace("%MP_DB_USER%", user).Replace("%MP_DB_PASSWORD%", password);


            var conn = new SqlConnection(connStr);
            SqlDataReader rdr = null;
            try
            {
                
                conn.Open();
                var command =
                    new SqlCommand(
                        "SELECT * FROM MinistryPlatform.dbo.vw_crds_Serving_Participants v WHERE v.Participant_ID IN ( 994377, 1446320, 1446324, 2057353 ) ",
                        conn);

                rdr = command.ExecuteReader();

                var list = new List<GroupServingParticipant>();
                while (rdr.Read())
                {
                    //Console.WriteLine(rdr[0]);
                    var s = new GroupServingParticipant();
                    s.DomainId = rdr.GetInt32(rdr.GetOrdinal("Domain_ID"));
                    s.EventId = rdr.GetInt32(rdr.GetOrdinal("Event_ID"));
                    s.EventStartDateTime = (DateTime) rdr["Event_Start_Date"];
                    s.EventTitle = rdr.GetString(rdr.GetOrdinal("Event_Title"));
                    s.GroupId = rdr.GetInt32(rdr.GetOrdinal("Group_ID"));
                    s.GroupName = rdr.GetString(rdr.GetOrdinal("Group_Name"));
                    s.GroupPrimaryContactEmail = rdr.GetString(rdr.GetOrdinal("Primary_Contact_Email"));
                    s.OpportunityId = rdr.GetInt32(rdr.GetOrdinal("Opportunity_ID"));
                    s.OpportunityMaximumNeeded = Convert.ToInt16(rdr["Maximum_Needed"]);
                    s.OpportunityMinimumNeeded = Convert.ToInt16(rdr["Minimum_Needed"]);
                    s.OpportunityRoleTitle = rdr.GetString(rdr.GetOrdinal("Role_Title"));
                    s.OpportunityShiftEnd = rdr.GetTimeSpan(rdr.GetOrdinal("Shift_End"));
                    s.OpportunityShiftStart = rdr.GetTimeSpan(rdr.GetOrdinal("Shift_Start"));
                    //s.OpportunitySignUpDeadline = rdr.GetInt32(rdr.GetOrdinal("Sign_Up_Deadline_ID"));
                    s.OpportunityTitle = rdr.GetString(rdr.GetOrdinal("Opportunity_Title"));
                    s.ParticipantNickname = rdr.GetString(rdr.GetOrdinal("Nickname"));
                    s.ParticipantEmail = rdr.GetString(rdr.GetOrdinal("Email_Address"));
                    s.ParticipantId = rdr.GetInt32(rdr.GetOrdinal("Participant_ID"));
                    s.ParticipantLastName = rdr.GetString(rdr.GetOrdinal("Last_Name"));
                    //bool? nullBoolean = null;
                    //bool? result = rdr.IsDBNull(rdr.GetOrdinal("Rsvp")) ? nullBoolean : (bool)rdr["Rsvp"];
                    //var thisThing = rdr.GetInt32(rdr.GetOrdinal("Rsvp"));
                    bool? rsvp = false;
                    if (rdr.IsDBNull(rdr.GetOrdinal("Rsvp")))
                    {
                        rsvp = null;
                    }
                    else if (rdr.GetInt32(rdr.GetOrdinal("Rsvp")) == 1)
                    {
                        rsvp = true;
                    }
                    s.Rsvp = rsvp;
                    list.Add(s);
                }
                return list;
            }
            finally
            {
                // close the reader
                if (rdr != null)
                {
                    rdr.Close();
                }

                // 5. Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}
