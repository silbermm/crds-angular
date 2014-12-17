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
    public class LookupController : ApiController
    {

        [ResponseType(typeof(System.Web.Helpers.DynamicJsonArray))]
        [Route("api/lookup/{pageId}")]
        public IHttpActionResult Get(int pageId)
        {
            CookieHeaderValue cookie = Request.Headers.GetCookies("sessionId").FirstOrDefault();
            if (cookie != null && (cookie["sessionId"].Value != "null" || cookie["sessionId"].Value != null))
            {
                string token = cookie["sessionId"].Value;
                var contact = TranslationService.GetLookup(pageId, token);
                var json = DecodeJson(contact.ToString());

                return this.Ok(json);
            }
            else
            {
                return this.Unauthorized();
            }
        }

        private static dynamic DecodeJson(string json)
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
