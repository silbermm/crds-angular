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

        /// <summary>
        /// Ensure that a user is authenticated before executing the given labmda expression.  The expression will
        /// have a reference to the user's authentication token (the value of the "Authorization" cookie).  If
        /// the user is not authenticated, an UnauthorizedResult will be returned.
        /// </summary>
        /// <param name="doIt">A labmda expression to execute if the user is authenticated</param>
        /// <returns>An IHttpActionResult from the "doIt" expression, or UnauthorizedResult if the user is not authenticated.</returns>
        protected IHttpActionResult Authorized(Func<string,IHttpActionResult> doIt )
        {
            try
            {
                var authorized = Request.Headers.GetValues("Authorization").FirstOrDefault();
                //CookieHeaderValue cookie = Request.Headers.GetCookies("sessionId").FirstOrDefault();
                if (authorized != null && (authorized != "null" || authorized != ""))
                {
                    return doIt(authorized);
                }
                return Unauthorized();
            }
            catch (System.InvalidOperationException)
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Execute the labmda expression "actionWhenAuthorized" if the user is authenticated, or execute the expression
        /// "actionWhenNotAuthorized" if the user is not authenticated.  If authenticated, the "actionWhenAuthorized"
        /// expression will have a reference to the user's authentication token (the value of the "Authorization" cookie).
        /// </summary>
        /// <param name="actionWhenAuthorized">A labmda expression to execute if the user is authenticated</param>
        /// <param name="actionWhenNotAuthorized">A labmda expression to execute if the user is NOT authenticated</param>
        /// <returns>An IHttpActionResult from the labmda expression that was executed.</returns>
        protected IHttpActionResult Authorized(Func<string, IHttpActionResult> actionWhenAuthorized, Func<IHttpActionResult> actionWhenNotAuthorized)
        {
            try
            {
                var authorized = Request.Headers.GetValues("Authorization").FirstOrDefault();
                if (authorized != null && (authorized != "null" || authorized != ""))
                {
                    return actionWhenAuthorized(authorized);
                }
                else
                {
                    return actionWhenNotAuthorized();
                }
            }
            catch (System.InvalidOperationException)
            {
                return actionWhenNotAuthorized();
            }
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