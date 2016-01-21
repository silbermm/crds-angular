using System.Collections.Generic;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.EventReservations;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IEquipmentService
    {
        int CreateEquipmentReservation(EquipmentReservationDto equipmentReservation, string token);
        List<Equipment> GetEquipmentByLocationId(int locationId);
        List<EquipmentReservationDto> GetEquipmentReservations(int eventId, int roomId);
        void UpdateEquipmentReservation(EquipmentReservationDto equipmentReservation);
    }
}