using System.Collections.Generic;

namespace MinistryPlatform.Models
{
    public class Communication
    {
        public int AuthorUserId { get; set; }
        public int DomainId { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public Contact FromContact { get; set; }
        public Contact ReplyToContact { get; set; }
        public List<Contact> ToContacts { get; set; } 
        public int TemplateId { get; set; }
        public Dictionary<string, object> MergeData { get; set; }
    }
}