using crds_angular.Models.Crossroads;
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
                    AccountInfo info = AccountService.getAccountInfo(token);
                    return Ok(info);
                }
                catch (Exception e)
                {
                    return BadRequest();
                }
                
            });
            
        }

    }
}
