using System;

namespace MinistryPlatform.Models
{
    public class GroupServingParticipant
    {
        public int ContactId { get; set; }
        public int DomainId { get; set; }
        public string EventType { get; set; }
        public int EventTypeId { get; set; }
        public int EventId { get; set; }
        public DateTime EventStartDateTime { get; set; }
        public string EventTitle { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int GroupRoleId { get; set; }
        public string GroupPrimaryContactEmail { get; set; }
        public int OpportunityId { get; set; }
        public int? OpportunityMaximumNeeded { get; set; }
        public int? OpportunityMinimumNeeded { get; set; }
        public string OpportunityRoleTitle { get; set; }
        public TimeSpan? OpportunityShiftEnd { get; set; }
        public TimeSpan? OpportunityShiftStart { get; set; }
        public int OpportunitySignUpDeadline { get; set; }
        public int DeadlinePassedMessage { get; set; }
        public string OpportunityTitle { get; set; }
        public string ParticipantNickname { get; set; }
        public string ParticipantEmail { get; set; }
        public int ParticipantId { get; set; }
        public string ParticipantLastName { get; set; }
        public string Room { get; set; }
        public long RowNumber { get; set; }
        public bool? Rsvp { get; set; }
        public bool LoggedInUser { get; set; }
    }
}