using System;

namespace MinistryPlatform.Models
{
    public class ContactRelationship
    {
        public int Contact_Id { get; set; }
        public string Email_Address { get; set; }
        public string Last_Name { get; set; }
        public string Preferred_Name { get; set; }
        public int Participant_Id { get; set; }
        public int Relationship_Id { get; set; }
        public DateTime Birth_date { get; set; }
        public int Age { get; set; }
        public int HighSchoolGraduationYear { get; set; }
    }
}