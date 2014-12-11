using crds_angular.Models;
using crds_angular.Services;
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
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("X-Auth"))
            {
                string token = headers.GetValues("X-Auth").First();
                var person = PersonService.getLoggedInUserProfile(token);
                if (person == null)
                {
                    return this.Unauthorized();
                }
                return this.Ok(person);
            }
            else
            {
                return this.Unauthorized();
            }
            
        }

       
    }

    

   

   
    
}
