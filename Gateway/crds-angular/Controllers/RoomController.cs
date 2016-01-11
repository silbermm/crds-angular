using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using crds_angular.Exceptions.Models;
using crds_angular.Security;
using crds_angular.Services.Interfaces;

namespace crds_angular.Controllers
{
    public class RoomController : MPAuth
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [Route("api/room/location/{locationId}")]
        public IHttpActionResult GetRoomByLocationId(int locationId)
        {
            return Authorized(t =>
            {
                try
                {
                    var rooms = _roomService.GetRoomsByLocationId(locationId,t);

                    return Ok(rooms);
                }
                catch (Exception e)
                {
                    var msg = "Error getting Rooms by Location " + locationId;
                    logger.Error(msg, e);
                    var apiError = new ApiErrorDto(msg, e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }
    }
}
