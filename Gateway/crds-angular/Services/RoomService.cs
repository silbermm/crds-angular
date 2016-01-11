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

        public List<Room> GetRoomsByLocationId(int id, string token)
        {
            var records = _roomService.GetRoomsByLocationId(id, token);

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
        //public string Number { get; set; }
        //public Building Building { get; set; }
        public int BuildingId { get; set; }
        //public string BuildingName { get; set; }
        public int LocationId { get; set; }
        //public string LocationName { get; set; }
    }

    //public class Building
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public int LocationId { get; set; }
    //}
}