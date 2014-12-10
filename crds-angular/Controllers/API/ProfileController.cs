using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            var json = DecodeJson(contact.ToString());
            
            var person = new Person
            {
                Email = json.Email_Address,
                NickName = json.Nickname,
                FirstName = json.First_Name,
                MiddleName = json.Middle_Name,
                LastName = json.Last_Name,
                MaidenName = json.Maiden_Name,
                MobilePhone = json.Mobile_Phone,
                ServiceProvider = json.Mobile_Carrier_Text,
                BirthDate = json.Date_of_Birth,
                MaritalStatus = json.Marital_Status_ID_Text,
                Gender = json.Gender_ID_Text,
                Employer = json.Employer_Name,
                CrossroadsStartDate = json.Anniversary_Date
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
    }
    
}
