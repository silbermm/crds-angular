using System;
using System.Collections.Generic;
using AutoMapper;
using crds_angular.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Models.MP;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models.DTO;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Attribute = MinistryPlatform.Models.Attribute;

namespace crds_angular.Services
{
    public class PersonService : MinistryPlatformBaseService, IPersonService
    {
        private IContactService _contactService;

        public PersonService(IContactService contactService)
        {
            this._contactService = contactService;
        }

        public void SetProfile(String token, Person person)
        {
            var contactDictionary = getDictionary(person.GetContact());
            var householdDictionary = getDictionary(person.GetHousehold());
            var addressDictionary = getDictionary(person.GetAddress());
            addressDictionary.Add("State/Region", addressDictionary["State"]);

            MinistryPlatformService.UpdateRecord(AppSetting("MyContact"), contactDictionary, token);

            if (addressDictionary["Address_ID"] != null)
            {
                //address exists, update it
                MinistryPlatformService.UpdateRecord(AppSetting("MyAddresses"), addressDictionary, token);
            }
            else
            {
                //address does not exist, create it, then attach to household
                var addressId = MinistryPlatformService.CreateRecord(AppSetting("MyAddresses"), addressDictionary, token);
                householdDictionary.Add("Address_ID", addressId);
            }
            MinistryPlatformService.UpdateRecord(AppSetting("MyHousehold"), householdDictionary, token);
        }

        public List<Skill> GetLoggedInUserSkills(int contactId, string token)
        {
            return GetSkills(contactId, token);
        }

        public Household GetHousehold(int householdId)
        {
            var house = _contactService.GetHouseholdById(householdId);
            var household = new Household
            {
                Address_Line_1 = house.AddressLine1,
                Address_Line_2 = house.AddressLine2,
                City = house.City,
                Congregation_ID = house.CongregationId,
                County = house.County,
                Foreign_Country = house.ForeignCountry,
                Home_Phone = house.HomePhone,
                Postal_Code = house.PostalCode,
                State = house.State,
                Household_ID = house.HouseholdId
            };

            foreach (var fam in house.HouseholdMembers)
            {
                var member = new HouseholdMember
                {
                    Contact_ID = fam.ContactId,
                    Date_Of_Birth = fam.DateOfBirth,
                    First_Name = fam.FirstName,
                    Nickname = fam.Nickname,
                    Last_Name = fam.LastName,
                    Household_Position = fam.HouseholdPosition
                };
                household.Household_Members.Add(member);
            }

            return household;
        }

        public Person GetPerson(int contactId)
        {
            var contact = _contactService.GetContactById(contactId);
            var person = new Person();

            person.ContactId = contact.Contact_ID;
            person.EmailAddress = contact.Email_Address;
            person.NickName = contact.Nickname;
            person.FirstName = contact.First_Name;
            person.MiddleName = contact.Middle_Name;
            person.LastName = contact.Last_Name;
            person.MaidenName = contact.Maiden_Name;
            person.MobilePhone = contact.Mobile_Phone;
            person.MobileCarrierId = contact.Mobile_Carrier;
            person.DateOfBirth = contact.Date_Of_Birth;
            person.MaritalStatusId = contact.Marital_Status_ID;
            person.GenderId = contact.Gender_ID;
            person.EmployerName = contact.Employer_Name;
            person.AddressLine1 = contact.Address_Line_1;
            person.AddressLine2 = contact.Address_Line_2;
            person.City = contact.City;
            person.State = contact.State;
            person.PostalCode = contact.Postal_Code;
            person.AnniversaryDate = contact.Anniversary_Date;
            person.ForeignCountry = contact.Foreign_Country;
            person.HomePhone = contact.Home_Phone;
            person.CongregationId = contact.Congregation_ID;
            person.HouseholdId = contact.Household_ID;
            person.HouseholdName = contact.Household_Name;
            person.AddressId = contact.Address_ID;
            person.Age = contact.Age;

            return person;
        }

        public List<RoleDto> GetLoggedInUserRoles(string token)
        {
            return GetMyRecords.GetMyRoles(token);
        }

        public Person GetLoggedInUserProfile(String token)
        {
            var contact = _contactService.GetMyProfile(token);

            var person = new Person();

            person.ContactId = contact.Contact_ID;
            person.EmailAddress = contact.Email_Address;
            person.NickName = contact.Nickname;
            person.FirstName = contact.First_Name;
            person.MiddleName = contact.Middle_Name;
            person.LastName = contact.Last_Name;
            person.MaidenName = contact.Maiden_Name;
            person.MobilePhone = contact.Mobile_Phone;
            person.MobileCarrierId = contact.Mobile_Carrier;
            person.DateOfBirth = contact.Date_Of_Birth;
            person.MaritalStatusId = contact.Marital_Status_ID;
            person.GenderId = contact.Gender_ID;
            person.EmployerName = contact.Employer_Name;
            person.AddressLine1 = contact.Address_Line_1;
            person.AddressLine2 = contact.Address_Line_2;
            person.City = contact.City;
            person.State = contact.State;
            person.PostalCode = contact.Postal_Code;
            person.AnniversaryDate = contact.Anniversary_Date;
            person.ForeignCountry = contact.Foreign_Country;
            person.HomePhone = contact.Home_Phone;
            person.CongregationId = contact.Congregation_ID;
            person.HouseholdId = contact.Household_ID;
            person.HouseholdName = contact.Household_Name;
            person.AddressId = contact.Address_ID;
            person.Age = contact.Age;


            return person;
        }

        private List<Skill> GetSkills(int recordId, string token)
        {
            var attributes = GetMyRecords.GetMyAttributes(recordId, token);

            var skills =
                Mapper.Map<List<Attribute>, List<Skill>>(attributes);

            return skills;
        }
    }
}