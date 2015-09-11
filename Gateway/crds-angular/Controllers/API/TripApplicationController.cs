using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.Trip;
using crds_angular.Security;
using crds_angular.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class TripApplicationController : MPAuth
    {
        private readonly ITripService _tripService;

        public TripApplicationController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [AcceptVerbs("POST")]
        [Route("api/trip-application")]
        public IHttpActionResult Save([FromBody] TripApplicationDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
                var dataError = new ApiErrorDto("Save Trip Application Data Invalid", new InvalidOperationException("Invalid Save Data" + errors));
                throw new HttpResponseException(dataError.HttpResponseMessage);
            }

            return Authorized(token =>
            {
                try
                {
                    _tripService.SaveApplication(dto, token);
                    return Ok();
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("Save Failed", exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }
    }
}
