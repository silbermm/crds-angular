using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Models.Crossroads.Profile;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Attribute
{
    public class SpiritualJourneyContactAttributeDTO : AttributeTypeDTO
    {
        [JsonProperty(PropertyName = "isChecked")]
        public Boolean? IsChecked { get; set; }       
    }
}