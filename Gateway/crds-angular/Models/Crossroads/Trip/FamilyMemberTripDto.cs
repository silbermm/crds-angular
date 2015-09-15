using System;
using crds_angular.Models.Crossroads.Serve;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class FamilyMemberTripDto : FamilyMember
    {
        [JsonProperty(PropertyName = "signedUp")]
        public Boolean SignedUp { get; set; }

        [JsonProperty(PropertyName = "signedUpDate")]
        public DateTime? SignedUpDate { get; set; }
    }
}