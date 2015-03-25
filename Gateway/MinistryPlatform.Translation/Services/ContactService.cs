using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.PlatformService;

namespace MinistryPlatform.Translation.Services
{
    public class ContactService : BaseService
    {
        private readonly int _myProfilePageId = AppSettings("MyProfile");

        public MinistryPlatform.Models.MyContact GetMyProfile(string token)
        {
            var pageId = _myProfilePageId;
            var something = MinistryPlatformService.GetRecords(pageId, token);
            var somethingelse = MinistryPlatformService.GetRecordsDict(pageId, token);

            var count = somethingelse.Count;

            var dict = somethingelse[0];

            var lastName = dict["Last_Name"].ToString();
            var contactId = (int) dict["Contact_ID"];

            var Congregation_ID = dict["Congregation_ID"];
            var Household_ID = dict["Household_ID"];
            var Address_ID = dict["Address_ID"];
            var Address_Line_1 = dict["Address_Line_1"];
            var Address_Line_2 = dict["Address_Line_2"];

            var contact = new MyContact();

            contact.AddressId = dict["Address_ID"].ToString().ToNullableInt();
            contact.AddressLine1 = dict["Address_Line_1"].ToString();
            contact.AddressLine2 = dict["Address_Line_2"].ToString();

            contact.CongregationId = dict["Congregation_ID"].ToString().ToNullableInt();
            contact.HouseholdId = dict["Household_ID"].ToString().ToInt();

            contact.City = dict["City"].ToString();
            contact.State = dict["State"].ToString();
            contact.PostalCode = dict["Postal_Code"].ToString();

            contact.AnniversaryDate = dict["Anniversary_Date"].ToString().DateToString();
            //contact.BulkEmailOptOut = dict["Bulk_Email_Opt_Out"].ToString().ToBool();
            //contact.CompanyPhone = dict["Company_Phone"].ToString();
            //contact.ContactGuid = dict["Contact_Guid"].ToString();
            contact.ContactId = dict["Contact_ID"].ToString().ToInt();
            //contact.ContactStatus = dict["Contact_Status"].ToString();
            //contact.ContactStatusId = dict["Contact_Status_ID"].ToString().ToInt();
            //contact.CurrentSchool = dict["Current_School"].ToString();
            contact.DateOfBirth = dict["Date_of_Birth"].ToString();
            //contact.DisplayName = dict["Display_Name"].ToString();
            //contact.DonorRecord = dict["Donor_Record"].ToString();
            //contact.DonorRecordId = dict["Donor_Record_ID"].ToString().ToInt();


            contact.EmailAddress = dict["Email_Address"].ToString();
            //contact.EmailUnlisted = dict["Email_Unlisted"].ToString().ToBool();
            contact.EmployerName = dict["Employer_Name"].ToString();
            //contact.FaxPhone = dict["Fax_Phone"].ToString();
            //contact.FileId = dict["File_ID"].ToString().ToInt();
            contact.FirstName = dict["First_Name"].ToString();
            contact.ForeignCountry = dict["Foreign_Country"].ToString();
            //contact.Gender = dict["Gender"].ToString();
            contact.GenderId = dict["Gender_ID"].ToString().ToInt();
            contact.HomePhone = dict["Home_Phone"].ToString();
            //contact.HouseholdId = dict["Household_ID"].ToString().ToInt();
            //contact.HouseholdPosition = dict["Household_Position"].ToString();
            //contact.HouseholdPositionId = dict["Household_Position_ID"].ToString().ToInt();
            //contact.HouseholdText = dict["Household_Text"].ToString();
            //contact.HighSchoolGradDate = dict["High_School_Grad_Date"].ToString();
            //contact.IdCard = dict["Id_Card"].ToString();
            //contact.Industry = dict["Industry"].ToString();
            //contact.IndustryId = dict["Industry_ID"].ToString().ToInt();
            contact.LastName = dict["Last_Name"].ToString();
            contact.MaidenName = dict["Maiden_Name"].ToString();
            //contact.MaritalStatus = dict["Marital_Status"].ToString();
            contact.MaritalStatusId = dict["Marital_Status_ID"].ToString().ToInt();
            contact.MiddleName = dict["Middle_Name"].ToString();
            contact.MobileCarrierId = dict["Mobile_Carrier_ID"].ToString().ToNullableInt();
            contact.MobilePhone = dict["Mobile_Phone"].ToString();
            //contact.MobilePhoneUnlisted = dict["Mobile_Phone_Unlisted"].ToString().ToBool();
            contact.NickName = dict["Nickname"].ToString();
            //contact.Occupation = dict["Occupation"].ToString();
            //contact.OccupationId = dict["Occupation_ID"].ToString().ToInt();
            //contact.PagerPhone = dict["Pager_Phone"].ToString();
            //contact.ParticipantRecord = dict["Participant_Record"].ToString();
            //contact.ParticipantRecordId = dict["Participant_Record_ID"].ToString().ToInt();
            //contact.Prefix = dict["Prefix"].ToString();
            //contact.PrefixId = dict["Prefix_ID"].ToString().ToInt();
            //contact.RecordName = dict["Record_Name"].ToString();
            //contact.RemoveFromDictionary = dict["Remove_From_Dictionary"].ToString().ToBool();
            //contact.Ssn = dict["SSN"].ToString();
            //contact.Suffix = dict["Suffix"].ToString();
            //contact.SuffixId = dict["Suffix_ID"].ToString().ToInt();
            //contact.TaskAction = dict["Task_Action"].ToString();
            //contact.TaskId = dict["Task_ID"].ToString().ToInt();
            //contact.UserAccount = dict["User_Account"].ToString();
            //contact.UserAccountId = dict["User_Account_ID"].ToString().ToInt();


            //contact.WebPage = dict["Web_Page"].ToString();

            //var person = new Person
            //{
            //    Contact_Id = contactJson.Contact_Id,
            //    Email_Address = contactJson.Email_Address,
            //    NickName = contactJson.Nickname,
            //    First_Name = contactJson.First_Name,
            //    Middle_Name = contactJson.Middle_Name,
            //    Last_Name = contactJson.Last_Name,
            //    Maiden_Name = contactJson.Maiden_Name,
            //    Mobile_Phone = contactJson.Mobile_Phone,
            //    Mobile_Carrier = contactJson.Mobile_Carrier_ID,
            //    Date_of_Birth = contactJson.Date_of_Birth,
            //    Marital_Status_Id = contactJson.Marital_Status_ID,
            //    Gender_Id = contactJson.Gender_ID,
            //    Employer_Name = contactJson.Employer_Name,
            //    Address_Line_1 = contactJson.Address_Line_1,
            //    Address_Line_2 = contactJson.Address_Line_2,
            //    City = contactJson.City,
            //    State = contactJson.State,
            //    Postal_Code = contactJson.Postal_Code,
            //    Anniversary_Date = contactJson.Anniversary_Date,
            //    Foreign_Country = contactJson.Foreign_Country,
            //    County = contactJson.County,
            //    Home_Phone = contactJson.Home_Phone,
            //    Congregation_ID = contactJson.Congregation_ID,
            //    Household_ID = contactJson.Household_ID,
            //    Address_Id = contactJson.Address_ID
            //};

            //var lastName = somethingelse["Last_Name"].ToString();

            return contact;
        }
    }
}