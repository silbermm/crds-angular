namespace MinistryPlatform.Models
{
    public class PrivateInvite
    {
        public string EmailAddress { get; set; }
        public string InvitationGuid { get; set; }
        public bool InvitationUsed { get; set; }
        public int PledgeCampaignId { get; set; }
        public string PledgeCampaignIdText { get; set; }
        public int PrivateInvitationId { get; set; }
        public string RecipientName { get; set; }
    }
}