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
        
        List<Group> GetMyServingTeams(int contactId, string token);

        bool checkIfUserInGroup(int participantId, IList<int> participants);

        List<GroupSignupRelationships> GetGroupSignupRelations(int groupType, string token); 
    }
}
