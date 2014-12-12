using crds_angular.Models;
using crds_angular.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace crds_angular.Controllers.API
{
    public class LoginController : ApiController
    {
        [ResponseType(typeof(Person))]
        public IHttpActionResult Post([FromBody]Credentials cred)
        {
            // try to login 
            var token = TranslationService.Login(cred.username, cred.password);
            if (token == null)
            {
                return this.Unauthorized();
            } 
            //var p = PersonService.getLoggedInUserProfile(token);
            return this.Ok("{}");
        }
    }

    public class Credentials
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
