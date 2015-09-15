using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class ContactService : BaseService, IContactService
    {
        private readonly int _contactsPageId;
        private readonly int _householdsPageId;
        private readonly int _securityRolesSubPageId;
        private readonly int _congregationDefaultId;
        private readonly int _householdDefaultSourceId;
        private readonly int _householdPositionDefaultId;
        private readonly int _addressesPageId;
        private readonly ILog _logger = LogManager.GetLogger(typeof(ContactService));

        private readonly IMinistryPlatformService _ministryPlatformService;

        public ContactService(IMinistryPlatformService ministryPlatformService, IAuthenticationService authenticationService, IConfigurationWrapper configuration)
            : base(authenticationService, configuration)
        {
            _ministryPlatformService = ministryPlatformService;

            _householdsPageId = configuration.GetConfigIntValue("Households");
            _securityRolesSubPageId = configuration.GetConfigIntValue("SecurityRolesSubPageId");
            _congregationDefaultId = configuration.GetConfigIntValue("Congregation_Default_ID");
            _householdDefaultSourceId = configuration.GetConfigIntValue("Household_Default_Source_ID");
            _householdPositionDefaultId = configuration.GetConfigIntValue("Household_Position_Default_ID");
            _addressesPageId = configuration.GetConfigIntValue("Addresses");
            _contactsPageId = configuration.GetConfigIntValue("Contacts");
        }

        public string GetContactEmail(int contactId)
        {
            try
            {
                var recordsDict = _ministryPlatformService.GetRecordDict(_contactsPageId, contactId, ApiLogin());

                var contactEmail = recordsDict["Email_Address"].ToString();

                return contactEmail;
            }
            catch (NullReferenceException)
            {
                _logger.Debug(string.Format("Trying to email address of {0} failed", contactId));
                return string.Empty;
            }
        }

        public MyContact GetContactById(int contactId)
        {
            var searchString = string.Format(",\"{0}\"", contactId);
            
            var pageViewRecords = _ministryPlatformService.GetPageViewRecords("AllIndividualsWithContactId", ApiLogin(), searchString);

            if (pageViewRecords.Count > 1)
            {
                throw new ApplicationException("GetContactById returned multiple records");
            }

            return ParseProfileRecord(pageViewRecords[0]);
        }

        public Household GetHouseholdById(int householdId)
        {
            var token = ApiLogin();
            var recordsDict = _ministryPlatformService.GetPageViewRecords("HouseholdProfile", token, householdId.ToString());
            if (recordsDict.Count > 1)
            {
                throw new ApplicationException("GetHouseholdById returned multiple records");
            }

            var record = recordsDict.FirstOrDefault();
            var house = new Household
            {
                AddressLine1 = record.ToString("Address_Line_1"),
                AddressLine2 = record.ToString("Address_Line_2"),
                City = record.ToString("City"),
                State = record.ToString("State/Region"),
                PostalCode = record.ToString("Postal_Code"),
                HomePhone = record.ToString("Home_Phone"),
                ForeignCountry = record.ToString("Foreign_Country"),
                County = record.ToString("County"),
                CongregationId = record.ToNullableInt("Congregation_ID"),
                HouseholdId = record.ToInt("Household_ID")
            };
            var familyRecords = _ministryPlatformService.GetSubpageViewRecords("HouseholdMembers", house.HouseholdId, token);
            var family = familyRecords.Select(famRec => new HouseholdMember
            {
                ContactId = famRec.ToInt("Contact_ID"), 
                FirstName = famRec.ToString("First_Name"), 
                Nickname = famRec.ToString("Nickname"), 
                LastName = famRec.ToString("Last_Name"), 
                DateOfBirth = famRec.ToDate("Date_of_Birth"),
                HouseholdPosition = famRec.ToString("Household_Position")
            }).ToList();

            house.HouseholdMembers = family;
            return house;
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
                Household_Name = recordsDict.ToString("Household_Name"),
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

        public int CreateContactForNewDonor(ContactDonor contactDonor)
        {
            return (CreateContact(contactDonor));
        }

        public int CreateContactForGuestGiver(string emailAddress, string displayName)
        {
            var contactDonor = new ContactDonor
            {
                Details = new ContactDetails
                {
                    DisplayName = displayName,
                    EmailAddress = emailAddress
                }
            };
            return (CreateContact(contactDonor));
        }

        private int CreateContact(ContactDonor contactDonor)
        {
            var token = ApiLogin();

            var emailAddress = contactDonor.Details.EmailAddress;
            var displayName = contactDonor.Details.DisplayName;
            int? householdId = null;
            if (contactDonor.Details.HasAddress)
            {
                try
                {
                    householdId = CreateHouseholdAndAddress(displayName, contactDonor.Details.Address, token);
                }
                catch (Exception e)
                {
                    var msg = string.Format("Error creating household and address for emailAddress: {0} displayName: {1}",
                                            emailAddress,
                                            displayName);
                    _logger.Error(msg, e);
                    throw (new ApplicationException(msg, e));
                }
            }

            var contactDictionary = new Dictionary<string, object>
            {
                {"Email_Address", emailAddress},
                {"Company", false},
                {"Display_Name", displayName},
                {"Nickname", displayName},
                {"Household_ID", householdId},
                {"Household_Position_ID", _householdPositionDefaultId}
            };

            try
            {
                return(_ministryPlatformService.CreateRecord(_contactsPageId, contactDictionary, token));
            }
            catch (Exception e)
            {
                var msg = string.Format("Error creating contact, emailAddress: {0} displayName: {1}",
                                        emailAddress,
                                        displayName);
                _logger.Error(msg, e);
                throw (new ApplicationException(msg, e));
            }
        }

        private int CreateHouseholdAndAddress(string householdName, PostalAddress address, string apiToken)
        {
            var addressDictionary = new Dictionary<string, object>
                {
                    { "Address_Line_1", address.Line1 },
                    { "Address_Line_2", address.Line2 },
                    { "City", address.City },
                    { "State/Region", address.State },
                    { "Postal_Code", address.PostalCode }
                };
            var addressId = _ministryPlatformService.CreateRecord(_addressesPageId, addressDictionary, apiToken);

            var householdDictionary = new Dictionary<string, object>
                {
                    {"Household_Name", householdName},
                    {"Congregation_ID", _congregationDefaultId},
                    {"Household_Source_ID", _householdDefaultSourceId},
                    {"Address_ID", addressId}
                };

            return(_ministryPlatformService.CreateRecord(_householdsPageId, householdDictionary, apiToken));
        }

        public IList<int> GetContactIdByRoleId(int roleId, string token)
        {
            var records = _ministryPlatformService.GetSubPageRecords(_securityRolesSubPageId, roleId, token);

            return records.Select(record => (int) record["Contact_ID"]).ToList();
        }
    }
}