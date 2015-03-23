using System;

namespace crds_angular.Models.Crossroads
{
    public class ServingRSVP
    {
        public Response Response { get; set; }
        public DateTime Occurrence { get; set; }
    }

    public enum Response
    {
        Yes, No, Maybe
    }
}