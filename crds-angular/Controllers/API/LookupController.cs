using crds_angular.Security;
using crds_angular.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;

namespace crds_angular.Controllers.API
{
    public class LookupController : CookieAuth
    {
        [ResponseType(typeof(System.Web.Helpers.DynamicJsonArray))]
        [Route("api/lookup/{pageId}")]
        public IHttpActionResult Get(int pageId)
        {
            return Authorized(t => {
                var contact = TranslationService.GetLookup(pageId, t);
                var json = DecodeJson(contact.ToString());

                

                return this.Ok(json);
            });
        }

        [HttpGet]
        [Route("api/lookup/{email?}")]
        public IHttpActionResult EmailExists(string email)
        {
            return AuthorizedWithCookie(t =>
            {
                
                var exists = MinistryPlatform.Translation.Services.LookupService.EmailSearch(email, t.SessionId);
                if (exists.Count == 0 || Convert.ToInt32(exists["dp_RecordID"]) == t.UserId  )
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            });
        }

        protected static dynamic DecodeJson(string json)
        {
            var obj = System.Web.Helpers.Json.Decode(json);
            if (obj.GetType() == typeof(System.Web.Helpers.DynamicJsonArray))
            {
                dynamic[] array = obj;
                return array;
            }
            return null;
        }
    }
}
