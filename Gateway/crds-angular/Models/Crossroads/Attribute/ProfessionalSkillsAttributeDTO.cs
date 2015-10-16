using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Attribute
{
    public class ProfessionalSkillsAttributeDTO : AttributeTypeDTO
    {
        [JsonProperty(PropertyName = "isChecked")]
        public Boolean? IsChecked { get; set; }
    }
}