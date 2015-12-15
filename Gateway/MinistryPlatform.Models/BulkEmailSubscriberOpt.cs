using Newtonsoft.Json;

namespace MinistryPlatform.Models
{
    public class BulkEmailSubscriberOpt
    {
        public string ThirdPartySystemID { get; set; }
        public string EmailAddress { get; set; }
        public string Status { get; set; }
        public int PublicationID { get; set; }
    }
}
