using System.Collections.Generic;

namespace crds_angular.Services.Interfaces
{
    public interface IRoomService
    {
        List<Room> GetRoomsByLocationId(int id, string token);
    }
}