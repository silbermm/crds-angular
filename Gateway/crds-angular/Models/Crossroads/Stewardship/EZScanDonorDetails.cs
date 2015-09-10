using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MinistryPlatform.Models;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public class EZScanDonorDetails
    {
        [JsonProperty(PropertyName = "DisplayName")]
        public string DisplayName { get; set; }
        [JsonProperty(PropertyName = "PostalAddress")]
        public PostalAddress Address { get; set; }
    }
}