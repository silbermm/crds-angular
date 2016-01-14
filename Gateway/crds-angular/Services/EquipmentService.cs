using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Events;
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
            var records = _mpEquipmentService.GetEquipmentByLocationId(locationId);

            return records.Select(record => new RoomEquipment
            {
                Id = record.EquipmentId,
                Name = record.EquipmentName,
                Quantity = record.QuantityOnHand
            }).ToList();
        }
    }
}