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
        [Test]    
        public void LoadContactAttributes()
        {
            var configWrapper = new ConfigurationWrapper();
            var platformService = new PlatformServiceClient();
            var ministryPlatformService = new MPServices.MinistryPlatformServiceImpl(platformService, configWrapper);
            var authenticationService = new MPServices.AuthenticationServiceImpl(platformService, ministryPlatformService);
            var apiUserService = new MPServices.ApiUserService(configWrapper, authenticationService);

            var mpService = new MPServices.ContactAttributeService(authenticationService, configWrapper, ministryPlatformService);
            var service = new ContactAttributeService(mpService, apiUserService);

            var contactId = 2399608;
            var attributes = service.GetContactAttributes(contactId);

            Assert.That(attributes.Count > 0);
        }

        [Test]        
        public void SaveContactAttributes()
        {
            var configWrapper = new ConfigurationWrapper();
            var platformService = new PlatformServiceClient();
            var ministryPlatformService = new MPServices.MinistryPlatformServiceImpl(platformService, configWrapper);
            var authenticationService = new MPServices.AuthenticationServiceImpl(platformService, ministryPlatformService);
            var apiUserService = new MPServices.ApiUserService(configWrapper, authenticationService);
            
            var mpService = new MPServices.ContactAttributeService(authenticationService, configWrapper, ministryPlatformService);
            var service = new ContactAttributeService(mpService, apiUserService);
            
            var contactId = 2399608;
            var attributes = service.GetContactAttributes(contactId);

            var firstAttributeType = attributes[0];

            var attributeToRemove = firstAttributeType.Attributes[0];
            firstAttributeType.Attributes.Remove(attributeToRemove);
            service.SaveContactAttributes(contactId, attributes);

            var attributeToAdd = new ContactAttributeDTO()
            {
                AttributeId = attributeToRemove.AttributeId,
                StartDate = DateTime.Today,
                EndDate = null,
                Notes = string.Empty
            };

            firstAttributeType.Attributes.Add(attributeToAdd);
            service.SaveContactAttributes(contactId, attributes);
        }
        
        [Test]
        public void RemoveAndThenAddAllContactAttributes()
        {
            var configWrapper = new ConfigurationWrapper();
            var platformService = new PlatformServiceClient();
            var ministryPlatformService = new MPServices.MinistryPlatformServiceImpl(platformService, configWrapper);
            var authenticationService = new MPServices.AuthenticationServiceImpl(platformService, ministryPlatformService);
            var apiUserService = new MPServices.ApiUserService(configWrapper, authenticationService);

            var mpService = new MPServices.ContactAttributeService(authenticationService, configWrapper, ministryPlatformService);
            var service = new ContactAttributeService(mpService, apiUserService);

            var contactId = 2399608;
            var attributes = service.GetContactAttributes(contactId);

            // Remove all items            
            service.SaveContactAttributes(contactId, new Dictionary<int, ContactAttributeTypeDTO>());

            // Add all back
            service.SaveContactAttributes(contactId, attributes);
        }
    }
}
