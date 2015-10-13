using System;
using System.Collections.Generic;
using crds_angular.Services;
using Crossroads.Utilities.Services;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.PlatformService;
using MPServices = MinistryPlatform.Translation.Services;

using NUnit.Framework;

namespace crds_angular.test.Services
{
    class ContactAttributeServiceIntegrationTest
    {

        // TODO: These are integration tests. Removed before merging
        [Test]
        [Ignore]
        public void SaveContactAttributes()
        {
            var configWrapper = new ConfigurationWrapper();
            var platformService = new PlatformServiceClient();
            var ministryPlatformService = new MPServices.MinistryPlatformServiceImpl(platformService, configWrapper);
            var authenticationService = new MPServices.AuthenticationServiceImpl(platformService, ministryPlatformService);

            
            var mpService = new MPServices.ContactAttributeService(authenticationService, configWrapper, ministryPlatformService);
            var service = new ContactAttributeService(mpService, configWrapper, authenticationService);

            
            var contactId = 2399608;
            var attributes = service.GetContactAttributes(contactId);


            var attributeToRemove = attributes[0];
            attributes.Remove(attributeToRemove);
            service.SaveContactAttributes(contactId, attributes);


            var attributeToAdd = new ContactAttribute()
            {
                AttributeId = attributeToRemove.AttributeId,
                AttributeTypeId = attributeToRemove.AttributeTypeId,
                StartDate = DateTime.Today,
                EndDate = null,
                Notes = string.Empty
            };

            attributes.Add(attributeToAdd);
            service.SaveContactAttributes(contactId, attributes);
        }


        // TODO: These are integration tests. Removed before merging
        [Test]
        [Ignore]
        public void RemoveAndThenAddAllContactAttributes()
        {
            var configWrapper = new ConfigurationWrapper();
            var platformService = new PlatformServiceClient();
            var ministryPlatformService = new MPServices.MinistryPlatformServiceImpl(platformService, configWrapper);
            var authenticationService = new MPServices.AuthenticationServiceImpl(platformService, ministryPlatformService);

            var mpService = new MPServices.ContactAttributeService(authenticationService, configWrapper, ministryPlatformService);
            var service = new ContactAttributeService(mpService, configWrapper, authenticationService);

            var contactId = 2399608;
            var attributes = service.GetContactAttributes(contactId);

            // Remove all items            
            service.SaveContactAttributes(contactId, new List<ContactAttribute>());

            // Add all back
            service.SaveContactAttributes(contactId, attributes);
        }

    }
}
