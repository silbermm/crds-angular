using System;

namespace MinistryPlatform.Translation.Models.EventReservations
{
    public class EventReservationDto
    {
        public int CongregationId { get; set; }
        public int ContactId { get; set; }
        public string Description { get; set; }
        public bool DonationBatchTool { get; set; }
        public DateTime EndDateTime { get; set; }
        public int EventTypeId { get; set; }
        public string MeetingInstructions { get; set; }
        public int MinutesSetup { get; set; }
        public int MinutesTeardown { get; set; }
        public int ProgramId { get; set; }
        public int? ReminderDaysId { get; set; }
        public bool SendReminder { get; set; }
        public DateTime StartDateTime { get; set; }
        public string Title { get; set; }
    }
}