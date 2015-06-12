using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Security;
using crds_angular.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class VolunteerApplicationController : MPAuth
    {
        private readonly IVolunteerApplicationService _volunteerApplicationService;

        public VolunteerApplicationController(IVolunteerApplicationService volunteerApplicationService)
        {
            _volunteerApplicationService = volunteerApplicationService;
        }

        [ResponseType(typeof(List<FamilyMember>))]
        [Route("api/volunteer-application/family/{contactId}")]
        public IHttpActionResult GetFamily(int contactId)
        {
            return Authorized(token =>
            {
                try
                {
                    var family = _volunteerApplicationService.FamilyThatUserCanSubmitFor(contactId, token);
                    return Ok(family);
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto("Volunteer Application GET Family", ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }

            });
        }


        //[ResponseType(typeof (List<FamilyMember>))]
        //[Route("api/volunteer-application/family/{contactId}")]
        //public IHttpActionResult Family(int contactId)
        //{
        //    return Authorized(token =>
        //    {
        //        try
        //        {
        //            var family = _volunteerApplicationService.FamilyThatUserCanSubmitFor(contactId, token);
        //            return Ok(family);
        //        }
        //        catch (Exception exception)
        //        {
        //            var apiError = new ApiErrorDto("Volunteer Application GET Family", exception);
        //            throw new HttpResponseException(apiError.HttpResponseMessage);
        //        }
        //    });
        //}
    }
}