using System.Collections.Generic;

namespace MinistryPlatform.Models
{
    public class Household
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string HomePhone { get; set; }
        public string ForeignCountry { get; set; }
        public string County { get; set; }
        public int? CongregationId { get; set; }
        public int HouseholdId { get; set; }
        public List<HouseholdMember> HouseholdMembers { get; set; }  
    }
}
