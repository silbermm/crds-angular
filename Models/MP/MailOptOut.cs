using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Models.MP
{
    public class MailOptOut
    {
        public int Household_ID { get; set; }
        public bool Bulk_Mail_Opt_Out { get; set; }
    }
}