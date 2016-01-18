using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IEquipmentService
    {
        int CreateEquipmentReservation(EquipmentReservationDto equipmentReservation);
        List<Equipment> GetEquipmentByLocationId(int locationId);
        List<EquipmentService.EquipmentReservation> GetEquipmentReservations(int eventId, int roomId);
    }
}