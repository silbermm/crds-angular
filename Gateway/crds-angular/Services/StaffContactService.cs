using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Services
{
    public class StaffContactService : IStaffContactService
    {
        private readonly IContactService _contactService;

        public StaffContactService(IContactService contactService)
        {
            _contactService = contactService;
        }

        public List<PrimaryContactDto> GetStaffContacts(string token)
        {
            var mpContacts = _contactService.StaffContacts(token);
            return mpContacts.Select(mpContact => new PrimaryContactDto
            {
                ContactId = mpContact.ToInt("Contact ID"),
                DisplayName = mpContact.ToString("Display Name"),
                Email = mpContact.ToString("dp_RecordName")
            }).ToList();
        }
    }
}