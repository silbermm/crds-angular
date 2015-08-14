using System;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.Trip;
using crds_angular.Security;
using crds_angular.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class TripController : MPAuth
    {
        private readonly ITripService _tripService;

        public TripController(ITripService tripService)
        {
            _tripService = tripService;
        }

        //[AcceptVerbs("GET")]
        //[ResponseType(typeof(TripGroupDto))]
        //[Route("api/trip/groups/{eventId}")]
        //public IHttpActionResult GetGroupsForEvent(int eventId)
        //{
        //    return Authorized(token =>
        //    {
        //        try
        //        {
        //            var groups = _tripService.GetGroupsByEventId(eventId);
        //            return Ok(groups);
        //        }
        //        catch (Exception ex)
        //        {
        //            var apiError = new ApiErrorDto("GetGroupsForEvent Failed", ex);
        //            throw new HttpResponseException(apiError.HttpResponseMessage);
        //        }
        //    });
        //}

        [AcceptVerbs("GET")]
        [ResponseType(typeof(TripFormResponseDto))]
        [Route("api/trip/form-responses/{selectionId}/{selectionCount}")]
        public IHttpActionResult TripFormResponses(int selectionId, int selectionCount)
        {
            return Authorized(token =>
            {
                try
                {
                    var groups = _tripService.GetFormResponses(selectionId, selectionCount);
                    return Ok(groups);
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto("GetGroupsForEvent Failed", ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        public IHttpActionResult SaveParticipants([FromBody] SaveTripParticipantsDto dto)
        {
            
            _tripService.SaveParticipants(dto);
        }

        [AcceptVerbs("GET")]
        [ResponseType(typeof (TripParticipantDto))]
        [Route("api/trip/search/{search}")]
        public IHttpActionResult Search(string search)
        {
            try
            {
                var list = _tripService.Search(search);
                return Ok(list);
            }
            catch (Exception ex)
            {
                var apiError = new ApiErrorDto("Trip Search Failed", ex);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        [AcceptVerbs("GET")]
        [ResponseType(typeof (MyTripsDTO))]
        [Route("api/trip/mytrips/{contactId}")]
        public IHttpActionResult MyTrips(int contactId)
        {
            return Authorized(token =>
            {
                try
                {
                    var trips = _tripService.GetMyTrips(contactId, token);
                    return Ok(trips);
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto("Failed to retrieve My Trips info", ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }
    }
}