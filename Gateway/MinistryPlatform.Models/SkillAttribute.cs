using System;

namespace MinistryPlatform.Models
{
    public class SkillAttribute
    {
        public int dp_RecordID { get; set; }
        public string dp_RecordName { get; set; }
        public int dp_Selected { get; set; }
        public int? dp_FileID { get; set; }
        public int dp_RecordStatus { get; set; }
        public int Attribute_ID { get; set; }
        public string Attribute_Type { get; set; }
        public string Attribute_Category { get; set; }
        public string Attribute_Name { get; set; }
        public DateTime Start_Date { get; set; }
    }
}
