using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Stewardship
{
    // ReSharper disable once InconsistentNaming
    public class DonationDistributionDTO
    {
        [JsonProperty("program_name")]
        public string ProgramName { get; set; }
        [JsonProperty("amount")]
        public int Amount { get; set; }
    }
}