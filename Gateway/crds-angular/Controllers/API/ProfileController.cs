using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using log4net;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class ProfileController : MPAuth
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IPersonService _personService;
        private IAuthenticationService _authenticationService;

        public ProfileController(IPersonService personService, IAuthenticationService authenticationService)
        {
            this._personService = personService;
            this._authenticationService = authenticationService;
        }


        [ResponseType(typeof (List<ServingDay>))]
        [Route("api/profile/servesignup")]
        public IHttpActionResult GetFamilyServeDays()
        {
            return Authorized(token =>
            {
                try
                {
                    var contactId = _authenticationService.GetContactId(token);
                    var servingTeams = _personService.GetServingTeams(contactId, token);
                    var servingDays = _personService.GetServingDays(servingTeams, token);
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

        [ResponseType(typeof (List<FamilyMember>))]
        [Route("api/profile/family")]
        public IHttpActionResult GetFamily()
        {
            return Authorized(token =>
            {
                var contactId = AuthenticationService.GetContactId(token);
                var list = _personService.GetMyImmediateFamily(contactId, token);
                if (list == null)
                {
                    return Unauthorized();
                }
                return this.Ok(list);
            });
        }

        [ResponseType(typeof (Person))]
        [Route("api/profile")]
        public IHttpActionResult GetProfile()
        {
            return Authorized(token =>
            {
                var person = _personService.GetLoggedInUserProfile(token);
                if (person == null)
                {
                    return Unauthorized();
                }
                return this.Ok(person);
            });
        }

        [Route("api/profile")]
        public IHttpActionResult Post([FromBody] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Authorized(t =>
            {
                _personService.SetProfile(t, person);
                return this.Ok();
            });
        }
    }
}