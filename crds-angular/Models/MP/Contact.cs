using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Models.MP
{
    public class Contact
    {
        public int ContactId { get; set; }
        
        public bool Company { get; set; }
        public string CompanyName { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string DisplayName { get; set; }

        public string DateOfBirth { get; set; }



        public Prefix Prefix { get; set; }
        public Suffix Suffix { get; set; }

        public Gender Gender { get; set; }



    }
}