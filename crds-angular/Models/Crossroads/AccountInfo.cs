using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Models.Crossroads
{
    public class AccountInfo
    {
        public int ContactId {get; set;}
        public string NewPassword { get; set; }
        public bool EmailNotifications { get; set; }
        public bool TextNotifications { get; set; }
        public bool PaperlessStatements { get; set; }



    }
}