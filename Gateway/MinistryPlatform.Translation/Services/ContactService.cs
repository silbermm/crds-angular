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
            contact.Address_ID = myContact.ToNullableInt("Address_ID");
            contact.Address_Line_1 = myContact.ToString("Address_Line_1");
            contact.Address_Line_2 = myContact.ToString("Address_Line_2");
            contact.Congregation_ID = myContact.ToNullableInt("Congregation_ID");
            contact.Household_ID = myContact.ToInt("Household_ID");
            contact.City = myContact.ToString("City");
            contact.State = myContact.ToString("State");
            contact.Postal_Code = myContact.ToString("Postal_Code");
            contact.Anniversary_Date = myContact.ToDateAsString("Anniversary_Date");
            contact.Contact_ID = myContact.ToInt("Contact_ID");
            contact.Date_Of_Birth = myContact.ToDateAsString("Date_of_Birth");
            contact.Email_Address = myContact.ToString("Email_Address");
            contact.Employer_Name = myContact.ToString("Employer_Name");
            contact.First_Name = myContact.ToString("First_Name");
            contact.Foreign_Country = myContact.ToString("Foreign_Country");
            contact.Gender_ID = myContact.ToInt("Gender_ID");
            contact.Home_Phone = myContact.ToString("Home_Phone");
            contact.Last_Name = myContact.ToString("Last_Name");
            contact.Maiden_Name = myContact.ToString("Maiden_Name");
            contact.Marital_Status_ID = myContact.ToInt("Marital_Status_ID");
            contact.Middle_Name = myContact.ToString("Middle_Name");
            contact.Mobile_Carrier_ID = myContact.ToNullableInt("Mobile_Carrier_ID");
            contact.Mobile_Phone = myContact.ToString("Mobile_Phone");
            contact.Nickname = myContact.ToString("Nickname");

            return contact;
        }
    }
}