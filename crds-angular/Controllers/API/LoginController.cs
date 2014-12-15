using crds_angular.Models;
using crds_angular.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.SessionState;
using System.Net.Http.Headers;

namespace crds_angular.Controllers.API
{
    public class LoginController : ApiController
    {

        [ResponseType(typeof(LoginReturn))]
        [HttpGet]
        [Route("api/authenticated")]
        public IHttpActionResult isAuthenticated()
        {
            // try to hit a page with current token and see if we are authenticated
             CookieHeaderValue cookie = Request.Headers.GetCookies("sessionId").FirstOrDefault();
             if (cookie != null && (cookie["sessionId"].Value != null || cookie["sessionId"].Value != "" || cookie["sessionId"].Value != "null"))
             {
                 string token = cookie["sessionId"].Value;
                 var person = PersonService.getLoggedInUserProfile(token); 
                 if (person == null)
                 {
                     return this.Unauthorized();
                 }
                 else
                 {
                     var l = new LoginReturn(token, person.Id);
                     return this.Ok(l);
                 }
             }
             else
             {
                 return Unauthorized();
             }


        }

        [ResponseType(typeof(LoginReturn))]
        public IHttpActionResult Post([FromBody]Credentials cred)
        {
            
            
            // try to login 
            var token = TranslationService.Login(cred.username, cred.password);
            if (token == null)
            {
                return this.Unauthorized();
            } 
            var p = PersonService.getLoggedInUserProfile(token);
            var r = new LoginReturn
            {
                userToken = token,
                userId = p.Id
            };
            return this.Ok(r);
        }
    }

    public class LoginReturn
    {
        public LoginReturn(){}
        public LoginReturn(string userToken, int userId){
            this.userId = userId;
            this.userToken = userToken;
        }
        public string userToken { get; set; }
        public int userId { get; set; }
    }

    public class Credentials
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
