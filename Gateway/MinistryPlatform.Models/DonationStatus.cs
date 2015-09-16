namespace MinistryPlatform.Models
{
    public class DonationStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool DisplayOnStatement { get; set; }
        public bool DisplayOnGivingHistory { get; set; }
        public bool DisplayOnMyTrips { get; set; }
    }
}
