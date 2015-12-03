using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinistryPlatform.Translation.Models.People
{
    public class Participant
    {
        public int ParticipantId { get; set; }
        public int ContactId { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public string role { get; set; }
        public string GroupName { get; set; }

    }
}
