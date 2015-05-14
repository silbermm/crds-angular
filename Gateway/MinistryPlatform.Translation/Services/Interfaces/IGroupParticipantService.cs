using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IGroupParticipantService
    {
        List<GroupServingParticipant> GetServingParticipants(List<int> participants, long from, long to);
    }
}