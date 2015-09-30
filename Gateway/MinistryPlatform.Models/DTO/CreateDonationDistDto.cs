namespace MinistryPlatform.Models.DTO
{
   public class CreateDonationDistDto
    {
        public int Amount { get; set; }
        public int DonorId { get; set; }
        public int CongregationId { get; set; }
        public string PaymentType { get; set; }
        public string ProgramId { get; set; }
    }
}
