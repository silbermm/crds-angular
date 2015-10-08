using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class ContactRelationshipService : BaseService, IContactRelationshipService
    {
        private readonly int _getMyFamilyViewId = Convert.ToInt32(AppSettings("MyContactFamilyRelationshipViewId"));

        private readonly int _getMyCurrentRelationships = Convert.ToInt32((AppSettings("MyContactCurrentRelationships")));

        private IMinistryPlatformService _ministryPlatformService;

        public ContactRelationshipService(IMinistryPlatformService ministryPlatformService, IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
        {
            this._ministryPlatformService = ministryPlatformService;
        }

        public IEnumerable<ContactRelationship> GetMyImmediatieFamilyRelationships(int contactId, string token)
        {
            var viewRecords = _ministryPlatformService.GetSubpageViewRecords("MyContactFamilyRelationshipViewId",
                contactId, token);

            return viewRecords.Select(viewRecord => new ContactRelationship
            {
                Contact_Id = viewRecord.ToInt("Contact_ID"),
                Email_Address = viewRecord.ToString("Email_Address"),
                Last_Name = viewRecord.ToString("Last Name"),
                Preferred_Name = viewRecord.ToString("Preferred Name"),
                Participant_Id = viewRecord.ToInt("Participant_ID"),
                Relationship_Id = viewRecord.ToInt("Relationship_ID"), 
                Age = viewRecord.ToInt("Age")
            }).ToList();
        }

        public IEnumerable<ContactRelationship> GetMyCurrentRelationships(int contactId, string token)
        {
            var viewRecords = _ministryPlatformService.GetSubpageViewRecords(_getMyCurrentRelationships, contactId,
                token);
            try
            {
                return viewRecords.Select(viewRecord => new ContactRelationship
                {
                    Contact_Id = (int) viewRecord["Contact_ID"],
                    Email_Address = (string) viewRecord["Email_Address"],
                    Last_Name = (string) viewRecord["Last Name"],
                    Preferred_Name = (string) viewRecord["Preferred Name"],
                    Participant_Id = (int) viewRecord["Participant_ID"],
                    Relationship_Id = (int) viewRecord["Relationship_ID"]
                }).ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public int AddRelationship(Relationship relationship )
        {
            //var records = _ministryPlatformService.GetSubPageRecord("ContactRelationships",
            //                                          5337888,
            //                                          ApiLogin());
            try
            {
                var dict = new Dictionary<string, object>
                {
                    {"Relationship_ID", relationship.RelationshipID},
                    {"Related_Contact_ID", relationship.RelatedContactID},
                    {"Start_Date", relationship.StartDate},
                    {"End_Date", relationship.EndDate}
                };
                return _ministryPlatformService.CreateSubRecord(_configurationWrapper.GetConfigIntValue("ContactRelationships"),
                                                         _configurationWrapper.GetConfigIntValue("Contacts"),
                                                         dict,
                                                         ApiLogin());
            }
            catch (Exception e)
            {
                return -1;
            }
            
        }
        
    }
}