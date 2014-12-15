using crds_angular.Models;
using crds_angular.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.SessionState;

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

            SessionStateItemCollection Session = new SessionStateItemCollection();
            CookieHeaderValue cookie = Request.Headers.GetCookies("sessionId").FirstOrDefault();
            if (cookie.ToString() != null)
            {

                string token = cookie["sessionId"].Value;
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
