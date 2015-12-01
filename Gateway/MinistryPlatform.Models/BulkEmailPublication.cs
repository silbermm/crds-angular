using System;

namespace MinistryPlatform.Models
{
    public class BulkEmailPublication
    {
        public int PublicationId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThirdPartyPublicationId { get; set; }
        public DateTime LastSuccessfulSync { get; set; }
    }
}
