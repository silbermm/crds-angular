using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Events;
using crds_angular.Services.Interfaces;

namespace crds_angular.Services
{
    public class CongregationService : ICongregationService
    {
        private readonly MinistryPlatform.Translation.Services.Interfaces.ICongregationService _congregationService;
        private readonly IRoomService _roomService;
        private readonly IEquipmentService _equipmentService;

        public CongregationService(MinistryPlatform.Translation.Services.Interfaces.ICongregationService congregationService,
                                   IRoomService roomService,
                                   IEquipmentService equipmentService)
        {
            _congregationService = congregationService;
            _roomService = roomService;
            _equipmentService = equipmentService;
        }

        public Congregation GetCongregationById(int id)
        {
            var congregation = _congregationService.GetCongregationById(id);
            if (congregation == null)
            {
                return null;
            }
            var c = new Congregation();
            c.CongregationId = congregation.CongregationId;
            c.LocationId = congregation.LocationId;
            c.Name = congregation.Name;

            return c;
        }

        public List<Room> GetRooms(int congregationId)
        {
            var congregation = _congregationService.GetCongregationById(congregationId);
            if (congregation == null)
            {
                throw new ApplicationException("Congregation Not Found");
            }
            var rooms = _roomService.GetRoomsByLocationId(congregation.LocationId);
            return rooms;
        }

        public List<RoomEquipment> GetEquipment(int congregationId)
        {
            var congregation = _congregationService.GetCongregationById(congregationId);
            if (congregation == null)
            {
                throw new ApplicationException("Congregation Not Found");
            }
            var equipment = _equipmentService.GetEquipmentByLocationId(congregation.LocationId);
            return equipment;
        }
    }
}