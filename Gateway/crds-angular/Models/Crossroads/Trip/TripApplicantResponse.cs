using System.Collections.Generic;

namespace crds_angular.Models.Crossroads.Trip
{
    public class TripApplicantResponse
    {

        public List<TripApplicant> Applicants { get; set; }
        public TripInfo TripInfo { get; set; }
    }

    public class TripInfo
    {
        public int EventId { get; set; }
        public decimal FundraisingGoal { get; set; }
        public int PledgeCampaignId { get; set; }
    }

    //public class TripApplicant
    //{
        
    //}
}