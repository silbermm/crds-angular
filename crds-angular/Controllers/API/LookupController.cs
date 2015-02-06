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
using MinistryPlatform.Translation.Models;
using crds_angular.Models.MP;
using System.Configuration;
using System.Web.Http.Results;
using MinistryPlatform.Translation;
using WebGrease.Css.Ast.Selectors;

namespace crds_angular.Controllers.API
{
    public class LookupController : CookieAuth
    {
        [ResponseType(typeof(System.Web.Helpers.DynamicJsonArray))]
        [Route("api/lookup/{pageId}")]
        public IHttpActionResult Get(int pageId)
        {
            return Authorized(t => {
                var contact = TranslationService.GetLookup(pageId, t);
                var json = DecodeJson(contact.ToString());

                return this.Ok(json);
            });
        }

        [ResponseType(typeof(List<Dictionary<string,object>>))]
        [Route("api/lookup/{table?}")]
        [HttpGet]
        public IHttpActionResult Lookup(string table)
        {
            return Authorized(t => {
                var ret = new List<Dictionary<string, object>>();
                switch (table) {
                    case "genders"  :
                        ret = MinistryPlatform.Translation.Services.LookupService.Genders(t);
                        break;
                    case "maritalstatus" :
                        ret = MinistryPlatform.Translation.Services.LookupService.MaritalStatus(t);
                        break;
                    case "serviceproviders" :
                        ret = MinistryPlatform.Translation.Services.LookupService.ServiceProviders(t);
                        break;
                    case "countries" :
                        ret = MinistryPlatform.Translation.Services.LookupService.Countries(t);
                        break;
                    case "states" :
                        var json = TranslationService.GetStates(t);
                        ret = DecodeJson(json);
                        break;
                    case "crossroadslocations" :
                        ret = MinistryPlatform.Translation.Services.LookupService.CrossroadsLocations(t);
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

        [ResponseType(typeof(System.Web.Helpers.DynamicJsonArray))]
        [HttpGet]
        [Route("api/lookup/{lookup?}")]
        public IHttpActionResult Get(string lookup)
        {
            return Authorized(t =>
            {
                if(lookup == "states"){
                    var states = TranslationService.GetStates(t);
                    var json = DecodeJson(states.ToString());
                     return this.Ok(json);
                }                
                else 
                {
                    return this.BadRequest();
                }               
            });
        }


        [HttpGet]
        [Route("api/lookup/{email?}")]
        public IHttpActionResult EmailExists(string email)
        {
            //TODO let's clean this up
            var returnvalue =  AuthorizedWithCookie(t =>
            {
                
                var exists = MinistryPlatform.Translation.Services.LookupService.EmailSearch(email, t.SessionId);
                if (exists.Count == 0 || Convert.ToInt32(exists["dp_RecordID"]) == t.UserId  )
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            });
            
            if (returnvalue is UnauthorizedResult)
            {
                string token = AuthenticationService.authenticate(ConfigurationManager.AppSettings["ApiUser"], ConfigurationManager.AppSettings["ApiPass"]);
                var exists = MinistryPlatform.Translation.Services.LookupService.EmailSearch(email, token);
                if (exists.Count == 0)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {

                return returnvalue;
            }
        }

        protected static dynamic DecodeJson(string json)
        {
            var obj = System.Web.Helpers.Json.Decode(json);
            if (obj.GetType() == typeof(System.Web.Helpers.DynamicJsonArray))
            {
                dynamic[] array = obj;
                return array;
            }
            return null;
        }
    }
}
