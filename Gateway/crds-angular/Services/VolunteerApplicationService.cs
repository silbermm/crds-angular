using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Services.Interfaces;

namespace crds_angular.Services
{
    public class VolunteerApplicationService : IVolunteerApplicationService
    {
        private readonly IServeService _serveService;

        public VolunteerApplicationService(IServeService serveService)
        {
            _serveService = serveService;
        }
        public List<FamilyMember> FamilyThatUserCanSubmitFor(int contactId, string token)
        {
            var list = _serveService.GetImmediateFamilyParticipants(contactId, token);
            var removeSpouse = list.Where(s => s.RelationshipId != 1).ToList();
            return removeSpouse;
        }
    }
}