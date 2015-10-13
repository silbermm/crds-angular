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
                AttributeTypeId = record.ToInt("Attribute_Type_ID")
            }).ToList();
            return contactAttributes;
        }

        public void SaveContactAttributes(int contactId, List<ContactAttribute> contactAttributes)
        {
            var token = ApiLogin();       
            var attributesToSave = contactAttributes.ToList();

            // Get current list of attributes
            var attributesPersisted = GetCurrentContactAttributes(contactId);

            // Remove all matches from list, since there is nothing to do with them
            RemoveMatchesFromBothLists(attributesToSave, attributesPersisted);

            foreach (var attribute in attributesToSave)
            {
                // These are new so add them
                SaveAttribute(token, contactId, attribute);
            }

            foreach (var attribute in attributesPersisted)
            {
                // These are old so end-date them to remove them
                attribute.EndDate = DateTime.Today;
                UpdateAttribute(token, attribute);
            }
        }

        private void RemoveMatchesFromBothLists(List<ContactAttribute> attributesToSave, List<ContactAttribute> attributesPersisted)
        {
            for (int index = attributesToSave.Count - 1; index >= 0; index--)
            {
                var attribute = attributesToSave[index];

                for (int currentIndex = attributesPersisted.Count - 1; currentIndex >= 0; currentIndex--)
                {
                    var currentAttribute = attributesPersisted[currentIndex];

                    if (currentAttribute.ContactAttributeId == attribute.ContactAttributeId)
                    {
                        // match by Id
                        // TODO: Do we need to look at other fields here like attribute.AttributeId & attribute.AttributeTypeId
                        // Or would a Contains be more correct and remove the looping?

                        attributesPersisted.RemoveAt(currentIndex);
                        attributesToSave.RemoveAt(index);
                        break;
                    }
                }
            }
        }

        private void SaveAttribute(string token, int contactId, ContactAttribute attribute)
        {
            var attributeDictionary = TranslateContactAttributeToDictionary(attribute);

            try
            {                
                _ministryPlatformService.CreateSubRecord(_contactAttributesSubPage, contactId, attributeDictionary, token);
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

        private void UpdateAttribute(string token, ContactAttribute attribute)
        {
            var attributeDictionary = TranslateContactAttributeToDictionary(attribute);

            try
            {
                _ministryPlatformService.UpdateSubRecord(_contactAttributesSubPage, attributeDictionary, token);
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