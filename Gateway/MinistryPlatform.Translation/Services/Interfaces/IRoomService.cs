using System.Collections.Generic;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.EventReservations;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IRoomService
    {
        int CreateRoomReservation(RoomReservationDto roomReservation, string token);
        List<Room> GetRoomsByLocationId(int locationId);
        List<RoomLayout> GetRoomLayouts();
        List<RoomReservationDto> GetRoomReservations(int eventId);
        void UpdateRoomReservation(RoomReservationDto roomReservation, string token);
    }
}