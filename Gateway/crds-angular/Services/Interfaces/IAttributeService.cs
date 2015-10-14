using System.Collections.Generic;
using crds_angular.Models.Crossroads.Attribute;

namespace crds_angular.Services.Interfaces
{
    public interface IAttributeService
    {
        List<AttributeTypeDTO> GetAttributeTypes(int? attributeTypeId);
    }
}