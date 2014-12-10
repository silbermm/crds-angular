using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace crds_angular.Controllers.API
{
    public class LoginController : ApiController
    {
        [ResponseType(typeof(Dictionary<string, object>))]
        public IHttpActionResult Post([FromBody]Credentials cred)
        {
            //var response = new LoginResponse();

            //if (claims != null)
            //    if (id == 1)
            //    {
            //        return this.NotFound();
            //    }

            var d = new Dictionary<string, object>
            {
                {"id", 27},
                {"username", cred.username},
                {"Email", "tony.maddox@ingagepartners.com"},
                {"FirstName", "Tony"},
                {"LastName", "Maddox"},
                {"ZipCode", "45242"}
            };


            return this.Ok(d);
        }
    }

    public class Credentials
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
