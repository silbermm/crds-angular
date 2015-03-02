using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinistryPlatform.Translation.Models
{
    public class CommunicationPreferences
    {
        public bool Bulk_Email_Opt_Out { get; set; }
        public bool Bulk_SMS_Opt_Out { get; set; }
        public bool Bulk_Mail_Opt_Out { get; set; }
    }
}
