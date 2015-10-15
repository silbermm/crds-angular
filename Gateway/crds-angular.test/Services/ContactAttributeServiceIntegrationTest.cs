using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads.Profile;
using crds_angular.Services;
using Crossroads.Utilities.Services;
using MinistryPlatform.Translation.PlatformService;
using MPServices = MinistryPlatform.Translation.Services;

using NUnit.Framework;

namespace crds_angular.test.Services
{
    [Category("IntegrationTests")]
    class ContactAttributeServiceIntegrationTest
    {
        private ContactAttributeService _service;

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
            _service = new ContactAttributeService(mpService, apiUserService, mpAttributeService);
        }

        [Test]    
        public void LoadContactAttributes()
        {
            var contactId = 2399608;
            var attributes = _service.GetContactAttributes(contactId);

            Assert.That(attributes.Count > 0);
        }

        [Test]        
        public void SaveContactAttributes()
        {
            var contactId = 2399608;
            var attributes = _service.GetContactAttributes(contactId);

            var firstAttributeType = attributes[0];

            var attributeToRemove = firstAttributeType.Attributes[0];
            firstAttributeType.Attributes.Remove(attributeToRemove);
            _service.SaveContactAttributes(contactId, attributes);

            var attributeToAdd = new ContactAttributeDTO()
            {
                AttributeId = attributeToRemove.AttributeId,
                StartDate = DateTime.Today,
                EndDate = null,
                Notes = string.Empty
            };

            firstAttributeType.Attributes.Add(attributeToAdd);
            _service.SaveContactAttributes(contactId, attributes);
        }
        
        [Test]
        public void RemoveAndThenAddAllContactAttributes()
        {
            var contactId = 2399608;
            var attributes = _service.GetContactAttributes(contactId);

            // Remove all items            
            _service.SaveContactAttributes(contactId, new Dictionary<int, ContactAttributeTypeDTO>());

            // Add all back
            _service.SaveContactAttributes(contactId, attributes);
        }
    }
}
