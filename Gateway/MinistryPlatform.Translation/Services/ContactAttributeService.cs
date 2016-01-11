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
        private readonly IApiUserService _apiUserService;
        private readonly int _contactAttributesSubPage = Convert.ToInt32(AppSettings("ContactAttributesSubPage"));
        private readonly int _constactSelectedContactAttributes = Convert.ToInt32(AppSettings("SelectedContactAttributes"));
        private readonly int _myContactAttributesSubPage = Convert.ToInt32(AppSettings("MyContactAttributesSubPage"));
        private readonly int _myContactCurrentAttributesSubPageView = Convert.ToInt32(AppSettings("MyContactCurrentAttributesSubPageView"));
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ContactAttributeService(IAuthenticationService authenticationService, 
            IConfigurationWrapper configurationWrapper, 
            IMinistryPlatformService ministryPlatformService, 
            IApiUserService apiUserService)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
            _apiUserService = apiUserService;
        }

        public List<ContactAttribute> GetCurrentContactAttributes(string token, int contactId, bool useMyProfile, int? attributeTypeIdFilter = null)
        {
            var subPageViewId = useMyProfile ? _myContactCurrentAttributesSubPageView : _constactSelectedContactAttributes;
            var searchString = attributeTypeIdFilter.HasValue ? string.Format(",,,,\"{0}\"", attributeTypeIdFilter.Value) : "";
            var records = _ministryPlatformService.GetSubpageViewRecords(subPageViewId, contactId, token, searchString);

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

        private Dictionary<string, object> TranslateContactAttributeToDictionary(ContactAttribute attribute)
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