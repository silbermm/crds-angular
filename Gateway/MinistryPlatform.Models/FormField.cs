namespace MinistryPlatform.Models
{
    public class FormField
    {
        public int FormFieldId { get; set; }
        public int CrossroadsId { get; set; }
        public string FieldLabel { get; set; }
        public int FieldOrder { get; set; }
        public int FormId { get; set; }
        public string FormTitle { get; set; }
        public string FieldType { get; set; }
        public bool Required { get; set; }
    }
}
