using System.Collections.Generic;
using crds_angular.Models.Json;
using Newtonsoft.Json;


namespace crds_angular.Models.Crossroads
{
    [JsonConverter(typeof(GroupDetailSerializer))]
    public class GroupDetail
    {
        public int groupID { get; set; }
        public bool groupFullInd { get; set; }
        public bool waitListInd { get; set; }
        public int waitListGroupId { get; set; }
    }
}