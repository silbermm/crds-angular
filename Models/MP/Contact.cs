using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Models.MP
{
    public class Contact
    {
        public int Contact_Id { get; set; }
        public string Email_Address { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Maiden_Name { get; set; }
        public string NickName { get; set; }
        public string DisplayName { get; set; }
        public string MaidenName { get; set; }

        public int? PrefixId { get; set; }
        public string Prefix { get; set; }

        public int? SuffixId { get; set; }
        public string Suffix { get; set; }

        public int? GenderId { get; set; }
        public string Gender { get; set; }

        public int? MaritalStatusId { get; set; }
        public string MaritalStatus { get; set; }

        public int? ContactStatusId { get; set; }
        public string ContactStatus { get; set; }

        public string EmailAddress { get; set; }
        public bool EmailUnlisted { get; set; }
        public bool BulkEmailOptOut { get; set; }

        public string MobilePhone { get; set; }
        public int? MobileCarrierId { get; set; }
        public string MobileCarrier { get; set; }
        public bool MobilePhoneUnlisted { get; set; }

        public string CompanyPhone { get; set; }
        public string PagerPhone { get; set; }
        public string FaxPhone { get; set; }

        public int? UserAccountId { get; set; }
        public string UserAccount { get; set; }

        public bool RemoveFromDictionary { get; set; }

        public int? IndustryId { get; set; }
        public string Industry { get; set; }

        public int? OccupationId { get; set; }
        public string Occupation { get; set; }

        public string CurrentSchool { get; set; }
        public string EmployerName { get; set; }

        public string SSN { get; set; }

        public string ContactGUID { get; set; }

        public string AnniversaryDate { get; set; }

        public string HSGraduationDate { get; set; }


        public int? HouseholdId { get; set; }
        public string HouseholdText { get; set; }

        public int? HouseholdPositionId { get; set; }
        public string HouseholdPosition { get; set; }

        public int? ParticipantRecordId { get; set; }
        public string ParticipantRecord { get; set; }

        public int? DonorRecordId { get; set; }
        public string DonorRecord { get; set; }

        public string DateOfBirth { get; set; }

        public string IdCard { get; set; }
        public string WebPage { get; set; }

        public string RecordName { get; set; }
        public int? FileId { get; set; }
        public int? TaskId { get; set; }
        public string TaskAction { get; set; }
        public string Mobile_Phone { get; set; }
        public int? Mobile_Carrier_ID { get; set; }
        public string Date_of_Birth { get; set; }
        public int? Marital_Status_Id { get; set; }
        public int? Gender_Id { get; set; }
        public string Employer_Name { get; set; }
        public string Anniversary_Date { get; set; }

    }
}
