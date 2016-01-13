using System.Collections.Generic;
using System.Linq;
using crds_angular.Services.Interfaces;

namespace crds_angular.Services
{
    public class RoomService : IRoomService
    {
        private readonly MinistryPlatform.Translation.Services.Interfaces.IRoomService _roomService;

        public RoomService(MinistryPlatform.Translation.Services.Interfaces.IRoomService roomService)
        {
            _roomService = roomService;
        }

        public List<Room> GetRoomsByLocationId(int id)
        {
            var records = _roomService.GetRoomsByLocationId(id);

            return records.Select(record => new Room
            {
                BuildingId = record.BuildingId,
                Id = record.RoomId,
                LocationId = record.LocationId,
                Name = record.RoomName
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

    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BuildingId { get; set; }
        public int LocationId { get; set; }
    }

    public class RoomLayout
    {
        public int Id { get; set; }
        public string LayoutName { get; set; }
    }
}