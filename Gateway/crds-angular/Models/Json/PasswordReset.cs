using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Models.Json
{
    public class PasswordReset
    {
        public string Password { get; set; }
        public string Token { get; set; }
    }
}