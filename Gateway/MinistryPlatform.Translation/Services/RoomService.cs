using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.EventReservations;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{

    public class RoomService : BaseService, IRoomService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly ILog _logger = LogManager.GetLogger(typeof (RoomService));

        public RoomService(IMinistryPlatformService ministryPlatformService, IAuthenticationService authenticationService, IConfigurationWrapper configuration)
            : base(authenticationService, configuration)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public List<RoomReservationDto> GetRoomReservations(int eventId)
        {
            var token = ApiLogin();
            var search = string.Format(",{0}", eventId);
            var records = _ministryPlatformService.GetPageViewRecords("GetRoomReservations", token, search);

            return records.Select(record => new RoomReservationDto
            {
                Cancelled = record.ToBool("Cancelled"),
                EventRoomId = record.ToInt("Event_Room_ID"),
                Hidden = record.ToBool("Hidden"),
                Notes = record.ToString("Notes"),
                RoomId = record.ToInt("Room_ID"),
                RoomLayoutId = record.ToInt("Room_Layout_ID")
            }).ToList();
        }

        public int CreateRoomReservation(RoomReservationDto roomReservation, string token)
        {
            var roomReservationPageId = _configurationWrapper.GetConfigIntValue("RoomReservationPageId");
            var reservationDictionary = new Dictionary<string, object>
            {
                {"Event_ID", roomReservation.EventId},
                {"Room_ID", roomReservation.RoomId},
                {"Room_Layout_ID", roomReservation.RoomLayoutId},
                {"Notes", roomReservation.Notes},
                {"Hidden", roomReservation.Hidden},
                {"Cancelled", roomReservation.Cancelled}
            };

            try
            {
                return (_ministryPlatformService.CreateRecord(roomReservationPageId, reservationDictionary, token, true));
            }
            catch (Exception e)
            {
                var msg = string.Format("Error creating Room Reservation, roomReservation: {0}", roomReservation);
                _logger.Error(msg, e);
                throw (new ApplicationException(msg, e));
            }
        }

        public void UpdateRoomReservation(RoomReservationDto roomReservation)
        {
            var token = ApiLogin();
            var roomReservationPageId = _configurationWrapper.GetConfigIntValue("RoomReservationPageId");
            var reservationDictionary = new Dictionary<string, object>
            {
                {"Event_ID", roomReservation.EventId},
                {"Event_Room_ID", roomReservation.EventRoomId},
                {"Room_ID", roomReservation.RoomId},
                {"Room_Layout_ID", roomReservation.RoomLayoutId},
                {"Notes", roomReservation.Notes},
                {"Hidden", roomReservation.Hidden},
                {"Cancelled", roomReservation.Cancelled}
            };

            try
            {
                _ministryPlatformService.UpdateRecord(roomReservationPageId, reservationDictionary, token);
            }
            catch (Exception e)
            {
                var msg = string.Format("Error updating Room Reservation, roomReservation: {0}", roomReservation);
                _logger.Error(msg, e);
                throw (new ApplicationException(msg, e));
            }
        }

        public List<Room> GetRoomsByLocationId(int locationId)
        {
            var t = ApiLogin();
            var search = string.Format(",,,,{0}", locationId);
            var records = _ministryPlatformService.GetPageViewRecords("RoomsByLocationId", t, search);

            return records.Select(record => new Room
            {
                BuildingId = record.ToInt("Building_ID"),
                LocationId = record.ToInt("Location_ID"),
                RoomId = record.ToInt("Room_ID"),
                RoomName = record.ToString("Room_Name"),
                RoomNumber = record.ToString("Room_Number"),
                BanquetCapacity = record.ToInt("Banquet_Capacity"),
                Description = record.ToString("Description"),
                TheaterCapacity = record.ToInt("Theater_Capacity")
            }).ToList();
        }

        public List<RoomLayout> GetRoomLayouts()
        {
            var t = ApiLogin();
            var records = _ministryPlatformService.GetPageViewRecords("RoomLayoutsById", t);

            return records.Select(record => new RoomLayout
            {
                LayoutId = record.ToInt("Room_Layout_ID"),
                LayoutName = record.ToString("Layout_Name")
            }).ToList();
        }
    }
}