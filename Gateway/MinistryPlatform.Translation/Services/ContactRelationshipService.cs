using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class ContactRelationshipService : BaseService, IContactRelationshipService
    {
        private readonly int _getMyFamilyViewId = Convert.ToInt32(AppSettings("MyContactFamilyRelationshipViewId"));

        private IMinistryPlatformService _ministryPlatformService;

        public ContactRelationshipService(IMinistryPlatformService ministryPlatformService)
        {
            this._ministryPlatformService = ministryPlatformService;
        }

        public IEnumerable<Contact_Relationship> GetMyImmediatieFamilyRelationships(int contactId, string token)
        {
            var viewRecords = _ministryPlatformService.GetSubpageViewRecords(_getMyFamilyViewId, contactId, token);

            return viewRecords.Select(viewRecord => new Contact_Relationship
            {
                Contact_Id = (int)viewRecord["Contact_ID"],
                Email_Address = (string)viewRecord["Email_Address"],
                Last_Name = (string)viewRecord["Last Name"],
                Preferred_Name = (string)viewRecord["Preferred Name"]
            }).ToList();
        }
    }
}
