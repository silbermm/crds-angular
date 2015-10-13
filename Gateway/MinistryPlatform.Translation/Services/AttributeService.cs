using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;
using Attribute = MinistryPlatform.Models.Attribute;

namespace MinistryPlatform.Translation.Services
{
    public class AttributeService : BaseService, IAttributeService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;
            
        public AttributeService(IMinistryPlatformService ministryPlatformService, IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public List<Attribute> GetAttributes(int? attributeTypeId)
        {
            var token = base.ApiLogin();

            var filter = attributeTypeId.HasValue ? string.Format(",,\"{0}\"", attributeTypeId) : string.Empty;
            var records = _ministryPlatformService.GetPageViewRecords("AttributesPageView", token, filter);

            return records.Select(record => new Attribute
            {
                AttributeId = record.ToInt("Attribute_ID"), 
                Name = record.ToString("Attribute_Name"), 
                CategoryId = record.ToNullableInt("Attribute_Category_ID"), 
                Category = record.ToString("Attribute_Category"), 
                AttributeTypeId = record.ToInt("Attribute_Type_ID"), 
                AttributeTypeName = record.ToString("Attribute_Type")
            }).ToList();
        }
    }
}