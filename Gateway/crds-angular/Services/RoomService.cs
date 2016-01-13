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
    }

    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BuildingId { get; set; }
        public int LocationId { get; set; }
    }
}