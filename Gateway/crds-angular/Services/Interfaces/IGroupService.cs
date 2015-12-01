using crds_angular.Models.Crossroads;
using MinistryPlatform.Models;
using System.Collections.Generic;
using Event = crds_angular.Models.Crossroads.Events.Event;
using crds_angular.Models.Crossroads.Groups;

namespace crds_angular.Services.Interfaces
{
    public interface IGroupService
    {     
        GroupDTO getGroupDetails(int groupId, int contactId, Participant participant, string authUserToken);

        void addParticipantsToGroup(int groupId, List<ParticipantSignup> participants);

        List<Event> GetGroupEvents(int groupId);

        List<GroupContactDTO> GetGroupMembersByEvent(int groupId, int eventId);
    }
}
