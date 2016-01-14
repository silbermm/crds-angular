using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Events;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Services;

namespace crds_angular.Services
{
    public class RoomService : IRoomService
    {
        private readonly MinistryPlatform.Translation.Services.Interfaces.IRoomService _roomService;

        public RoomService(MinistryPlatform.Translation.Services.Interfaces.IRoomService roomService)
        {
            _roomService = roomService;
        }

        //public void CreateRoom()
        //{
            
        //}

        public List<Room> GetRoomsByLocationId(int id)
        {
            //var tmpResr = new RoomReservationDto();
            //tmpResr.Approved = false;
            //tmpResr.Cancelled = false;
            //tmpResr.EventId = 3184060;
            //tmpResr.Hidden = true;
            //tmpResr.Notes = "testing api";
            //tmpResr.RoomId = 208;
            //tmpResr.RoomLayoutId = 2;
            //_roomService.CreateRoomReservation(tmpResr);

            var records = _roomService.GetRoomsByLocationId(id);

            return records.Select(record => new Room
            {
                BuildingId = record.BuildingId,
                Id = record.RoomId,
                LocationId = record.LocationId,
                Name = record.RoomName,
                BanquetCapacity = record.BanquetCapacity,
                Description = record.Description,
                TheaterCapacity = record.TheaterCapacity
            }).ToList();
        }

        public List<RoomLayout> GetRoomLayouts()
        {
            var records = _roomService.GetRoomLayouts();

            return records.Select(record => new RoomLayout
            {
                Id = record.LayoutId,
                LayoutName = record.LayoutName
            }).ToList();
        }
    }
}