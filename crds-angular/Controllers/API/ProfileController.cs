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
            
            return Authorized(t => {
                var personService = new PersonService();
                Person person = personService.getLoggedInUserProfile(t);
                if (person == null)
                {
                    return Unauthorized();
                }
                Debug.WriteLine ("in the profile controller");
                return this.Ok(person);
            });
        }

        [Route("api/profile")]
        public IHttpActionResult Put([FromBody]Person person)
        {  
            return Authorized(token =>
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var personService = new PersonService();
                personService.setProfile(token, person);

                return this.Ok();
            });
            
        }

       
    }

    

   

   
    
}
