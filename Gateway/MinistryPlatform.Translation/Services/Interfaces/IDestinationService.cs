using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IDestinationService
    {
        List<TripDocuments> DocumentsForDestination(int destinationId);
    }
}