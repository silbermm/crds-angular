using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace crds_angular.Controllers.API
{
    public class ProfileController : ApiController
    {
        [ResponseType(typeof(Person))]
        public IHttpActionResult Get(int pageId, int recordId)
        {
            return this.BadRequest();
        }

        [ResponseType(typeof (Person))]
        [Route("api/profile/{pageId}")]
        public IHttpActionResult Get(int pageId)
        {

            var contact = crds_angular.Services.TranslationService.GetMyProfile();
            var contactJson = DecodeJson(contact);

            var householdId = contactJson.Household_ID;
            var household = crds_angular.Services.TranslationService.GetMyHousehold(householdId);
            var houseJson = DecodeJson(household);

            var addressId = houseJson.Address_ID;
            var addr = crds_angular.Services.TranslationService.GetMyAddress(addressId);
            var addressJson = DecodeJson(addr);

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

            return this.Ok(person);
        }

        private static dynamic DecodeJson(string json)
        {
            var obj = System.Web.Helpers.Json.Decode(json);
            if (obj.GetType() == typeof(System.Web.Helpers.DynamicJsonArray))
            {
                dynamic[] array = obj;
                if (array.Length == 1)
                {
                    return array[0];
                }
            }
            //should probably throw error here
            return null;
        }
    }

    public class Person
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MaidenName { get; set; }
        public string NickName { get; set; }
        public string MobilePhone { get; set; }
        public string ServiceProvider { get; set; }
        public string BirthDate { get; set; }
        public string MaritalStatus { get; set; }
        public string Gender { get; set; }
        public string Employer { get; set; }
        public string CrossroadsStartDate { get; set; }
        public Household Household { get; set; }
        public Address Address { get; set; }
    }

    public class Household
    {
        public string HouseholdPosition { get; set; }
        public string HomePhone { get; set; }
        public int CrossroadsLocation { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
    }
    
}
