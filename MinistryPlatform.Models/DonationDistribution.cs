namespace MinistryPlatform.Models
{
    public class DonationDistribution
    {
        public int donationDistributionId { get; set; }
        public int donationId { get; set; }
        public int donationDistributionAmt { get; set; }
        public string donationDistributionProgram { get; set; }
    }
}