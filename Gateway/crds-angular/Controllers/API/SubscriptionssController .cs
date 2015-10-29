using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Security;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class SubscriptionsController : MPAuth
    {
        private readonly MPInterfaces.IMinistryPlatformService _ministryPlatformService;
        private readonly MPInterfaces.IAuthenticationService _authenticationService;

        public SubscriptionsController(MPInterfaces.IMinistryPlatformService ministryPlatformService, MPInterfaces.IAuthenticationService authenticationService)
        {
            _ministryPlatformService = ministryPlatformService;
            _authenticationService = authenticationService;
        }

        [ResponseType(typeof (List<Dictionary<string, object>>))]
        [Route("api/subscriptions")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            return (Authorized(token =>
            {
                var contactId = _authenticationService.GetContactId(token);
                var subscriptions = _ministryPlatformService.GetSubPageRecords("SubscriptionsSubPage", contactId, token);
                return (Ok(subscriptions));
            }));
        }

        [ResponseType(typeof(List<Dictionary<string, object>>))]
        [Route("api/profile/subscriptions")]
        [HttpPost]
        public IHttpActionResult Post()
        {
            //var token = _apiUserService.GetToken();
            //var publications = _ministryPlatformService.GetRecordsDict("Publications", token, ",,,,True", "8 asc");
            return this.Ok();
        }  
    }
}