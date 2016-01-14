using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IRoomService
    {
        int CreateRoomReservation(RoomReservationDto roomReservation);
        List<Room> GetRoomsByLocationId(int locationId);
        List<RoomLayout> GetRoomLayouts();
    }
}