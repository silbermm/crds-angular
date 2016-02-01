using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Models.Json
{
    public class Communication
    {
        public bool EmailNotifications { get; set; }
        public bool TextNotifications { get; set; }
        public bool PaperlessStatements { get; set; }
    }
}