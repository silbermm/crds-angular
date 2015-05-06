using System.Collections.Generic;
using MinistryPlatform.Models;

namespace crds_angular.Models.Crossroads
{
    public class FamilyMember
    {
        public int ContactId { get; set; }
        public string PreferredName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int ParticipantId { get; set; }
        public Participant Participant { get; set; }
        public List<MinistryPlatform.Models.Response> Responses { get; set; }
    }
}