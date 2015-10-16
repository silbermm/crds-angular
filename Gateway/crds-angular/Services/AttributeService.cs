using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Attribute;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Services
{
    public class AttributeService : IAttributeService
    {
        private readonly MPInterfaces.IAttributeService _attributeService;

        public AttributeService(MPInterfaces.IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        public List<AttributeTypeDTO> GetAttributeTypes(int? attributeTypeId)
        {
            var attributes = _attributeService.GetAttributes(attributeTypeId);

            var attributeTypes = new Dictionary<int, AttributeTypeDTO>();

            foreach (var attribute in attributes)
            {
                var attributeDto = ConvertAttributeToAttributeDto(attribute);

                var key = GetOrCreateAttributeTypeDto(attribute, attributeTypes);

                attributeTypes[key].Attributes.Add(attributeDto);
            }

            return attributeTypes.Values.ToList();
        }

        public AttributeDTO ConvertAttributeToAttributeDto(Attribute attribute)
        {
            var attributeDto = new AttributeDTO
            {
                AttributeId = attribute.AttributeId,
                Name = attribute.Name,
                CategoryId = attribute.CategoryId,
                Category = attribute.Category
            };
            return attributeDto;
        }

        private static int GetOrCreateAttributeTypeDto(Attribute attribute, Dictionary<int, AttributeTypeDTO> attributeTypes)
        {
            var attributeTypeDto = new AttributeTypeDTO()
            {
                AttributeTypeId = attribute.AttributeTypeId,
                Name = attribute.AttributeTypeName,
                AllowMultipleSelections = !attribute.PreventMultipleSelection
            };

            var key = attributeTypeDto.AttributeTypeId;

            if (!attributeTypes.ContainsKey(key))
            {
                attributeTypes[key] = attributeTypeDto;
            }
            return key;
        }
    }
}