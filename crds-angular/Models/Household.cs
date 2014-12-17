using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Models
{
    public class Household
    {
        public string Household_ID { get; set; }
        public string Household_Position { get; set; }
        public string Home_Phone { get; set; }
        public int? Congregation_ID { get; set; }
    }
}