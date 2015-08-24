namespace crds_angular.Models.Crossroads.Stewardship
{
    public class DonationDTO
    {
        public string program_id { get; set; }
        public int amount { get; set; }
        public string donation_id { get; set; }
        public string email { get; set; }
        public int? batch_id { get; set; }
    }
}