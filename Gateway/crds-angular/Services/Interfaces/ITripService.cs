using System.Collections.Generic;
using crds_angular.Models.Crossroads.Trip;

namespace crds_angular.Services.Interfaces
{
    public interface ITripService
    {
        List<TripParticipantDto> Search(string search);
        MyTripsDTO GetMyTrips(int contactId);
    }
}
