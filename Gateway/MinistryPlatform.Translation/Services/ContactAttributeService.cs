using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class ContactAttributeService : BaseService, IContactAttributeService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly int _contactAttributesSubPage = Convert.ToInt32((AppSettings("ContactAttributesSubPage")));
        private readonly int _myContactAttributesSubPage = Convert.ToInt32((AppSettings("MyContactAttributesSubPage")));
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
                AttributeId = record.ToInt("Attribute_ID"),
                StartDate = record.ToDate("Start_Date"),
                EndDate = record.ToNullableDate("End_Date"),
                Notes = record.ToString("Notes"),
                AttributeTypeId = record.ToInt("Attribute_Type_ID"),
                AttributeTypeName = record.ToString("Attribute_Type")
            }).ToList();
            return contactAttributes;
        }

        public int CreateAttribute(string token, int contactId, ContactAttribute attribute, bool useMyProfile)
        {
            var attributeDictionary = TranslateContactAttributeToDictionary(attribute);

            var subPageId = useMyProfile ? _myContactAttributesSubPage : _contactAttributesSubPage;            

            try
            {
                return _ministryPlatformService.CreateSubRecord(subPageId, contactId, attributeDictionary, token);
            }
            catch (Exception e)
            {
                var msg = string.Format("Error creating contact attribute, contactId: {0} attributeId: {1}",
                                        contactId,
                                        attribute.AttributeId);
                _logger.Error(msg, e);
                throw (new ApplicationException(msg, e));
            }
        }

        public void UpdateAttribute(string token, ContactAttribute attribute, bool useMyProfile)
        {
            var attributeDictionary = TranslateContactAttributeToDictionary(attribute);
            var subPageId = useMyProfile ? _myContactAttributesSubPage : _contactAttributesSubPage;            

            try
            {
                _ministryPlatformService.UpdateSubRecord(subPageId, attributeDictionary, token);
            }
            catch (Exception e)
            {
                var msg = string.Format("Error updating contact attribute, contactAttributeId: {0} attributeId: {1}",                                        
                                        attribute.ContactAttributeId, attribute.AttributeId);
                _logger.Error(msg, e);
                throw (new ApplicationException(msg, e));
            }
        }

        private static Dictionary<string, object> TranslateContactAttributeToDictionary(ContactAttribute attribute)
        {
            var attributeDictionary = new Dictionary<string, object>
            {
                {"Attribute_Type_ID", attribute.AttributeTypeId},
                {"Attribute_ID", attribute.AttributeId},
                {"Contact_Attribute_ID", attribute.ContactAttributeId},
                {"Start_Date", attribute.StartDate},
                {"End_Date", attribute.EndDate},
                {"Notes", attribute.Notes}
            };
            return attributeDictionary;
        }
    }
}