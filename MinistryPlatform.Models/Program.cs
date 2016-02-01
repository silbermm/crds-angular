using System;


namespace MinistryPlatform.Models
{
    public class Program
    {
        public int ProgramId { get; set; }
        public string Name { get; set; }
        public int? CommunicationTemplateId { get; set; }
        public int ProgramType { get; set; }
        public bool AllowRecurringGiving { get; set; }
    }
}
