using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Models
{
    public class Profile
    {
        public Person person { get; set; }
        public Household household { get; set; }
    }
}