using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class RoomService : BaseService, IRoomService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;

        public RoomService(IMinistryPlatformService ministryPlatformService, IAuthenticationService authenticationService, IConfigurationWrapper configuration)
            : base(authenticationService, configuration)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public List<Room> GetRoomsByLocationId(int locationId)
        {
            var t = ApiLogin();
            var search = string.Format(",,,,{0}", locationId);
            var records = _ministryPlatformService.GetPageViewRecords("RoomsByLocationId", t, search);

            return records.Select(record => new Room
            {
                BuildingId = record.ToInt("Building_ID"),
                LocationId = record.ToInt("Location_ID"),
                RoomId = record.ToInt("Room_ID"),
                RoomName = record.ToString("Room_Name"),
                RoomNumber = record.ToString("Room_Number"),
                BanquetCapacity = record.ToInt("Banquet_Capacity"),
                Description = record.ToString("Description"),
                TheaterCapacity = record.ToInt("Theater_Capacity")
            }).ToList();
        }

        public List<RoomLayout> GetRoomLayouts()
        {
            var t = ApiLogin();
            var records = _ministryPlatformService.GetPageViewRecords("RoomLayoutsById", t);

            return records.Select(record => new RoomLayout
            {
                LayoutId = record.ToInt("Room_Layout_ID"),
                LayoutName = record.ToString("Layout_Name")
            }).ToList();
        }



        //public List<Room> GetRoomsByCongregationId(int congregationId)
        //{
        //    var token = ApiLogin();
        //    var search = string.Format(",,,,{0}", congregationId);
        //    var records = _ministryPlatformService.GetPageViewRecords("RoomsByCongregationId", token, search);

        //    return records.Select(record => new Room
        //    {
        //        BuildingId = record.ToInt("Building_ID"),
        //        LocationId = record.ToInt("Location_ID"),
        //        RoomId = record.ToInt("Room_ID"),
        //        RoomName = record.ToString("Room_Name"),
        //        RoomNumber = record.ToString("Room_Number")
        //    }).ToList();
        //}
    }
}