namespace MinistryPlatform.Models
{
    public class TripFormResponse
    {
        public int ContactId { get; set; }
        public int DestinationId { get; set; }
        public int? DonorId { get; set; }
        public int? EventId { get; set; }
        public int FormId { get; set; }
        public decimal FundraisingGoal { get; set; }
        public int ParticipantId { get; set; }
        public int? PledgeCampaignId { get; set; }
    }
}