using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IRoomService
    {
        List<Room> GetRoomsByLocationId(int locationId);
    }
}