using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class ContactAttributeService : BaseService, IContactAttributeService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;

        public ContactAttributeService(IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper, IMinistryPlatformService ministryPlatformService)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public List<ContactAttribute> GetCurrentContactAttributes(int contactId)
        {
            var token = ApiLogin();
            var records = _ministryPlatformService.GetSubpageViewRecords("SelectedContactAttributes", contactId, token);

            var contactAttributes = records.Select(record => new ContactAttribute
            {
                ContactAttributeId = record.ToInt("Contact_Attribute_ID"),                
                StartDate = record.ToDate("Start_Date"),
                EndDate = record.ToNullableDate("End_Date"),
                Notes = record.ToString("Notes"),
                AttributeId = record.ToInt("Attribute_ID"),
                AttributeTypeId = record.ToInt("Attribute_Type_ID")
            }).ToList();
            return contactAttributes;
        }
    }
}