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
        [Route("api/eventTool")]
        [ResponseType(typeof (EventToolDto))]
        public IHttpActionResult Get()
        {
            var room = new EventRoomDto();
            var equipment = new EventRoomEquipmentDto();
            var retVal = new EventToolDto();
            room.Equipment.Add(equipment);
            retVal.Rooms.Add(room);
            return Ok(retVal);
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
    }
}