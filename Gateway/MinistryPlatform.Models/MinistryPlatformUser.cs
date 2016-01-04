namespace MinistryPlatform.Models
{
    public class MinistryPlatformUser
    {
        public string UserId { get; set; }
        public string Guid { get; set; }
        public bool CanImpersonate { get; set; }
        public string UserEmail { get; set; }
        public int UserRecordId { get; set; }
    }
}
