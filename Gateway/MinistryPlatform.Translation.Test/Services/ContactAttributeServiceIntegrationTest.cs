using System;
using System.Collections.Generic;
using Crossroads.Utilities.Services;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Services;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
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
            var ministryPlatformService = new MinistryPlatformServiceImpl(platformService, configWrapper);
            var authenticationService = new AuthenticationServiceImpl(platformService, ministryPlatformService);

            
            var service = new ContactAttributeService(authenticationService, configWrapper, ministryPlatformService);
            
            var contactId = 2399608;
            var attributes = service.GetCurrentContactAttributes(contactId);


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
            var ministryPlatformService = new MinistryPlatformServiceImpl(platformService, configWrapper);
            var authenticationService = new AuthenticationServiceImpl(platformService, ministryPlatformService);

            var service = new ContactAttributeService(authenticationService, configWrapper, ministryPlatformService);

            var contactId = 2399608;
            var attributes = service.GetCurrentContactAttributes(contactId);

            // Remove all items            
            service.SaveContactAttributes(contactId, new List<ContactAttribute>());

            // Add all back
            service.SaveContactAttributes(contactId, attributes);
        }

    }
}
