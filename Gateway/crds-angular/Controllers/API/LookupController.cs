using System;
using System.Collections.Generic;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.Http.Results;
using crds_angular.Security;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services;

namespace crds_angular.Controllers.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class LookupController : MPAuth
    {
        private IConfigurationWrapper _configurationWrapper;

        public LookupController(IConfigurationWrapper configurationWrapper)
        {
            this._configurationWrapper = configurationWrapper;
        }

        [ResponseType(typeof (List<Dictionary<string, object>>))]
        [Route("api/lookup/{table?}")]
        [HttpGet]
        public IHttpActionResult Lookup(string table)
        {
            return Authorized(t =>
            {
                var ret = new List<Dictionary<string, object>>();
                switch (table)
                {
                    case "genders":
                        ret = LookupService.Genders(t);
                        break;
                    case "maritalstatus":
                        ret = LookupService.MaritalStatus(t);
                        break;
                    case "serviceproviders":
                        ret = LookupService.ServiceProviders(t);
                        break;
                    case "countries":
                        ret = LookupService.Countries(t);
                        break;
                    case "states":
                        ret = LookupService.States(t);
                        break;
                    case "crossroadslocations":
                        ret = LookupService.CrossroadsLocations(t);
                        break;
                    case "workteams":
                        ret = LookupService.WorkTeams(t);
                        break;
                    default:
                        break;
                }
                if (ret.Count == 0)
                {
                    return this.BadRequest(string.Format("table: {0}", table));
                }
                return Ok(ret);
            });
        }

        [ResponseType(typeof (Dictionary<string, object>))]
        [HttpGet]
        [Route("api/lookup/{userId}/find/{email?}")]
        public IHttpActionResult EmailExists(int userId, string email)
        {
            //TODO let's clean this up
            var authorizedWithCookie = Authorized(t =>
            {
                var exists = LookupService.EmailSearch(email, t);
                if (exists.Count == 0 || Convert.ToInt32(exists["dp_RecordID"]) == userId)
                {
                    return Ok();
                }
                return BadRequest();
            });

            if (authorizedWithCookie is UnauthorizedResult)
            {
                var apiUser = _configurationWrapper.GetEnvironmentVarAsString("API_USER");
                var apiPassword = _configurationWrapper.GetEnvironmentVarAsString("API_PASSWORD");

                var authData = AuthenticationService.authenticate(apiUser, apiPassword);
                var token = authData["token"].ToString();
                var exists = LookupService.EmailSearch(email, token.ToString());
                if (exists.Count == 0)
                {
                    return Ok();
                }
                return BadRequest();
            }
            return authorizedWithCookie;
        }

        protected static dynamic DecodeJson(string json)
        {
            var obj = System.Web.Helpers.Json.Decode(json);
            if (obj.GetType() != typeof (DynamicJsonArray)) return null;
            dynamic[] array = obj;
            return array;
        }
    }
}