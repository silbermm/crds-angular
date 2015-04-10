using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IParticipantService
    {
        Participant GetParticipant(int contactId);
    }
}
