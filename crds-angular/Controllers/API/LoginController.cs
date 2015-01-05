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
using crds_angular.Security;
using System.Diagnostics;
namespace crds_angular.Controllers.API
{
    public class LoginController : CookieAuth
    {

        [ResponseType(typeof(LoginReturn))]
        [HttpGet]
        [Route("api/authenticated")]
        public IHttpActionResult isAuthenticated()
        {
           
            return Authorized(token =>
            {
                try
                {
                    var personService = new PersonService();
                    var person = personService.getLoggedInUserProfile(token);

                    if (person == null)
                    {
                        return this.Unauthorized();
                    }
                    else
                    {
                        var l = new LoginReturn(token, person.Contact_Id, person.First_Name);
                        return this.Ok(l);
                    }
                }
                catch (Exception e)
                {
                    return this.Unauthorized();
                }
            });
        }

        [ResponseType(typeof(LoginReturn))]
        public IHttpActionResult Post([FromBody]Credentials cred)
        {
            logger.Debug("In the POST");

            if (!EventLog.SourceExists("Crossroads"))
            {
                EventLog.CreateEventSource("Crossroads", "CrossroadsLog");
            }
            var eventLog = new EventLog();
            eventLog.Source = "Crossroads";
            eventLog.Log = "Application";
            eventLog.WriteEntry("In the POST...");
            
            // try to login 
            eventLog.WriteEntry("1. TranslationService.Login - before");
            var token = TranslationService.Login(cred.username, cred.password);
            eventLog.WriteEntry("1. TranslationService.Login - after ");
            if (token == null)
            {
                eventLog.WriteEntry("Token NULL");
                return this.Unauthorized();
            }
            var personService = new PersonService();
            eventLog.WriteEntry("3. getLoggedInUserProfile - before");
            var p = personService.getLoggedInUserProfile(token);
            eventLog.WriteEntry("3. getLoggedInUserProfile - after");
            var r = new LoginReturn
            {
                userToken = token,
                userId = p.Contact_Id,
                username = p.First_Name
            };
            eventLog.WriteEntry("5. OK - before);
            return this.Ok(r);
        }
    }

    public class LoginReturn
    {
        public LoginReturn(){}
        public LoginReturn(string userToken, int userId, string username){
            this.userId = userId;
            this.userToken = userToken;
            this.username = username;
        }
        public string userToken { get; set; }
        public int userId { get; set; }
        public string username { get; set; }
    }

    public class Credentials
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
