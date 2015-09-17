using System;
using System.Linq;
using System.Messaging;
using System.Web.Http;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.Trip;
using crds_angular.Models.Json;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Messaging.Interfaces;

namespace crds_angular.Controllers.API
{
    public class TripApplicationController : MPAuth
    {
        private readonly ITripService _tripService;
        private readonly IMessageFactory _messageFactory;
        private readonly MessageQueue _eventQueue;

        public TripApplicationController(ITripService tripService, IConfigurationWrapper configuration, IMessageFactory messageFactory, IMessageQueueFactory messageQueueFactory)
        {
            _tripService = tripService;
            _messageFactory = messageFactory;

            var eventQueueName = configuration.GetConfigValue("TripApplicationEventQueueName");
            _eventQueue = messageQueueFactory.CreateQueue(eventQueueName, QueueAccessMode.Send);
            _messageFactory = messageFactory;
        }

        [Route("api/trip-application"), HttpPost]
        public IHttpActionResult Save([FromBody] TripApplicationDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
                var dataError = new ApiErrorDto("Save Trip Application Data Invalid", new InvalidOperationException("Invalid Save Data" + errors));
                throw new HttpResponseException(dataError.HttpResponseMessage);
            }

            //return Authorized(token =>
            //{
            TripApplicationResponseDto response = null;
            try
            {
                var message = _messageFactory.CreateMessage(dto);
                _eventQueue.Send(message, MessageQueueTransactionType.None);
                response = new TripApplicationResponseDto
                {
                    Message = "Queued event for asynchronous processing"
                };
                //_tripService.SaveApplication(dto, token);
                //return Ok();
            }
            catch (Exception e)
            {
                const string msg = "Unexpected error processing Trip Application Save";
                var responseDto = new TripApplicationResponseDto()
                {
                    Exception = new ApplicationException(msg, e),
                    Message = msg
                };
                return (RestHttpActionResult<TripApplicationResponseDto>.ServerError(responseDto));
            }
            return ((IHttpActionResult) RestHttpActionResult<TripApplicationResponseDto>.Ok(response));
            //});
        }
    }
}