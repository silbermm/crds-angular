using System;
using crds_angular.Util.Interfaces;

namespace crds_angular.Util
{
    public class DateTimeWrapper : IDateTime
    {
        public DateTime Now
        {
            get { return DateTime.Now; }
        }

        public DateTime Today
        {
            get { return DateTime.Today; }
        }
    }
}