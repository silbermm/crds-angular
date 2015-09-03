using System;

namespace crds_angular.Models.MP
{
    public class HouseholdMember
    {
        public int Contact_ID { get; set; }
        public string First_Name { get; set; }
        public string Nickname { get; set; }
        public string Last_Name { get; set; }
        public DateTime Date_Of_Birth { get; set; }
        public string Household_Position { get; set; }
    }
}