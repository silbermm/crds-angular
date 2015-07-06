using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using crds_angular.Security;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models.DTO;

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
                    var roles = _personService.GetLoggedInUserRoles(token);

                    if (person == null)
                    {
                        return this.Unauthorized();
                    }
                    else
                    {
                        var l = new LoginReturn(token, person.ContactId, person.FirstName, person.EmailAddress, roles);
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
            var userRoles = _personService.GetLoggedInUserRoles(token);
            if (token == null)
            {
                return this.Unauthorized();
            }
            var p = _personService.GetLoggedInUserProfile(token);
            var r = new LoginReturn
            {
                userToken = token,
                userId = p.ContactId,
                username = p.FirstName,
                userEmail = p.EmailAddress,
                roles = userRoles
            };
            return this.Ok(r);
        }
    }

    public class LoginReturn
    {
        public LoginReturn(){}
        public LoginReturn(string userToken, int userId, string username, string userEmail, List<RoleDto> roles){
            this.userId = userId;
            this.userToken = userToken;
            this.username = username;
            this.roles = roles;
        }
        public string userToken { get; set; }
        public int userId { get; set; }
        public string username { get; set; }
        public string userEmail { get; set;  }
        public List<RoleDto> roles { get; set; }
    }

    public class Credentials
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
