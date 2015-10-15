using System;
using System.Collections.Generic;
using AutoMapper;
using crds_angular.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Profile;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using MPServices = MinistryPlatform.Translation.Services.Interfaces;
using MinistryPlatform.Models.DTO;
using MinistryPlatform.Translation.Services;


namespace crds_angular.Services
{
    public class PersonService : MinistryPlatformBaseService, IPersonService
    {
        private readonly MPServices.IContactService _contactService;
        private readonly IContactAttributeService _contactAttributeService;

        public PersonService(MPServices.IContactService contactService, IContactAttributeService contactAttributeService)
        {
            _contactService = contactService;
            _contactAttributeService = contactAttributeService;
        }

        public void SetProfile(String token, Person person)
        {
            var contactDictionary = getDictionary(person.GetContact());
            var householdDictionary = getDictionary(person.GetHousehold());
            var addressDictionary = getDictionary(person.GetAddress());
            addressDictionary.Add("State/Region", addressDictionary["State"]);

            _contactService.UpdateContact(person.ContactId, contactDictionary, householdDictionary, addressDictionary);
        }

        public List<Skill> GetLoggedInUserSkills(int contactId, string token)
        {
            return GetSkills(contactId, token);
        }

        public Person GetPerson(int contactId)
        {
            var contact = _contactService.GetContactById(contactId);
            var person = Mapper.Map<Person>(contact);

            var family = _contactService.GetHouseholdFamilyMembers(person.HouseholdId);
            person.HouseholdMembers = family;

            // TODO: Should this move to _contactService or should update move it's call out to this service?
            var attributesTypes = _contactAttributeService.GetContactAttributes(contactId);
            person.AttributeTypes = attributesTypes;

            return person;
        }

        public List<RoleDto> GetLoggedInUserRoles(string token)
        {
            return GetMyRecords.GetMyRoles(token);
        }

        public Person GetLoggedInUserProfile(String token)
        {
            var contact = _contactService.GetMyProfile(token);
            var person = Mapper.Map<Person>(contact);

            var family = _contactService.GetHouseholdFamilyMembers(person.HouseholdId);
            person.HouseholdMembers = family;

            return person;
        }

        private List<Skill> GetSkills(int recordId, string token)
        {
            var attributes = GetMyRecords.GetMyAttributes(recordId, token);

            var skills =
                Mapper.Map<List<SkillAttribute>, List<Skill>>(attributes);

            return skills;
        }
    }
}