using System;
using System.Linq;
using crds_angular.Models.Crossroads.Profile;
using crds_angular.Services;
using Crossroads.Utilities.Services;
using MinistryPlatform.Translation.PlatformService;
using MvcContrib.TestHelper.Ui;
using NUnit.Framework;
using MPServices = MinistryPlatform.Translation.Services;

namespace crds_angular.test.Services
{
    [Category("IntegrationTests")]
    class ContactAttributeServiceIntegrationTest
    {
        private ContactAttributeService _service;
        private AttributeService _attributeService;

        [SetUp]
        public void SetUp()
        {
            var configWrapper = new ConfigurationWrapper();
            var platformService = new PlatformServiceClient();
            var ministryPlatformService = new MPServices.MinistryPlatformServiceImpl(platformService, configWrapper);
            var authenticationService = new MPServices.AuthenticationServiceImpl(platformService, ministryPlatformService);
            var apiUserService = new MPServices.ApiUserService(configWrapper, authenticationService);
            

            var mpAttributeService = new MPServices.AttributeService(ministryPlatformService, authenticationService, configWrapper);
            var mpService = new MPServices.ContactAttributeService(authenticationService, configWrapper, ministryPlatformService);
            _attributeService = new AttributeService(mpAttributeService);
            _service = new ContactAttributeService(mpService, _attributeService, apiUserService, mpAttributeService);
        }

        [Test]    
        public void LoadContactAttributes()
        {
            var contactId = 2399608;
            var attributes = _service.GetContactAttributes(contactId);

            Assert.That(attributes.MultiSelect.Count > 0);
            Assert.That(attributes.SingleSelect.Count > 0);
        }

        [Test]        
        public void SaveContactAttributes()
        {
            var contactId = 2399608;
            var allAttributes = _service.GetContactAttributes(contactId);
            var attributes = allAttributes.MultiSelect;
            var singleAttributes = allAttributes.SingleSelect;

            var firstAttributeType = attributes.Values.First(x => x.Attributes.Exists(attribute => attribute.Selected));

            var attributeToRemove = firstAttributeType.Attributes.First(x => x.Selected);
            attributeToRemove.Selected = false;
            _service.SaveContactAttributes(contactId, attributes, singleAttributes);

            var attributeToAdd = new ContactAttributeDTO()
            {
                AttributeId = attributeToRemove.AttributeId,
                StartDate = DateTime.Today,
                EndDate = null,
                Notes = string.Empty
            };

            // Unset the values that were set from last save
            attributeToRemove.Selected = true;
            attributeToRemove.EndDate = null; 

            firstAttributeType.Attributes.Add(attributeToAdd);
            _service.SaveContactAttributes(contactId, attributes, singleAttributes);
        }
        
        [Test]
        public void RemoveAndThenAddAllContactAttributes()
        {
            var contactId = 2399608;
            var originalAttributes = _service.GetContactAttributes(contactId);
            var deletedAttributes = _service.GetContactAttributes(contactId);

            deletedAttributes.MultiSelect.Values.ForEach(attributeType => attributeType.Attributes.ForEach(attribute => attribute.Selected = false));

            // Remove all items            
            _service.SaveContactAttributes(contactId, deletedAttributes.MultiSelect, deletedAttributes.SingleSelect);

            // Add all back
            _service.SaveContactAttributes(contactId, originalAttributes.MultiSelect, originalAttributes.SingleSelect);
        }

        [Test]
        public void RemoveAndThenAddSingleContactAttribute()
        {
            var contactId = 2399608;
            var originalAttributes = _service.GetContactAttributes(contactId);
            var deletedAttributes = _service.GetContactAttributes(contactId);            

            deletedAttributes.SingleSelect.Values.ForEach(attributeType => attributeType.Value = null);

            // Remove all items            
            _service.SaveContactAttributes(contactId, deletedAttributes.MultiSelect, deletedAttributes.SingleSelect);

            // Add all back
            _service.SaveContactAttributes(contactId, originalAttributes.MultiSelect, originalAttributes.SingleSelect);
        }

        [Test]
        public void ChangeSingleContactAttribute()
        {
            var contactId = 2399608;                        
            var attributes = _service.GetContactAttributes(contactId);
            
            var item = attributes.SingleSelect.First(x => x.Value.Value != null);
            var attributeTypes = _attributeService.GetAttributeTypes(item.Key);
            var anotherValue = attributeTypes[0].Attributes.First(x => x.AttributeId != item.Value.Value.AttributeId);
            var originalValue = item.Value.Value;
            
            // Modify the items            
            item.Value.Value = anotherValue;
            _service.SaveContactAttributes(contactId, attributes.MultiSelect, attributes.SingleSelect);
            
            // Add all back
            item.Value.Value = originalValue;
            _service.SaveContactAttributes(contactId, attributes.MultiSelect, attributes.SingleSelect);
        }

        [Test]
        public void InitialSelectionOfSingleContactAttribute()
        {
            var contactId = 2399608;
            var attributes = _service.GetContactAttributes(contactId);

            var item = attributes.SingleSelect.First(x => x.Value.Value == null);
            var attributeTypes = _attributeService.GetAttributeTypes(item.Key);
            var anotherValue = attributeTypes[0].Attributes.First();
            var originalValue = item.Value.Value;

            // Modify the items            
            item.Value.Value = anotherValue;
            _service.SaveContactAttributes(contactId, attributes.MultiSelect, attributes.SingleSelect);

            // Add all back
            item.Value.Value = originalValue;
            _service.SaveContactAttributes(contactId, attributes.MultiSelect, attributes.SingleSelect);

            //// Remove all items            
            //_service.SaveContactAttributes(contactId, deletedAttributes.MultiSelect, deletedAttributes.SingleSelect);

            //// Add all back
            //_service.SaveContactAttributes(contactId, originalAttributes.MultiSelect, deletedAttributes.SingleSelect);
        }
    }
}
