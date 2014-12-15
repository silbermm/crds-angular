using crds_angular.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Services
{
    public static class PersonService
    {

        public static Person getLoggedInUserProfile(String token)
        {
            var contactId = MinistryPlatform.Translation.AuthenticationService.GetContactId(token);
            JArray contact = MinistryPlatform.Translation.Services.GetPageRecordService.GetRecord(455, contactId, token);
            var contactJson = TranslationService.DecodeJson(contact.ToString());


            Household house = new Household();
            Address address = new Address();
            try
            {
                var householdId = contactJson.Household_ID;
                var household = crds_angular.Services.TranslationService.GetMyHousehold(householdId, token);
                var houseJson = TranslationService.DecodeJson(household);
                house.HouseholdPosition = contactJson.Household_Position_ID_Text;
                house.HomePhone = houseJson.Home_Phone;
                house.CrossroadsLocation = houseJson.Congregation_ID;
                var addressId = houseJson.Address_ID;
                var addr = crds_angular.Services.TranslationService.GetMyAddress(addressId, token);
                var addressJson = TranslationService.DecodeJson(addr);
                address.Street = addressJson.Address_Line_1;
                address.Street2 = addressJson.Address_Line_2;
                address.City = addressJson.City;
                address.State = addressJson["State/Region"];
                address.Zip = addressJson.Postal_Code;
                address.Country = addressJson.Foreign_Country;
                address.County = addressJson.County;
            }
            catch
            {

            }
       
            var person = new Person
            {
                Id = contactId,
                Email = contactJson.Email_Address,
                NickName = contactJson.Nickname,
                FirstName = contactJson.First_Name,
                MiddleName = contactJson.Middle_Name,
                LastName = contactJson.Last_Name,
                MaidenName = contactJson.Maiden_Name,
                MobilePhone = contactJson.Mobile_Phone,
                ServiceProvider = contactJson.Mobile_Carrier_Text,
                BirthDate = contactJson.Date_of_Birth,
                MaritalStatus = contactJson.Marital_Status_ID_Text,
                Gender = contactJson.Gender_ID_Text,
                Employer = contactJson.Employer_Name,
                CrossroadsStartDate = contactJson.Anniversary_Date,
                Household = house,
                Address = address
            };

            return person;
        
        }
        

        

    }
}