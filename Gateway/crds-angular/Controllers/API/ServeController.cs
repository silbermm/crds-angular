using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Routing.Constraints;
using crds_angular.Exceptions.Models;
using Crossroads.Utilities.Extensions;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Opportunity;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using log4net;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class ServeController : MPAuth
    {
        private readonly IServeService _serveService;

        public ServeController(IServeService serveService)
        {
            _serveService = serveService;
        }

        /// <summary>
        /// Gets the opportunities for a volunteer and his/her family
        /// Accepts optional parameters from and to that specify the date range to fetch
        /// </summary>
        /// <param name="contactId">The volunteers contactId</param>
        /// <param name="from">Optional- The starting date</param>
        /// <param name="to">Optional- The end date</param>
        /// <returns></returns>
        [ResponseType(typeof (List<ServingDay>))]
        [Route("api/serve/family-serve-days/{contactId}")]
        public IHttpActionResult GetFamilyServeDays(int contactId, long from = 0, long to = 0)
        {
            return Authorized(token =>
            {
                try
                {
                    var servingDays = _serveService.GetServingDays(token, contactId, from, to);
                    return Ok(servingDays);
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("Get Family Serve Days Failed", exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [ResponseType(typeof (List<FamilyMember>))]
        [Route("api/serve/family/{contactId}")]
        public IHttpActionResult GetFamily(int contactId)
        {
            return Authorized(token =>
            {
                try
                {
                    var list = _serveService.GetImmediateFamilyParticipants(contactId, token);
                    return Ok(list);
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto("Save RSVP Failed", ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
                
            });
        }

        [ResponseType(typeof(List<QualifiedServerDto>))]
        [Route("api/serve/qualifiedservers/{groupId}/{contactId}")]
        public IHttpActionResult GetQualifiedServers(int groupId, int contactId)
        {
            return Authorized(token =>
            {
                try
                {
                    var list = _serveService.GetQualifiedServers(groupId,contactId, token);
                    return Ok(list);
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto("Save RSVP Failed", ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }

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
                return Ok();
            });
        }

        [ResponseType(typeof(Capacity))]
        [Route("api/serve/opp-capacity")]
        public IHttpActionResult GetOpportunityCapacity([FromUri] OpportunityCapacityDto oppCap)
        {
            return Authorized(token =>
            {
                try
                {
                    var oppCapacity = _serveService.OpportunityCapacity(oppCap.Id, oppCap.EventId, oppCap.Min, oppCap.Max, token);
                    if (oppCapacity == null)
                    {
                        return Unauthorized();
                    }
                    return Ok(oppCapacity);
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("Get Opportunity Capacity Failed", exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }
    }
}
