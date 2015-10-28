using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Models.Crossroads;
using crds_angular.Security;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class PublicationsController : MPAuth
    {
        private IMinistryPlatformService _ministryPlatformService;        
        private readonly IApiUserService _apiUserService;

        public PublicationsController(IMinistryPlatformService ministryPlatformService, IApiUserService apiUserService)
        {
            _ministryPlatformService = ministryPlatformService;            
            _apiUserService = apiUserService;
        }

        [ResponseType(typeof(List<Dictionary<string, object>>))]
        [Route("api/publications/")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            var token = _apiUserService.GetToken();
            var publications = _ministryPlatformService.GetRecordsDict("Publications", token, ",,,,True", "8 asc");
            return this.Ok(publications);
        }
    }
}