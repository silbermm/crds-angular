using System;

namespace MinistryPlatform.Models
{
    public class TripParticipant
    {
        public string EmailAddress { get; set; }
        public DateTime EventEndDate { get; set; }
        public int EventId { get; set; }
        public int EventParticipantId { get; set; }
        public DateTime EventStartDate { get; set; }
        public string EventTitle { get; set; }
        public string EventType { get; set; }
        public string Lastname { get; set; }
        public string Nickname { get; set; }
        public int ParticipantId { get; set; }
        public int ProgramId { get; set; }
        public string ProgramName { get; set; }
        public int CampaignId { get; set; }
        public string CampaignName { get; set; }
        public int DonorId { get; set; }
        public int ContactId { get; set; }
    }
}