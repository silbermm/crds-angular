using System;
using System.Collections.Generic;
using MinistryPlatform.Models;
using Group = MinistryPlatform.Models.Group;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IGroupService
    {
       int addParticipantToGroup(int participantId, int groupId, int groupRoleId, DateTime startDate,
            DateTime? endDate = null, Boolean? employeeRole = false);

        IList<Event> getAllEventsForGroup(int groupId);

        Group getGroupDetails(int groupId);

        //List<Group> GetServingTeams(int contactId, string token); can't find uses of this TM 08/12

        bool checkIfUserInGroup(int participantId, IList<GroupParticipant> participants);

        bool checkIfRelationshipInGroup(int relationshipId, IList<int> currRelationshipList);
     
        List<GroupSignupRelationships> GetGroupSignupRelations(int groupType); 

        //int CalculateAge(DateTime birthDate, DateTime now); can't find uses of this TM 08/12

       // bool CheckAgeForRelationship(IList<ContactRelationship> familyToReturn, string signupRelations);

        bool ParticipantGroupMember(int groupId, int participantId);

        List<Group> GetGroupsForEvent(int eventId);
    }
}
