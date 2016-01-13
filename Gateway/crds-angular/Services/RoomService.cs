using System.Collections.Generic;
using System.Linq;
using crds_angular.Services.Interfaces;

namespace crds_angular.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly MinistryPlatform.Translation.Services.Interfaces.IEquipmentService _mpEquipmentService;

        public EquipmentService(MinistryPlatform.Translation.Services.Interfaces.IEquipmentService equipmentService)
        {
            _mpEquipmentService = equipmentService;
        }

        public List<RoomEquipment> GetEquipmentByLocationId(int locationId)
        {
            //return new List<RoomEquipment>();

            var records = _mpEquipmentService.GetEquipmentByLocationId(locationId);

            return records.Select(record => new RoomEquipment
            {
                Id = record.EquipmentId,
                Name = record.EquipmentName
            }).ToList();
        }
    }

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

    public class RoomEquipment
    {
        public int Id { get; set; }
        public string Name { get; set; }   
    }
}