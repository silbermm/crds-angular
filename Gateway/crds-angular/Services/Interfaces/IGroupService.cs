using crds_angular.Models.Crossroads;
using MinistryPlatform.Models;
using System.Collections.Generic;

namespace crds_angular.Services.Interfaces
{
    public interface IGroupService
    {     
        GroupDTO getGroupDetails(int groupId, int contactId, Participant participant, string authUserToken);

        void addParticipantsToGroup(int groupId, List<ParticipantSignup> participants);
    }
}
