using System;

namespace MinistryPlatform.Translation.Models
{
    public class MPServeReminders
    {
        public string Opportunity_Title { get; set; }
        public int Signedup_Contact_Id { get; set; }
        public string Signedup_Email_Address { get; set; }
        public string Event_Title { get; set; }
        public DateTime Event_Start_Date { get; set; }
        public DateTime Event_End_Date { get; set; }
        public int? Template_Id { get; set; }
        public int Opportunity_Contact_Id { get; set; }
        public string Opportunity_Email_Address { get; set; }
        public TimeSpan Shift_Start { get; set; }
        public TimeSpan Shift_End { get; set; }
    }
}
