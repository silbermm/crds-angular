using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IGroupParticipantService
    {
        int Get(int groupId, int participantId); 
        List<GroupServingParticipant> GetServingParticipants(List<int> participants, long from, long to, int loggedInContactId);
    }
}