using crds_angular.Models;
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
            var contact = crds_angular.Services.TranslationService.GetMyProfile(token);
            var contactJson = TranslationService.DecodeJson(contact);

            var householdId = contactJson.Household_ID;
            var household = crds_angular.Services.TranslationService.GetMyHousehold(householdId);
            var houseJson = TranslationService.DecodeJson(household);

            var addressId = houseJson.Address_ID;
            var addr = crds_angular.Services.TranslationService.GetMyAddress(addressId);
            var addressJson = TranslationService.DecodeJson(addr);

            var house = new Household
            {
                HouseholdPosition = contactJson.Household_Position_ID_Text,
                HomePhone = houseJson.Home_Phone,
                CrossroadsLocation = houseJson.Congregation_ID
            };

            var address = new Address
            {
                Street = addressJson.Address_Line_1,
                Street2 = addressJson.Address_Line_2,
                City = addressJson.City,
                State = addressJson["State/Region"],
                Zip = addressJson.Postal_Code,
                Country = addressJson.Foreign_Country,
                County = addressJson.County
            };

            var person = new Person
            {
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