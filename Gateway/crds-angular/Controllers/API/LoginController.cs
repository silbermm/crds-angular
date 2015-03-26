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
using System.Web.Http.Cors;
using crds_angular.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class LoginController : MPAuth
    {

        private IPersonService _personService;

        public LoginController(IPersonService personService)
        {
            _personService = personService;
        }

        [ResponseType(typeof(LoginReturn))]
        [HttpGet]
        [Route("api/authenticated")]
        public IHttpActionResult isAuthenticated()
        {
           
            return Authorized(token =>
            {
                try
                {
                    //var personService = new PersonService();
                    var person = _personService.GetLoggedInUserProfile(token);

                    if (person == null)
                    {
                        return this.Unauthorized();
                    }
                    else
                    {
                        var l = new LoginReturn(token, person.ContactId, person.FirstName);
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
            // try to login 
            var token = TranslationService.Login(cred.username, cred.password);
            if (token == null)
            {
                return this.Unauthorized();
            }
            //var personService = new PersonService();
            var p = _personService.GetLoggedInUserProfile(token);
            var r = new LoginReturn
            {
                userToken = token,
                userId = p.ContactId,
                username = p.FirstName
            };
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
