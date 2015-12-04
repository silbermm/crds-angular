using System;
using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IGroupService
    {
        int addParticipantToGroup(int participantId,
                                  int groupId,
                                  int groupRoleId,
                                  Boolean childCareNeeded,
                                  DateTime startDate,
                                  DateTime? endDate = null,
                                  Boolean? employeeRole = false);

        IList<Event> getAllEventsForGroup(int groupId);

        Group getGroupDetails(int groupId);

        bool checkIfUserInGroup(int participantId, IList<GroupParticipant> participants);

        bool checkIfRelationshipInGroup(int relationshipId, IList<int> currRelationshipList);

        List<GroupSignupRelationships> GetGroupSignupRelations(int groupType);

        bool ParticipantQualifiedServerGroupMember(int groupId, int participantId);
        bool ParticipantGroupMember(int groupId, int participantId);

        List<Group> GetGroupsForEvent(int eventId);

        void SendCommunityGroupConfirmationEmail(int participantId, int groupId, bool waitlist, bool childcareNeeded);
        List<GroupParticipant> getEventParticipantsForGroup(int groupId, int eventId);

        IList<string> GetEventTypesForGroup(int groupId, string token = null);
    }
}