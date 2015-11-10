using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Childcare;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Security;
using crds_angular.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class ChildcareController : MPAuth
    {
        private readonly IChildcareService _childcareService;

        public ChildcareController(IChildcareService childcareService)
        {
            _childcareService = childcareService;
        }

        [Route("api/childcare/rsvp")]
        [AcceptVerbs("POST")]
        public IHttpActionResult SaveRsvp([FromBody] ChildcareRsvpDto saveRsvp)
        {
            return Authorized(token =>
            {
                try
                {
                    _childcareService.SaveRsvp(saveRsvp);
                    return Ok();
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("Childcare-SaveRsvp failed", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [ResponseType(typeof(Event))]
        [Route("api/childcare/event/{eventid}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult ChildcareEventById(int eventid)
        {
            return Authorized(token =>
            {
                try
                {
                    return Ok(_childcareService.GetMyChildcareEvent(eventid, token));
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("ChildcareEventById failed", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [ResponseType(typeof(List<FamilyMember>))]
        [Route("api/childcare/eligible-children")]
        [AcceptVerbs("GET")]
        public IHttpActionResult ChildrenEligibleForChildcare()
        {
            return Authorized(token =>
            {
                try
                {
                    return Ok(_childcareService.MyChildren(token));
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("ChildrenEligibleForChildcare failed", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }
    }
}