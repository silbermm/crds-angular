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
using System.Threading;
using crds_angular.Models.Crossroads;
using crds_angular.Util;
using Microsoft.Owin;
using MinistryPlatform.Translation.Services;

namespace crds_angular.Security
{
    public class MPAuth : ApiController
    {
        protected readonly log4net.ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Ensure that a user is authenticated before executing the given lambda expression.  The expression will
        /// have a reference to the user's authentication token (the value of the "Authorization" cookie).  If
        /// the user is not authenticated, an UnauthorizedResult will be returned.
        /// </summary>
        /// <param name="doIt">A lambda expression to execute if the user is authenticated</param>
        /// <returns>An IHttpActionResult from the "doIt" expression, or UnauthorizedResult if the user is not authenticated.</returns>
        protected IHttpActionResult Authorized(Func<string,IHttpActionResult> doIt )
        {
            return (Authorized(doIt, () => { return (Unauthorized()); }));
        }

        /// <summary>
        /// Execute the lambda expression "actionWhenAuthorized" if the user is authenticated, or execute the expression
        /// "actionWhenNotAuthorized" if the user is not authenticated.  If authenticated, the "actionWhenAuthorized"
        /// expression will have a reference to the user's authentication token (the value of the "Authorization" cookie).
        /// </summary>
        /// <param name="actionWhenAuthorized">A lambda expression to execute if the user is authenticated</param>
        /// <param name="actionWhenNotAuthorized">A lambda expression to execute if the user is NOT authenticated</param>
        /// <returns>An IHttpActionResult from the lambda expression that was executed.</returns>
        protected IHttpActionResult Authorized(Func<string, IHttpActionResult> actionWhenAuthorized, Func<IHttpActionResult> actionWhenNotAuthorized)
        {
            try
            {
                var authorized = Request.Headers.GetValues("Authorization").FirstOrDefault();
                //var refreshToken = Request.Headers.GetValues("RefreshToken").FirstOrDefault();

                //var authData = AuthenticationService.RefreshToken(refreshToken);
                //var sessionCookie = new CookieHeaderValue("sessionId", authData["token"].ToString());
                //var refreshCookie = new CookieHeaderValue("refreshToken", authData["refreshToken"].ToString());
                //var testCookie = new CookieHeaderValue("dan", "wins");
                //var cookies = new List<CookieHeaderValue>();
                //testCookie.Domain = "localhost";
                //testCookie.Expires = DateTimeOffset.Now.AddDays(1);
                //testCookie.Path = "/";
                //cookies.Add(sessionCookie);
                //cookies.Add(refreshCookie);
                //cookies.Add(testCookie);
                //cookie.Expires = DateTimeOffset.Now.AddDays(1);
                //cookie.Domain = Request.RequestUri.Host;
                //cookie.Path = "/";
                //var token = authData["token"].ToString();
                //var exp = authData["exp"].ToString();
                //var newrefreshToken = authData["refreshToken"].ToString();
                if (authorized != null && (authorized != "null" || authorized != ""))
                {
                    //var result = new CookieResult(cookies, actionWhenAuthorized(authorized));
                    //return result;

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