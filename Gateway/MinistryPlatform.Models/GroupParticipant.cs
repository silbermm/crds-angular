namespace MinistryPlatform.Models
{
    public class GroupParticipant
    {
        public int ParticipantId { get; set; }
        public int ContactId { get; set; }
        public string NickName { get; set; }
        public string LastName { get; set; }
        public int GroupRoleId { get; set; }
        public string GroupRoleTitle { get; set; }
    }
}
