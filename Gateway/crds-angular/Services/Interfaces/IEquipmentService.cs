using System.Collections.Generic;
using crds_angular.Models.Crossroads.Events;

namespace crds_angular.Services.Interfaces
{
    public interface IEquipmentService
    {
        List<RoomEquipment> GetEquipmentByLocationId(int locationId);
    }
}