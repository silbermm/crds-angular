using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MaidenName { get; set; }
        public string NickName { get; set; }
        public string MobilePhone { get; set; }
        public string ServiceProvider { get; set; }
        public string BirthDate { get; set; }
        public string MaritalStatus { get; set; }
        public string Gender { get; set; }
        public string Employer { get; set; }
        public string CrossroadsStartDate { get; set; }
        public Household Household { get; set; }
        public Address Address { get; set; }
    }
}