using System.Collections.Generic;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Trip;

namespace crds_angular.Services.Interfaces
{
    public interface ITripService
    {
        List<TripGroupDto> GetGroupsByEventId(int eventId);
        MyTripsDTO GetMyTrips(int contactId, string token);
        List<TripParticipantDto> Search(string search);
        
    }
}
