using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IAttributeService
    {
        List<AttributeType> GetAttributeTypes(int? attributeTypeId);
    }
}