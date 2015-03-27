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
            var contact = new MyContact();
            contact.AddressId = myContact.ToNullableInt("Address_ID");
            contact.AddressLine1 = myContact.ToString("Address_Line_1");
            contact.AddressLine2 = myContact.ToString("Address_Line_2");
            contact.CongregationId = myContact.ToNullableInt("Congregation_ID");
            contact.HouseholdId = myContact.ToInt("Household_ID");
            contact.City = myContact.ToString("City");
            contact.State = myContact.ToString("State");

            contact.PostalCode = myContact.ToString("Postal_Code");
            contact.AnniversaryDate = myContact.ToDateAsString("Anniversary_Date");
            contact.ContactId = myContact.ToInt("Contact_ID");
            contact.DateOfBirth = myContact.ToDateAsString("Date_of_Birth");
            contact.EmailAddress = myContact.ToString("Email_Address");
            contact.EmployerName = myContact.ToString("Employer_Name");
            contact.FirstName = myContact.ToString("First_Name");
            contact.ForeignCountry = myContact.ToString("Foreign_Country");
            contact.GenderId = myContact.ToInt("Gender_ID");
            contact.HomePhone = myContact.ToString("Home_Phone");
            contact.LastName = myContact.ToString("Last_Name");
            contact.MaidenName = myContact.ToString("Maiden_Name");
            contact.MaritalStatusId = myContact.ToInt("Marital_Status_ID");
            contact.MiddleName = myContact.ToString("Middle_Name");
            contact.MobileCarrierId = myContact.ToNullableInt("Mobile_Carrier_ID");
            contact.MobilePhone = myContact.ToString("Mobile_Phone");
            contact.NickName = myContact.ToString("Nickname");
           

            return contact;
        }
    }
}