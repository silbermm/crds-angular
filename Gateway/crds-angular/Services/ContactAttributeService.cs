using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Services
{
    public class ContactAttributeService : IContactAttributeService
    {
        private readonly MPInterfaces.IContactAttributeService _mpContactAttributeService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly MPInterfaces.IAuthenticationService _mpAuthenticationService;

        public ContactAttributeService(
            MPInterfaces.IContactAttributeService mpContactAttributeService, 
            IConfigurationWrapper configurationWrapper, 
            MPInterfaces.IAuthenticationService mpAuthenticationService)
        {
            _mpContactAttributeService = mpContactAttributeService;
            _configurationWrapper = configurationWrapper;
            _mpAuthenticationService = mpAuthenticationService;
        }

        // TODO: Switch out ContactAttributes with ContactAttributesDTO
        public List<ContactAttribute> GetContactAttributes(int contactId)
        {
            var contactAttributes = _mpContactAttributeService.GetCurrentContactAttributes(contactId);            
            return contactAttributes;
        }

        // TODO: Switch out ContactAttributes with ContactAttributesDTO
        public void SaveContactAttributes(int contactId, List<ContactAttribute> contactAttributes)
        {            
            var attributesToSave = contactAttributes.ToList();            

            // Get current list of attributes
            var attributesPersisted = _mpContactAttributeService.GetCurrentContactAttributes(contactId);

            // Remove all matches from list, since there is nothing to do with them
            RemoveMatchesFromBothLists(attributesToSave, attributesPersisted);

            var apiUserToken = GetApiUserToken();
            foreach (var attribute in attributesToSave)
            {
                // These are new so add them
                _mpContactAttributeService.CreateAttribute(apiUserToken, contactId, attribute);
            }

            foreach (var attribute in attributesPersisted)
            {
                // These are old so end-date them to remove them
                attribute.EndDate = DateTime.Today;
                _mpContactAttributeService.UpdateAttribute(apiUserToken, attribute);
            }
        }

        private string GetApiUserToken()
        {
            var apiUser = this._configurationWrapper.GetEnvironmentVarAsString("API_USER");
            var apiPassword = this._configurationWrapper.GetEnvironmentVarAsString("API_PASSWORD");
            var authData = _mpAuthenticationService.Authenticate(apiUser, apiPassword);
            var token = authData["token"].ToString();
            return token;
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
    }
}