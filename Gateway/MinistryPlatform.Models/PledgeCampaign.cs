using System;
using System.Collections.Generic;

namespace MinistryPlatform.Models
{
    public class PledgeCampaign
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Goal { get; set; }
        public int FormId { get; set; }
        public string Nickname { get; set; }
        public int YoungestAgeAllowed { get; set; }
        public DateTime RegistrationStart { get; set; }
        public DateTime RegistrationEnd { get; set; }
        public List<int> AgeExceptions { get; set; }
        public int EventId { get; set; }
    }
}