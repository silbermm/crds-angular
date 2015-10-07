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
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;

namespace crds_angular.Controllers.API
{
    public class AccountController : MPAuth
    {      
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        

        [ResponseType(typeof (AccountInfo))]
        public IHttpActionResult Get()
        {

            return Authorized( token =>
            {
                try
                {
                    AccountInfo info = _accountService.getAccountInfo(token);
                    Debug.WriteLine("in the account controller");
                    return Ok(info);
                }
                catch (Exception )
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
     
                if (_accountService.ChangePassword(token, password.password))
                {
                    return Ok();
                }
                return BadRequest();
            });

        }

        public IHttpActionResult Post([FromBody]AccountInfo accountInfo)
        {

            return Authorized(token =>
            {   
                _accountService.SaveCommunicationPrefs(token, accountInfo);
                return Ok();
            });
            
        }

    }
}
