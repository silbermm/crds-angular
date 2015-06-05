using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.VolunteerApplication;
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

        [Route("api/volunteer-application")]
        public IHttpActionResult Post([FromBody] VolunteerApplicationDto application)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
                var dataError = new ApiErrorDto("RSVP Data Invalid", new InvalidOperationException("Invalid POST Data" + errors));
                throw new HttpResponseException(dataError.HttpResponseMessage);
            }

            return Authorized(token =>
            {
                try
                {
                    _volunteerApplicationService.Save(application);
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("Volunteer Application POST Failed", exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
                return Ok();
            });
        }
    }
}
