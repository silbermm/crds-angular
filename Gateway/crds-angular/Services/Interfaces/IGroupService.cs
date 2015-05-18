using crds_angular.Models.Crossroads;
using MinistryPlatform.Models;
using System.Collections.Generic;

namespace crds_angular.Services.Interfaces
{
    public interface IGroupService
    {
        List<Dictionary<string, object>> addParticipantsToGroup(int groupId, List<int> participantIds);

        GroupDTO getGroupDetails(int groupId, int contactId, Participant participant, string authUserToken);
    }
}
