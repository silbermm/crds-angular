using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Models.MP
{
    public class EmailSMSOptOut
    {
        public int Contact_ID { get; set; }
        public bool Bulk_Email_Opt_Out { get; set; }
        public bool Bulk_SMS_Opt_Out { get; set; }

    }
}