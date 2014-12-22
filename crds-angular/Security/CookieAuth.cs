using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Description;
using System.Web.SessionState;
using System.Diagnostics;

namespace crds_angular.Security
{
    public class CookieAuth : ApiController
    {
        protected IHttpActionResult Authorized(Func<string,IHttpActionResult> doIt )
        {
            CookieHeaderValue cookie = Request.Headers.GetCookies("sessionId").FirstOrDefault();
            if (cookie != null && (cookie["sessionId"].Value != "null" || cookie["sessionId"].Value != null))
            {
                Debug.WriteLine("cookieauth");
                return doIt(cookie["sessionId"].Value);
            }
            Debug.WriteLine("I am unauthorized now???");
            return Unauthorized();   
        }

    }
}