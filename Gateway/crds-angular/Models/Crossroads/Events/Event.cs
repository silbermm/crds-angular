using System;

namespace crds_angular.Models.Crossroads.Events
{
    public class Event
    {
        public DateTime time { get; set; }
        public string meridian { get; set; }
        public string name { get; set; }
        public string location { get; set; }

    }
}