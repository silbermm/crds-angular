using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using log4net;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class ContactService : BaseService, IContactService
    {
        private readonly int _myProfilePageId = AppSettings("MyProfile");
        private readonly int contactsPageId = AppSettings("Contacts");
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private IMinistryPlatformService _ministryPlatformService;

        public ContactService(IMinistryPlatformService ministryPlatformService)
        {
            this._ministryPlatformService = ministryPlatformService;
        }

        public string GetContactEmail(int contactId)
        {
            try
            {
                var recordsDict = _ministryPlatformService.GetRecordDict(contactsPageId, contactId, apiLogin());

                var contactEmail = recordsDict["Email_Address"].ToString();

                return contactEmail;
            }
            catch (NullReferenceException ex)
            {
                logger.Debug(String.Format("Trying to email address of {0} failed", contactId));
                return string.Empty;
            }
        }

        public MyContact GetContactById(int contactId)
        {
            var searchString = string.Format(",{0}", contactId);
            var pageViewRecords = _ministryPlatformService.GetPageViewRecords("AllIndividualsWithContactId", apiLogin(), searchString);

            if (pageViewRecords.Count > 1)
            {
                throw new ApplicationException("GetContactById returned multiple records");
            }

            return ParseProfileRecord(pageViewRecords[0]);
        }

        public MyContact GetMyProfile(string token)
        {
            var recordsDict = _ministryPlatformService.GetRecordsDict("MyProfile", token);

            if (recordsDict.Count > 1)
            {
                throw new ApplicationException("GetMyProfile returned multiple records");
            }

            var contact = ParseProfileRecord(recordsDict[0]);

            return contact;
        }

        private static MyContact ParseProfileRecord(Dictionary<string, object> recordsDict)
        {
            var contact = new MyContact
            {
                Address_ID = recordsDict.ToNullableInt("Address_ID"),
                Address_Line_1 = recordsDict.ToString("Address_Line_1"),
                Address_Line_2 = recordsDict.ToString("Address_Line_2"),
                Congregation_ID = recordsDict.ToNullableInt("Congregation_ID"),
                Household_ID = recordsDict.ToInt("Household_ID"),
                City = recordsDict.ToString("City"),
                State = recordsDict.ToString("State"),
                Postal_Code = recordsDict.ToString("Postal_Code"),
                Anniversary_Date = recordsDict.ToDateAsString("Anniversary_Date"),
                Contact_ID = recordsDict.ToInt("Contact_ID"),
                Date_Of_Birth = recordsDict.ToDateAsString("Date_of_Birth"),
                Email_Address = recordsDict.ToString("Email_Address"),
                Employer_Name = recordsDict.ToString("Employer_Name"),
                First_Name = recordsDict.ToString("First_Name"),
                Foreign_Country = recordsDict.ToString("Foreign_Country"),
                Gender_ID = recordsDict.ToNullableInt("Gender_ID"),
                Home_Phone = recordsDict.ToString("Home_Phone"),
                Last_Name = recordsDict.ToString("Last_Name"),
                Maiden_Name = recordsDict.ToString("Maiden_Name"),
                Marital_Status_ID = recordsDict.ToNullableInt("Marital_Status_ID"),
                Middle_Name = recordsDict.ToString("Middle_Name"),
                Mobile_Carrier = recordsDict.ToNullableInt("Mobile_Carrier_ID"),
                Mobile_Phone = recordsDict.ToString("Mobile_Phone"),
                Nickname = recordsDict.ToString("Nickname"),
                Age = recordsDict.ToInt("Age")
            };
            return contact;
        }

        public int CreateContactForGuestGiver(string emailAddress, string displayName)
        {
            var contactDictionary = new Dictionary<string, object>();
            contactDictionary["Email_Address"] = emailAddress;
            contactDictionary["Company"] = false; // default
            contactDictionary["Display_Name"] = displayName;
            contactDictionary["Nickname"] = displayName;
            contactDictionary["Household_Position_ID"] =
                Convert.ToInt32(ConfigurationManager.AppSettings["Household_Position_Default_ID"]);

            try
            {
                var contactId = WithApiLogin<int>(apiToken =>
                    _ministryPlatformService.CreateRecord(contactsPageId, contactDictionary, apiToken)
                    );
                return (contactId);
            }
            catch (Exception e)
            {
                throw (new ApplicationException(
                    String.Format("Error creating contact for guest giver, emailAddress: {0} displayName: {1}",
                        emailAddress, displayName), e));
            }
        }
    }
}