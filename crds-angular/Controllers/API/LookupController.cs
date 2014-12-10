using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            var contact = TranslationService.GetLookup(pageId);
            var json = DecodeJson(contact);

            return this.Ok(json);
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
