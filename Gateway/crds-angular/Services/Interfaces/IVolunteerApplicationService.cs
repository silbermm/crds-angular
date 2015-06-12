using System.Collections.Generic;
using crds_angular.Models.Crossroads.Serve;

namespace crds_angular.Services.Interfaces
{
    public interface IVolunteerApplicationService
    {
        List<FamilyMember> FamilyThatUserCanSubmitFor(int contactId, string token);
    }
}
