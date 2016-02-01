using System;

namespace MinistryPlatform.Models
{
    public class HouseholdMember
    {
        public int ContactId { get; set; }
        public string FirstName { get; set; }
        public string Nickname { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string HouseholdPosition { get; set; }
        public int? StatementTypeId { get; set; }
        public int? DonorId { get; set; }
    }
}
