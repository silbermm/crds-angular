using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using log4net;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class ServeController : MPAuth
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IPersonService _personService;
        private IAuthenticationService _authenticationService;

        public ServeController(IPersonService personService, IAuthenticationService authenticationService)
        {
            this._personService = personService;
            this._authenticationService = authenticationService;
        }

        [ResponseType(typeof(List<ServingDay>))]
        [Route("api/serve/family-serve-days")]
        public IHttpActionResult GetFamilyServeDays()
        {
            return Authorized(token =>
            {
                try
                {
                    var servingDays = _personService.GetServingDays(token);
                    if (servingDays == null)
                    {
                        return Unauthorized();
                    }
                    return this.Ok(servingDays);
                }
                catch (Exception e)
                {
                    return this.BadRequest(e.Message);
                }
            });
        }

        [ResponseType(typeof(List<FamilyMember>))]
        [Route("api/serve/family")]
        public IHttpActionResult GetFamily()
        {
            return Authorized(token =>
            {
                var contactId = _authenticationService.GetContactId(token);
                var list = _personService.GetMyImmediateFamily(contactId, token);
                if (list == null)
                {
                    return Unauthorized();
                }
                return this.Ok(list);
            });
        }

    }
}
