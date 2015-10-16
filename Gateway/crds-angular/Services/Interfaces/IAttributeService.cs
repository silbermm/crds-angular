using System.Collections.Generic;
using crds_angular.Models.Crossroads.Attribute;
using MinistryPlatform.Models;

namespace crds_angular.Services.Interfaces
{
    public interface IAttributeService
    {
        List<AttributeTypeDTO> GetAttributeTypes(int? attributeTypeId);
        AttributeDTO ConvertAttributeToAttributeDto(Attribute attribute);
    }
}