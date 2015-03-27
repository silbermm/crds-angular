using System;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class ContactService : BaseService, IContactService
    {
        private readonly int _myProfilePageId = AppSettings("MyProfile");

        private IMinistryPlatformService _ministryPlatformService;


        public ContactService(IMinistryPlatformService ministryPlatformService)
        {
            this._ministryPlatformService = ministryPlatformService;
        }

        public MyContact GetMyProfile(string token)
        {
            var recordsDict = _ministryPlatformService.GetRecordsDict(_myProfilePageId, token);

            if (recordsDict.Count > 1)
            {
                throw  new ApplicationException("GetMyProfile returned multiple records");
            }

            var myContact = recordsDict[0];
            var contact = new MyContact
            {
                AddressId = myContact["Address_ID"].ToString().ToNullableInt(),
                AddressLine1 = myContact["Address_Line_1"].ToString(),
                AddressLine2 = myContact["Address_Line_2"].ToString(),
                CongregationId = myContact["Congregation_ID"].ToString().ToNullableInt(),
                HouseholdId = myContact["Household_ID"].ToString().ToInt(),
                City = myContact["City"].ToString(),
                State = myContact["State"].ToString(),
                PostalCode = myContact["Postal_Code"].ToString(),
                AnniversaryDate = myContact["Anniversary_Date"].ToString().DateToString(),
                ContactId = myContact["Contact_ID"].ToString().ToInt(),
                DateOfBirth = myContact["Date_of_Birth"].ToString(),
                EmailAddress = myContact["Email_Address"].ToString(),
                EmployerName = myContact["Employer_Name"].ToString(),
                FirstName = myContact["First_Name"].ToString(),
                ForeignCountry = myContact["Foreign_Country"].ToString(),
                GenderId = myContact["Gender_ID"].ToString().ToInt(),
                HomePhone = myContact["Home_Phone"].ToString(),
                LastName = myContact["Last_Name"].ToString(),
                MaidenName = myContact["Maiden_Name"].ToString(),
                MaritalStatusId = myContact["Marital_Status_ID"].ToString().ToInt(),
                MiddleName = myContact["Middle_Name"].ToString(),
                MobileCarrierId = myContact["Mobile_Carrier_ID"].ToString().ToNullableInt(),
                MobilePhone = myContact["Mobile_Phone"].ToString(),
                NickName = myContact["Nickname"].ToString()
            };

            return contact;
        }
    }
}