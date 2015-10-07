namespace MinistryPlatform.Models
{
    public class DonationBatch
    {
        public int Id { get; set; }
        public string ProcessorTransferId { get; set; }
        public int? DepositId { get; set; }
        public string BatchName { get; set; }
    }
}
