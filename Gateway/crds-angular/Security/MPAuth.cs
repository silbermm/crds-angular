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
using log4net;
using log4net.Config;
using System.Reflection;
using crds_angular.Models.Crossroads;

namespace crds_angular.Security
{
    public class MPAuth : ApiController
    {
        protected readonly log4net.ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected IHttpActionResult Authorized(Func<string,IHttpActionResult> doIt )
        {
            var authorized = Request.Headers.GetValues("Authorization").FirstOrDefault();
            //CookieHeaderValue cookie = Request.Headers.GetCookies("sessionId").FirstOrDefault();
            if (authorized != null && (authorized != "null" || authorized != ""))
            {                
                return doIt(authorized);
            }
            return Unauthorized();   
        }


        //protected IHttpActionResult AuthorizedWithCookie(Func<CookieInfo, IHttpActionResult> doIt)
        //{
        //    CookieHeaderValue cookie = Request.Headers.GetCookies("sessionId").FirstOrDefault();
        //    if (cookie != null && (cookie["sessionId"].Value != "null" || cookie["sessionId"].Value != null))
        //    {
        //        var c = new CookieInfo
        //        {
        //            SessionId = cookie["sessionId"].Value,
        //            UserId = Convert.ToInt32(cookie["userId"].Value),
        //            UserName = cookie["username"].Value
        //        };
        //        return doIt(c);
        //    }
        //    return Unauthorized();
        //}

    }
}