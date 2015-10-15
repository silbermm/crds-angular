using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Profile;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using Attribute = MinistryPlatform.Models.Attribute;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Services
{
    public class ContactAttributeService : IContactAttributeService
    {
        private readonly MPInterfaces.IContactAttributeService _mpContactAttributeService;
        private readonly MPInterfaces.IApiUserService _apiUserService;
        private readonly MPInterfaces.IAttributeService _mpAttributeService;

        public ContactAttributeService(
            MPInterfaces.IContactAttributeService mpContactAttributeService, 
            MPInterfaces.IApiUserService apiUserService,
            MPInterfaces.IAttributeService mpAttributeService)
        {
            _mpContactAttributeService = mpContactAttributeService;
            _apiUserService = apiUserService;
            _mpAttributeService = mpAttributeService;
        }

        public Dictionary<int, ContactAttributeTypeDTO> GetContactAttributes(int contactId)
        {
            var mpAttributes = _mpAttributeService.GetAttributes(null);
            var mpContactAttributes = _mpContactAttributeService.GetCurrentContactAttributes(contactId);

            var resultList = TranslateToAttributeTypeDtos(mpContactAttributes, mpAttributes);
            return resultList;
        }

        private Dictionary<int, ContactAttributeTypeDTO> TranslateToAttributeTypeDtos(
            List<ContactAttribute> mpContactAttributes, List<Attribute> mpAttributes)
        {
            // TODO: See if we can push this down to the MP Layer to get this data from the select directly
            // Possibly also pair this down to multi-select lists, and handle single-select as dropdown / lookups
            var attributeTypesDictionary = mpAttributes
                .Select(x => new {x.AttributeTypeId, x.AttributeTypeName})
                .Distinct()
                .ToDictionary(mpAttributeType => mpAttributeType.AttributeTypeId,
                              mpAttributeType => new ContactAttributeTypeDTO()
                              {
                                  AttributeTypeId = mpAttributeType.AttributeTypeId,
                                  Name = mpAttributeType.AttributeTypeName,
                              });

            foreach (var mpAttribute in mpAttributes)
            {
                var contactAttribute = new ContactAttributeDTO()
                {
                    AttributeId = mpAttribute.AttributeId,
                    Name = mpAttribute.Name,
                    Selected = false
                };

                attributeTypesDictionary[mpAttribute.AttributeTypeId].Attributes.Add(contactAttribute);
            }

            foreach (var mpContactAttribute in mpContactAttributes)
            {
                var contactAttributeType = attributeTypesDictionary[mpContactAttribute.AttributeTypeId];
                var contactAttribute = contactAttributeType.Attributes.First(x => x.AttributeId == mpContactAttribute.AttributeId);
                contactAttribute.StartDate = mpContactAttribute.StartDate;
                contactAttribute.EndDate = mpContactAttribute.EndDate;
                contactAttribute.Notes = mpContactAttribute.Notes;
                contactAttribute.Selected = true;
            }

            return attributeTypesDictionary;
        }
        
        public void SaveContactAttributes(int contactId, Dictionary<int, ContactAttributeTypeDTO> contactAttributes)
        {
            var attributesToSave = TranslateToMPAttributes(contactAttributes);

            // Get current list of attributes
            var attributesPersisted = _mpContactAttributeService.GetCurrentContactAttributes(contactId);

            // Remove all matches from list, since there is nothing to do with them
            RemoveMatchesFromBothLists(attributesToSave, attributesPersisted);

            var apiUserToken = _apiUserService.GetToken();
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

        private List<ContactAttribute> TranslateToMPAttributes(Dictionary<int, ContactAttributeTypeDTO> contactAttributesTypes)
        {
            var results = new List<ContactAttribute>();

            foreach (var contactAttributeType in contactAttributesTypes.Values)
            {
                foreach (var contactAttribute in contactAttributeType.Attributes)
                {
                    if (!contactAttribute.Selected)
                    {
                        continue;
                    }

                    var mpContactAttribute = new ContactAttribute()
                    {
                        AttributeId = contactAttribute.AttributeId,
                        AttributeTypeId = contactAttributeType.AttributeTypeId,
                        AttributeTypeName = contactAttributeType.Name,
                        StartDate = contactAttribute.StartDate,
                        EndDate = contactAttribute.EndDate,
                        Notes = contactAttribute.Notes
                    };

                    results.Add(mpContactAttribute);
                }
            }
            return results;
        }

        private void RemoveMatchesFromBothLists(List<ContactAttribute> attributesToSave, List<ContactAttribute> attributesPersisted)
        {
            for (int index = attributesToSave.Count - 1; index >= 0; index--)
            {
                var attribute = attributesToSave[index];

                for (int currentIndex = attributesPersisted.Count - 1; currentIndex >= 0; currentIndex--)
                {
                    var currentAttribute = attributesPersisted[currentIndex];

                    if (currentAttribute.AttributeId == attribute.AttributeId && currentAttribute.AttributeTypeId == attribute.AttributeTypeId)
                    {
                        attributesPersisted.RemoveAt(currentIndex);
                        attributesToSave.RemoveAt(index);
                        break;
                    }
                }
            }
        }
    }
}