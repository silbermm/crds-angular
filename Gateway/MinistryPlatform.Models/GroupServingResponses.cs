using System;

namespace MinistryPlatform.Models
{
    public class GroupServingResponses
    {
        public int ContactId { get; set; }
        public int ParticipantId { get; set; }
        public int GroupId { get; set; }
        public int EventId { get; set; }
        public int ResponseResultId { get; set; }
        public DateTime ResponseDate { get; set; }
    }
}
