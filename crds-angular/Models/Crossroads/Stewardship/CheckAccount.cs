﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public class CheckAccount
    {
        [JsonProperty("routingNumber")]
        public string RoutingNumber { get; set; }
        [JsonProperty("accountNumber")]
        public string AccountNumber { get; set; }

    }
}