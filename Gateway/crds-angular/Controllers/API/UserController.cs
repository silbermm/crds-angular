using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using crds_angular.Models.Crossroads;
using crds_angular.Services;
using Crossroads.Utilities.Services;

namespace crds_angular.Controllers.API
{
    public class UserController : ApiController
    {
        public IHttpActionResult Post([FromBody] User user)
        {
            var configurationWrapper = new ConfigurationWrapper();
            var accountService = new AccountService(configurationWrapper);

            var returnvalue = accountService.RegisterPerson(user);

                return this.Ok(returnvalue);
        }
    }
}