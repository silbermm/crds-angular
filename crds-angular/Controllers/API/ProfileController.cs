using crds_angular.Models;
using crds_angular.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.SessionState;
using crds_angular.Security;
using System.Diagnostics;

namespace crds_angular.Controllers.API
{
    public class ProfileController : CookieAuth
    {
        [ResponseType(typeof (Person))]
        [Route("api/profile")]
        public IHttpActionResult GetProfile()
        {       
            return Authorized(token => {
                var personService = new PersonService();
                var person = personService.getLoggedInUserProfile(token);
                if (person == null)
                {
                    return Unauthorized();
                }
                return this.Ok(person);
            });
        }

        [ResponseType(typeof(List<Models.Crossroads.Skill>))]
        [Route("api/myskills")]
        public IHttpActionResult GetMySkills()
        {
            return Authorized(token =>
            {
                var cookie = Request.Headers.GetCookies("userId").FirstOrDefault();
                if (cookie != null && (cookie["userId"].Value != "null" || cookie["userId"].Value != null))
                {
                    Debug.WriteLine("userId");
                    var contactId = int.Parse(cookie["userId"].Value);


                    var personService = new PersonService();
                    var skills = personService.getLoggedInUserSkills(contactId, token);
                    if (skills == null)
                    {
                        return Unauthorized();
                    }
                    return this.Ok(skills);
                }
                else
                {
                    return this.Unauthorized();
                }
            });
        }

        [Route("api/profile")]
        public IHttpActionResult Post([FromBody] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Authorized(t => {
                var personService = new PersonService();
                personService.setProfile(t, person);
                return this.Ok();
            });

        }
    }

    

   

   
    
}
