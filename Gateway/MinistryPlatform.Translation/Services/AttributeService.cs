using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;
using Attribute = MinistryPlatform.Models.Attribute;

namespace MinistryPlatform.Translation.Services
{
    public class AttributeService : BaseService, IAttributeService
    {
        public AttributeService(IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
        {
            
        }

        public List<AttributeType> GetAttributeTypes(int? attributeTypeId)
        {
            var token = base.ApiLogin();

            var filter = attributeTypeId.HasValue ? string.Format(",,\"{0}\"", attributeTypeId) : string.Empty;

            var pageViewId = _configurationWrapper.GetConfigIntValue("AttributesPageView");
            var records = MinistryPlatformService.GetPageViewRecords(pageViewId, token, filter);

            var attributeTypes = new Dictionary<int, AttributeType>();

            foreach (var record in records)
            {
                var attribute = new Attribute
                {
                    AttributeId = record.ToInt("Attribute_ID"),
                    Name = record.ToString("Attribute_Name"),
                    CategoryId = record.ToInt("Attribute_Category_ID"),
                    Category = record.ToString("Attribute_Category")                  
                };

                var attributeType = new AttributeType()
                {
                    Name = record.ToString("Attribute_Type"),
                    AttributeTypeId = record.ToInt("Attribute_Type_ID")
                };

                var key = attributeType.AttributeTypeId;

                if (!attributeTypes.ContainsKey(key))
                {
                    attributeTypes[key] = attributeType;
                }

                attributeTypes[key].Attributes.Add(attribute);
            }

            return attributeTypes.Values.ToList();
        }
    }
}