using System.Collections.Generic;

namespace crds_angular.Services.Interfaces
{
    public interface IEquipmentService
    {
        List<RoomEquipment> GetEquipmentByLocationId(int locationId);
    }
}