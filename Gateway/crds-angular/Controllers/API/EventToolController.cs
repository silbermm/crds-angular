using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.Events;
using crds_angular.Security;
using MinistryPlatform.Translation.Services.Interfaces;
using IEventService = crds_angular.Services.Interfaces.IEventService;

namespace crds_angular.Controllers.API
{
    public class EventToolController : MPAuth
    {
        private readonly IApiUserService _apiUserService;
        private readonly IEventService _eventService;

        public EventToolController(IApiUserService apiUserService, IEventService eventService)
        {
            _eventService = eventService;
            _apiUserService = apiUserService;
        }

        [AcceptVerbs("GET")]
        [Route("api/eventTool/{eventId}")]
        [ResponseType(typeof (EventToolDto))]
        public IHttpActionResult Get(int eventId)
        {
            return Authorized(token =>
            {
                try
                {
                    var eventReservation = _eventService.GetEventReservation(eventId);
                    return Ok(eventReservation);
                }
                catch (Exception e)
                {
                    var msg = "EventToolController: GET " + eventId;
                    logger.Error(msg, e);
                    var apiError = new ApiErrorDto(msg, e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [AcceptVerbs("POST")]
        [Route("api/eventTool")]
        public IHttpActionResult Post([FromBody] EventToolDto eventReservation)
        {
            if (ModelState.IsValid)
            {
                return Authorized(token =>
                {
                    try
                    {
                        _eventService.CreateEventReservation(eventReservation);
                        return Ok();
                    }
                    catch (Exception e)
                    {
                        var msg = "EventToolController: POST " + eventReservation.Title;
                        logger.Error(msg, e);
                        var apiError = new ApiErrorDto(msg, e);
                        throw new HttpResponseException(apiError.HttpResponseMessage);
                    }
                });
            }
            var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
            var dataError = new ApiErrorDto("Event Data Invalid", new InvalidOperationException("Invalid Event Data" + errors));
            throw new HttpResponseException(dataError.HttpResponseMessage);
        }

        [AcceptVerbs("PUT")]
        [Route("api/eventTool/{eventId}")]
        public IHttpActionResult Put([FromBody] EventToolDto eventReservation, int eventId)
        {
            if (ModelState.IsValid)
            {
                return Authorized(token =>
                {
                    try
                    {
                        if (eventId == 0)
                        {
                            throw new ApplicationException("Invalid Event Id");
                        }
                        _eventService.UpdateEventReservation(eventReservation, eventId);
                        return Ok();
                    }
                    catch (Exception e)
                    {
                        var msg = "EventToolController: PUT " + eventReservation.Title;
                        logger.Error(msg, e);
                        var apiError = new ApiErrorDto(msg, e);
                        throw new HttpResponseException(apiError.HttpResponseMessage);
                    }
                });
            }
            var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
            var dataError = new ApiErrorDto("Event Data Invalid", new InvalidOperationException("Invalid Event Data" + errors));
            throw new HttpResponseException(dataError.HttpResponseMessage);
        }
    }
}