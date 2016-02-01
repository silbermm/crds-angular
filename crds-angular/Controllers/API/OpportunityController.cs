using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.Opportunity;
using crds_angular.Security;
using Crossroads.Utilities.Extensions;
using MinistryPlatform.Models;
using MinistryPlatform.Models.DTO;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class OpportunityController : MPAuth
    {
        private readonly IOpportunityService _opportunityService;

        public OpportunityController(IOpportunityService opportunityService)
        {
            _opportunityService = opportunityService;
        }

        [ResponseType(typeof (int))]
        [Route("api/opportunity/{id}")]
        public IHttpActionResult Post(int id, [FromBody] string stuff)
        {
            var comments = string.Format("Request on {0}", DateTime.Now.ToString(CultureInfo.CurrentCulture));

            return Authorized(token =>
            {
                try
                {
                    var opportunityId = _opportunityService.RespondToOpportunity(token, id, comments);
                    return Ok(opportunityId);
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            });
        }

        [ResponseType(typeof (int))]
        [Route("api/opportunity/save-qualified-server")]
        public IHttpActionResult Post([FromBody] RespondToOpportunityDto opportunityResponse)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(val => val.Errors)
                    .Aggregate("", (current, err) => current + err.Exception.Message);
                var dataError = new ApiErrorDto("POST Data Invalid",
                    new InvalidOperationException("Invalid POST Data" + errors));
                throw new HttpResponseException(dataError.HttpResponseMessage);
            }

            try
            {
                if (opportunityResponse.Participants.Count > 0)
                {
                    _opportunityService.RespondToOpportunity(opportunityResponse);
                }
            }
            catch (Exception exception)
            {
                var apiError = new ApiErrorDto("Opportunity POST failed", exception);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
            return Ok();
        }

        [ResponseType(typeof (List<long>))]
        [Route("api/opportunity/getAllOpportunityDates/{id}")]
        public IHttpActionResult GetAllOpportunityDates(int id)
        {
            var oppDates = new List<long>();
            return Authorized(token =>
            {
                var opportunities = _opportunityService.GetAllOpportunityDates(id, token);
                oppDates.AddRange(opportunities.Select(opp => opp.ToUnixTime()));
                return Ok(oppDates);
            });
        }

        [ResponseType(typeof (Dictionary<string, long>))]
        [Route("api/opportunity/getLastOpportunityDate/{id}")]
        public IHttpActionResult GetLastOpportunityDate(int id)
        {
            return
                Authorized(
                    token =>
                        Ok(new Dictionary<string, long>
                        {
                            {"date", _opportunityService.GetLastOpportunityDate(id, token).ToUnixTime()}
                        }));
        }

        [ResponseType(typeof (OpportunityGroup))]
        [Route("api/opportunity/getGroupParticipantsForOpportunity/{id}")]
        public IHttpActionResult GetGroupParticipantsForOpportunity(int id)
        {
            return Authorized(token =>
            {
                var group = _opportunityService.GetGroupParticipantsForOpportunity(id, token);
                var oppGrp = Mapper.Map<Group, OpportunityGroup>(group);
                return Ok(oppGrp);
            });
        }

        [ResponseType(typeof (OpportunityResponseDto))]
        [Route("api/opportunity/getResponseForOpportunity/{id}/{contactId}")]
        public IHttpActionResult GetResponseForOpportunity(int id, int contactId)
        {
            try
            {
                var response = _opportunityService.GetOpportunityResponse(contactId, id);
                var mapped = Mapper.Map<Response, OpportunityResponseDto>(response);
                return Ok(mapped);
            }
            catch (Exception exception)
            {
                var apiError = new ApiErrorDto("Get Response For Opportunity", exception);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }
    }
}