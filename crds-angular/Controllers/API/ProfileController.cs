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
        [ResponseType(typeof (Person))]
        public IHttpActionResult Get()
        {

            var contact = crds_angular.Services.MinistryPlatform.GetMyProfile();
            var json = DecodeJson(contact);

            var p = new Person
            {
                Email = json.Email_Address,
                NickName = json.Nickname,
                FirstName = json.First_Name,
                MiddleName = null,
                LastName = json.Last_Name,
                MaidenName = null,
                MobilePhone = json.Mobile_Phone,
                ServiceProvider = null,
                BirthDate = json.Date_of_Birth,
                MaritalStatus = json.Marital_Status,
                Gender = json.Gender,
                Employer = null,
                CrossroadsStartDate = null
            };

            return this.Ok(p);
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
