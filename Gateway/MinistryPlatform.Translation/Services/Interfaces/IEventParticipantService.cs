using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IEventParticipantService
    {
        List<TripParticipant> TripParticipants(string search);
        
    }
}
