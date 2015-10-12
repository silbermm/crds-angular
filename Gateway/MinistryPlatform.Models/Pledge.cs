namespace MinistryPlatform.Models
{
    public class Pledge
    {
        public int PledgeId { get; set; }
        public int PledgeCampaignId { get; set; }
        public int DonorId { get; set; }
        public int PledgeStatusId { get; set; }
    }
}