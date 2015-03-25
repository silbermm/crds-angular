using System.Security.Permissions;

namespace MinistryPlatform.Models
{
    public class MyContact
    {
        public int? AddressId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AnniversaryDate { get; set; }
        public int? CongregationId { get; set; }
        //public bool BulkEmailOptOut { get; set; }
        //public string CompanyPhone { get; set; }
        //public string ContactGuid { get; set; }
        public int ContactId { get; set; }
        //public string ContactStatus { get; set; }
        //public int? ContactStatusId { get; set; }
        //public string CurrentSchool { get; set; }
        public string DateOfBirth { get; set; }
        // public string DisplayName { get; set; }
        //public string DonorRecord { get; set; }
        //public int? DonorRecordId { get; set; }
        public string EmailAddress { get; set; }
        //public bool EmailUnlisted { get; set; }
        public string EmployerName { get; set; }
        //public string FaxPhone { get; set; }
        //public int? FileId { get; set; }
        public string FirstName { get; set; }
        //public string Gender { get; set; }
        public int? GenderId { get; set; }
        public int HouseholdId { get; set; }
        //public string HouseholdPosition { get; set; }
        //public int? HouseholdPositionId { get; set; }
        //public string HouseholdText { get; set; }
        //public string HighSchoolGradDate { get; set; }
        //public string IdCard { get; set; }
        //public string Industry { get; set; }
        //public int? IndustryId { get; set; }
        public string LastName { get; set; }
        public string MaidenName { get; set; }
        //public string MaritalStatus { get; set; }
        public int? MaritalStatusId { get; set; }
        public string MiddleName { get; set; }
        public int? MobileCarrierId { get; set; }
        public string MobilePhone { get; set; }
        //public bool MobilePhoneUnlisted { get; set; }
        public string NickName { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ForeignCountry { get; set; }
        public string HomePhone { get; set; }
    }
}