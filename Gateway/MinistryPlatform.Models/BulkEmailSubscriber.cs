using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinistryPlatform.Models
{
    public class BulkEmailSubscriber
    {
        public int ContactPublicationId { get; set; }
        public int ContactId { get; set; }
        public string ThirdPartyContactId { get; set; }
        public string EmailAddress { get; set; }
        public bool Subscribed { get; set; }
        public Dictionary<string, string> MergeFields { get; private set; }

        public BulkEmailSubscriber()
        {
            MergeFields = new Dictionary<string, string>();
        }
    }
}