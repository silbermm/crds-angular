using System.Collections.Generic;

namespace crds_angular.Models.Crossroads.Serve
{
    public class UpdatedEvents
    {
        public List<int> EventIds { get; set; }

        public UpdatedEvents()
        {
            EventIds = new List<int>();
        }
    }
}