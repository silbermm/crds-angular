using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IParticipantService
    {
        Participant GetParticipant(int contactId);
        List<Response> GetParticipantResponses(int participantId);
        Participant GetParticipantRecord(string token);
        void UpdateParticipant(Dictionary<string, object> getDictionary);
    }
}
