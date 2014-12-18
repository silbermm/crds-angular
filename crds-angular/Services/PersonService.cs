using crds_angular.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace crds_angular.Services
{
    public static class PersonService
    {

        public static void setProfile(String token, Person person)
        {
            var dictionary = person.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => !p.IsMarkedWith<NotInDictionaryAttribute>())
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(person, null));

            MinistryPlatform.Translation.Services.UpdatePageRecordService.UpdateRecord(455, dictionary, token);
            MinistryPlatform.Translation.Services.UpdatePageRecordService.UpdateRecord(465, getHouseholdDictionary(person.Household), token);       
        }

        private static Dictionary<string, object> getHouseholdDictionary(Household household)
        {
            var dictionary = household.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => !p.IsMarkedWith<NotInDictionaryAttribute>())
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(household, null));
            return dictionary;
        }

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
                house.Household_ID = householdId.ToString();
                house.Household_Position = contactJson.Household_Position_ID_Text;
                house.Home_Phone = houseJson.Home_Phone;
                house.Congregation_ID = houseJson.Congregation_ID.ToString();
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
            catch (Exception ex)
            {
                //Console.Write(ex.Message);
                throw new Exception(ex.Message);
            }
       
            var person = new Person
            {
                Contact_Id = contactJson.Contact_Id,
                Email_Address = contactJson.Email_Address,
                NickName = contactJson.Nickname,
                First_Name = contactJson.First_Name,
                Middle_Name = contactJson.Middle_Name,
                Last_Name = contactJson.Last_Name,
                Maiden_Name = contactJson.Maiden_Name,
                Mobile_Phone = contactJson.Mobile_Phone,
                Mobile_Carrier = contactJson.Mobile_Carrier,
                Date_of_Birth = contactJson.Date_of_Birth,
                Marital_Status_Id = contactJson.Marital_Status_ID,
                Gender_Id = contactJson.Gender_ID,
                Employer_Name = contactJson.Employer_Name,
                Anniversary_Date = contactJson.Anniversary_Date,
                Address = address,
                Household = house
            };
            
            return person;
        
        }
        

        

    }
}