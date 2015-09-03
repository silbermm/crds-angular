using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Models.MP;
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

        public ProfileController(IPersonService personService)
        {
            this._personService = personService;
        }

        [ResponseType(typeof (Household))]
        [Route("api/profile/household/{householdId}")]
        public IHttpActionResult GetHousehold(int householdId)
        {
            return Authorized(token =>
            {
                var household = _personService.GetHousehold(householdId);
                if (household == null)
                {
                    return Unauthorized();
                }
                return this.Ok();
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

        [ResponseType(typeof(Person))]
        [Route("api/profile/{contactId}")]
        public IHttpActionResult GetProfile(int contactId)
        {
            return Authorized(token =>
            {
                var person = _personService.GetPerson(contactId);
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