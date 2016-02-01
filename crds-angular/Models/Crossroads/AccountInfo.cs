using crds_angular.Models.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Models.Crossroads
{
    [JsonConverter(typeof(AccountInfoSerializer))]
    public class AccountInfo
    {
        public bool EmailNotifications { get; set; }
        public bool TextNotifications { get; set; }
        public bool PaperlessStatements { get; set; }
    }
}