using crds_angular.Models.Crossroads;
using crds_angular.Models.Json;
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
using System.Diagnostics;

namespace crds_angular.Controllers.API
{
    public class AccountController : CookieAuth
    {

        

        [ResponseType(typeof (AccountInfo))]
        public IHttpActionResult Get()
        {

            return Authorized( token =>
            {
                try
                {
                    AccountService accountService = new AccountService();
                    AccountInfo info = accountService.getAccountInfo(token);
                    Debug.WriteLine("in the account controller");
                    return Ok(info);
                }
                catch (Exception e)
                {
                    return BadRequest();
                }
                
            });
            
        }

        [Route("api/account/password")]
        [HttpPost]
        public IHttpActionResult UpdatePassword([FromBody] NewPassword password)
        {

            return Authorized(token =>
            {
                AccountService accountService = new AccountService();
                if (accountService.ChangePassword(token, password.password))
                {
                    return Ok();
                }
                return BadRequest();
            });

        }

        public IHttpActionResult Put([FromBody]AccountInfo accountInfo)
        {

            return Authorized(token =>
            {
                AccountService accountService = new AccountService();
                accountService.SaveCommunicationPrefs(token, accountInfo);
                return Ok();
            });
            
        }

    }
}
