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

        [ResponseType(typeof(Dictionary<string, object>))]
        [Route("api/lookup/{pageId}")]
        public IHttpActionResult Get(int pageId)
        {
            var contact = crds_angular.Services.MinistryPlatform.GetLookup(pageId);
            var json = DecodeJson(contact);

            return this.Ok(json);
        }

        private static dynamic DecodeJson(string json)
        {
            var obj = System.Web.Helpers.Json.Decode(json);
            if (obj.GetType() == typeof(System.Web.Helpers.DynamicJsonArray))
            {
                dynamic[] array = obj;
                if (array.Length == 1)
                {
                    return array[0];
                }
            }
            //should probably throw error here
            return null;
        }
    }
}
