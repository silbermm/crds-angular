using crds_angular.Models.Crossroads;
using MinistryPlatform.Models;
using System.Collections.Generic;
using crds_angular.Models.Crossroads.Groups;

namespace crds_angular.Services.Interfaces
{
    public interface IGroupService
    {     
        GroupDTO getGroupDetails(int groupId, int contactId, Participant participant, string authUserToken);

        void addParticipantsToGroup(int groupId, List<ParticipantSignup> participants);
    }
}
