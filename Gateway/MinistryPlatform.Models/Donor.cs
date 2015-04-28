using System;

namespace MinistryPlatform.Models
{
    public class Donor
    {
        public int DonorId { get; set; }
        public int ContactId { get; set; }
        public string StatementFreq { get; set; }
        public string StatementType { get; set; }
        public string StatementMethod { get; set; }
        public DateTime SetupDate { get; set; }
        
    }
}
