using System.Collections.Generic;

namespace crds_angular.Models.Crossroads.Trip
{
    public class TripApplicantResponse
    {
        public List<TripApplicant> Applicants { get; set; }
        public List<TripToolError> Errors { get; set; }
        public TripInfo TripInfo { get; set; }
    }
}