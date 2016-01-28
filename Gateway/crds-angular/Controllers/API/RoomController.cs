using System;
using System.Web.Http;
using crds_angular.Exceptions.Models;
using crds_angular.Security;
using crds_angular.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class RoomController : MPAuth
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [Route("api/room/layouts")]
        public IHttpActionResult GetLayouts()
        {
            return Authorized(t =>
            {
                try
                {
                    var rooms = _roomService.GetRoomLayouts();

                    return Ok(rooms);
                }
                catch (Exception e)
                {
                    var msg = "Error getting Layouts ";
                    logger.Error(msg, e);
                    var apiError = new ApiErrorDto(msg, e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }
    }
}