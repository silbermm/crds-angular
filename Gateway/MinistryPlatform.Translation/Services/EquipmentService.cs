using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class EquipmentService : BaseService, IEquipmentService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;

        public EquipmentService(IMinistryPlatformService ministryPlatformService, IAuthenticationService authenticationService, IConfigurationWrapper configuration)
            : base(authenticationService, configuration)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public List<Equipment> GetEquipmentByLocationId(int locationId)
        {
            var t = ApiLogin();
            var search = string.Format(",,,,{0}", locationId);
            var records = _ministryPlatformService.GetPageViewRecords("EquipmentByLocationId", t, search);

            return records.Select(record => new Equipment
            {
                EquipmentId = record.ToInt("Equipment_ID"),
                EquipmentName = record.ToString("Equipment_Name"),
                QuantityOnHand = record.ToInt("Quantity_On_Hand")
            }).ToList();
        }


    }
}