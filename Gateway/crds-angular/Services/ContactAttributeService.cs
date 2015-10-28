using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Profile;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using Attribute = MinistryPlatform.Models.Attribute;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Services
{
    public class ContactAttributeService : IContactAttributeService
    {
        private readonly MPInterfaces.IContactAttributeService _mpContactAttributeService;
        private readonly IAttributeService _attributeService;
        private readonly MPInterfaces.IApiUserService _apiUserService;
        private readonly MPInterfaces.IAttributeService _mpAttributeService;

        public ContactAttributeService(
            MPInterfaces.IContactAttributeService mpContactAttributeService, 
            IAttributeService attributeService, 
            MPInterfaces.IApiUserService apiUserService,
            MPInterfaces.IAttributeService mpAttributeService)
        {
            _mpContactAttributeService = mpContactAttributeService;
            _attributeService = attributeService;
            _apiUserService = apiUserService;
            _mpAttributeService = mpAttributeService;
        }

        public ContactAllAttributesDTO GetContactAttributes(int contactId)
        {
            var mpAttributes = _mpAttributeService.GetAttributes(null);
            var mpContactAttributes = _mpContactAttributeService.GetCurrentContactAttributes(contactId);

            var allAttributes = new ContactAllAttributesDTO();

            allAttributes.MultiSelect = TranslateToAttributeTypeDtos(mpContactAttributes, mpAttributes);
            allAttributes.SingleSelect = TranslateToSingleAttributeTypeDtos(mpContactAttributes, mpAttributes);

            return allAttributes;
        }


        private Dictionary<int, ContactAttributeTypeDTO> TranslateToAttributeTypeDtos(List<ContactAttribute> mpContactAttributes, List<Attribute> mpAttributes)
        {
            var mpFilteredAttributes = mpAttributes.Where(x => x.PreventMultipleSelection == false).ToList();

            var attributeTypesDictionary = mpFilteredAttributes
                .Select(x => new {x.AttributeTypeId, x.AttributeTypeName})
                .Distinct()
                .ToDictionary(mpAttributeType => mpAttributeType.AttributeTypeId,
                              mpAttributeType => new ContactAttributeTypeDTO()
                              {
                                  AttributeTypeId = mpAttributeType.AttributeTypeId,
                                  Name = mpAttributeType.AttributeTypeName,
                              });

            

            foreach (var mpAttribute in mpFilteredAttributes)
            {
                var contactAttribute = new ContactAttributeDTO()
                {
                    AttributeId = mpAttribute.AttributeId,
                    Name = mpAttribute.Name,
                    SortOrder = mpAttribute.SortOrder,
                    Selected = false,
                    Category = mpAttribute.Category
                };

                attributeTypesDictionary[mpAttribute.AttributeTypeId].Attributes.Add(contactAttribute);
            }

            foreach (var mpContactAttribute in mpContactAttributes)
            {                
                if (!attributeTypesDictionary.ContainsKey(mpContactAttribute.AttributeTypeId))
                {
                    continue;
                }

                var contactAttributeType = attributeTypesDictionary[mpContactAttribute.AttributeTypeId];
                var contactAttribute = contactAttributeType.Attributes.First(x => x.AttributeId == mpContactAttribute.AttributeId);
                contactAttribute.StartDate = mpContactAttribute.StartDate;
                contactAttribute.EndDate = mpContactAttribute.EndDate;
                contactAttribute.Notes = mpContactAttribute.Notes;
                contactAttribute.Selected = true;
            }

            return attributeTypesDictionary;
        }

        private Dictionary<int, ContactSingleAttributeDTO> TranslateToSingleAttributeTypeDtos(
            List<ContactAttribute> mpContactAttributes, List<Attribute> mpAttributes)
        {
            var mpFilteredAttributes = mpAttributes.Where(x => x.PreventMultipleSelection == true).ToList();

            var attributeTypesDictionary = mpFilteredAttributes
                .Select(x => new { x.AttributeTypeId, x.AttributeTypeName })
                .Distinct()
                .ToDictionary(mpAttributeType => mpAttributeType.AttributeTypeId,
                              mpAttributeType => new ContactSingleAttributeDTO());

            foreach (var mpContactAttribute in mpContactAttributes)
            {                
                if (!attributeTypesDictionary.ContainsKey(mpContactAttribute.AttributeTypeId))
                {
                    continue;
                }

                var mpAttribute = mpAttributes.First(x => x.AttributeId == mpContactAttribute.AttributeId);

                var attribute = _attributeService.ConvertAttributeToAttributeDto(mpAttribute);
                var contactSingleAttribute = attributeTypesDictionary[mpContactAttribute.AttributeTypeId];

                contactSingleAttribute.Value = attribute;
                contactSingleAttribute.Notes = mpContactAttribute.Notes;
            }

            return attributeTypesDictionary;
        }

        public void SaveContactAttributes(int contactId, Dictionary<int, ContactAttributeTypeDTO> contactAttributes, Dictionary<int, ContactSingleAttributeDTO> contactSingleAttributes)
        {
            var currentAttributes = TranslateMultiToMPAttributes(contactAttributes);
            currentAttributes.AddRange(TranslateSingleToMPAttribute(contactSingleAttributes));

            var persistedAttributes = _mpContactAttributeService.GetCurrentContactAttributes(contactId);

            var attributesToSave = GetDataToSave(currentAttributes, persistedAttributes);

            var apiUserToken = _apiUserService.GetToken();
            foreach (var attribute in attributesToSave)
            {
                SaveAttribute(contactId, attribute, apiUserToken);
            }
        }

        private void SaveAttribute(int contactId, ContactAttribute attribute, string apiUserToken)
        {            
            if (attribute.ContactAttributeId == 0)
            {
                // These are new so add them
                _mpContactAttributeService.CreateAttribute(apiUserToken, contactId, attribute);
            }
            else
            {
                // These are existing so update them
                _mpContactAttributeService.UpdateAttribute(apiUserToken, attribute);
            }
        }

        private List<ContactAttribute> TranslateMultiToMPAttributes(Dictionary<int, ContactAttributeTypeDTO> contactAttributesTypes)
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

        private List<ContactAttribute> TranslateSingleToMPAttribute(Dictionary<int, ContactSingleAttributeDTO> contactSingleAttributes)
        {
            var results = new List<ContactAttribute>();

            foreach (var contactSingleAttribute in contactSingleAttributes)
            {
                var contactAttribute = contactSingleAttribute.Value;

                if (contactAttribute.Value == null)
                {
                    continue;
                }

                var mpContactAttribute = new ContactAttribute()
                {
                    AttributeId = contactAttribute.Value.AttributeId,
                    AttributeTypeId = contactSingleAttribute.Key,
                    Notes = contactAttribute.Notes
                };

                results.Add(mpContactAttribute);
            }
            return results;
        }

        private List<ContactAttribute> GetDataToSave(List<ContactAttribute> currentAttributes, List<ContactAttribute> persistedAttributes)
        {
            // prevent side effects by cloning lists
            currentAttributes = new List<ContactAttribute>(currentAttributes);
            persistedAttributes = new List<ContactAttribute>(persistedAttributes);

            for (int index = currentAttributes.Count - 1; index >= 0; index--)
            {
                var attributeToSave = currentAttributes[index];

                bool foundMatch = false;

                for (int currentIndex = persistedAttributes.Count - 1; currentIndex >= 0; currentIndex--)
                {
                    var currentAttribute = persistedAttributes[currentIndex];

                    if (currentAttribute.AttributeId == attributeToSave.AttributeId)
                    {
                        foundMatch = true;
                        if (attributeToSave.Notes == currentAttribute.Notes)
                        {
                            persistedAttributes.RemoveAt(currentIndex);
                            currentAttributes.RemoveAt(index);
                        }
                        else if (attributeToSave.Notes != String.Empty || attributeToSave.Notes == null)
                        {
                            persistedAttributes.RemoveAt(currentIndex);
                            attributeToSave.StartDate = currentAttribute.StartDate;
                            attributeToSave.ContactAttributeId = currentAttribute.ContactAttributeId;
                        }
                        else
                        {
                            currentAttributes.RemoveAt(index);    
                        }                                                
                        break;    
                    }
                }

                if (!foundMatch)
                {
                    // New Entry with no match
                    attributeToSave.StartDate = DateTime.Today;
                }
            }

            foreach (var persisted in persistedAttributes)
            {
                // Existing entry with no match, so effectively remove it by end-dating it
                persisted.EndDate = DateTime.Today;
            }

            var dataToSave = new List<ContactAttribute>(currentAttributes);
            dataToSave.AddRange(persistedAttributes);
            return dataToSave;
        }
    }
}