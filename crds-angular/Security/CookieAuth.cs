using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Description;
using System.Web.SessionState;

namespace crds_angular.Security
{
    public class CookieAuth : ApiController
    {
        protected IHttpActionResult Authorized(Func<string,IHttpActionResult> doIt )
        {
            CookieHeaderValue cookie = Request.Headers.GetCookies("sessionId").FirstOrDefault();
            if (cookie != null && (cookie["sessionId"].Value != "null" || cookie["sessionId"].Value != null))
            {
                return doIt(cookie["sessionId"].Value);
            }
            return Unauthorized();   
        }

    }
}