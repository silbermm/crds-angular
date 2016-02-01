using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Serve
{
    public class ServingTime
    {
        [JsonProperty(PropertyName = "index")]
        public long Index { get; set; }

        [JsonProperty(PropertyName = "time")]
        public string Time { get; set; }

        [JsonProperty(PropertyName = "servingTeams")]
        public List<ServingTeam> ServingTeams { get; set; }

        public ServingTime()
        {
            this.ServingTeams = new List<ServingTeam>();
        }
    }
}