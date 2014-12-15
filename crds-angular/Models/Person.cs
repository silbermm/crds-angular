using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Models
{
    public class Person
    {
        public int Contact_Id { get; set; }
        public string Email_Address { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Maiden_Name { get; set; }
        public string NickName { get; set; }
        public string Mobile_Phone { get; set; }
        public int? Mobile_Carrier { get; set; }
        public string Date_of_Birth { get; set; }
        public int? Marital_Status_Id { get; set; }
        public int? Gender_Id { get; set; }
        public string Employer_Name { get; set; }
        public string Anniversary_Date { get; set; }
        protected Household Household { get; set; }
        protected Address Address { get; set; }
    }
}