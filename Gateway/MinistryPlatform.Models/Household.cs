using System.Collections.Generic;

namespace MinistryPlatform.Models
{
    public class Household
    {
        public string Home_Phone { get; set; }
        public int? Congregation_ID { get; set; }
        public int Household_ID { get; set; }
        //public List<HouseholdMember> Household_Members { get; set; }  
    }
}
