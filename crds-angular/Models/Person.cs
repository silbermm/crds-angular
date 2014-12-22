using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public int? Mobile_Carrier_ID { get; set; }
        public string Date_of_Birth { get; set; }
        public int? Marital_Status_Id { get; set; }
        public int? Gender_Id { get; set; }
        public string Employer_Name { get; set; }
        public string Anniversary_Date { get; set; }
        public string Address_Line_1 { get; set; }
        public string Address_Line_2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Postal_Code { get; set; }
        public string Home_Phone { get; set; }
        public string Foreign_Country { get; set; }
        public string County { get; set; }
        public int? Congregation_ID { get; set; }
    }   
}