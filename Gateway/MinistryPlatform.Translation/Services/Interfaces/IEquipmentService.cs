using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IEquipmentService
    {
        List<Equipment> GetEquipmentByLocationId(int locationId);
    }
}