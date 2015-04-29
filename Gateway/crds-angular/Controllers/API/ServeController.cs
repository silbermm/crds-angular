using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Extenstions;
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
        //private IPersonService _personService;
        private IServeService _serveService;
        private IAuthenticationService _authenticationService;

        public ServeController(IServeService serveService, IAuthenticationService authenticationService)
        {
            this._serveService = serveService;
            this._authenticationService = authenticationService;
        }

        [ResponseType(typeof (List<ServingDay>))]
        [Route("api/serve/family-serve-days")]
        public IHttpActionResult GetFamilyServeDays()
        {
            return Authorized(token =>
            {
                try
                {
                    var servingDays = _serveService.GetServingDays(token);
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
        [Route("api/serve/family")]
        public IHttpActionResult GetFamily()
        {
            return Authorized(token =>
            {
                var contactId = _authenticationService.GetContactId(token);
                var list = _serveService.GetMyImmediateFamily(contactId, token);
                if (list == null)
                {
                    return Unauthorized();
                }
                return this.Ok(list);
            });
        }

        [Route("api/serve/save-rsvp")]
        public IHttpActionResult SaveRsvp([FromBody] SaveRsvpDto saveRsvp)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
                var dataError = new ApiErrorDto("RSVP Data Invalid", new InvalidOperationException("Invalid RSVP Data" + errors));
                throw new HttpResponseException(dataError.HttpResponseMessage);
            }
            //validate request
            if (saveRsvp.StartDateUnix <= 0)
            {
                var dateError = new ApiErrorDto("StartDate Invalid", new InvalidOperationException("Invalid Date"));
                throw new HttpResponseException(dateError.HttpResponseMessage);
            }

            return Authorized(token =>
            {
                try
                {
                    _serveService.SaveServeRsvp(token, saveRsvp.ContactId, saveRsvp.OpportunityId,
                        saveRsvp.EventTypeId, saveRsvp.StartDateUnix.FromUnixTime(),
                        saveRsvp.EndDateUnix.FromUnixTime(), saveRsvp.SignUp, saveRsvp.AlternateWeeks);
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("Save RSVP Failed", exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
                return this.Ok();
            });
        }
    }
}