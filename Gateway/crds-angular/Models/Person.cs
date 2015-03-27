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
        public int? Mobile_Carrier { get; set; }
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
        public int Household_ID { get; set; }
        public string Household_Position_ID { get; set; }
        public int? Address_Id { get; set; }
        public int Participant_Id { get; set; }
        public bool Bulk_Email_Opt_Out { get; set; }
        public bool Bulk_SMS_Opt_Out { get; set; }
        public bool Bulk_Mail_Opt_Out { get; set; }
        //public List<Models.Crossroads.Skill> skills { get; set; }

        public Models.MP.Contact GetContact()
        {
                return new Models.MP.Contact
            {
                Contact_Id = this.Contact_Id,
                Email_Address = this.Email_Address,
                First_Name = this.First_Name,
                Middle_Name = this.Middle_Name,
                Last_Name = this.Last_Name,
                Maiden_Name = this.Maiden_Name,
                NickName = this.NickName,
                Mobile_Phone = this.Mobile_Phone,
                Mobile_Carrier = this.Mobile_Carrier,
                Date_of_Birth = this.Date_of_Birth,
                Marital_Status_Id = this.Marital_Status_Id,
                Gender_Id = this.Gender_Id,
                Employer_Name = this.Employer_Name,
                Anniversary_Date = this.Anniversary_Date
            };
        } 

        public Models.MP.Household GetHousehold()
        {
            return new Models.MP.Household
            {
                Household_ID = this.Household_ID,
                Home_Phone = this.Home_Phone,
                Congregation_ID = this.Congregation_ID,
                Household_Position_ID = this.Household_Position_ID
            };
        }

        public Models.MP.Address GetAddress()
        {
            return new Models.MP.Address
            {
                Address_ID = this.Address_Id,
                Address_Line_1 = this.Address_Line_1,
                Address_Line_2 = this.Address_Line_2,
                City = this.City,
                State = this.State,
                Postal_Code = this.Postal_Code,
                Foreign_Country = this.Foreign_Country,
                County = this.County,
            };
        }
    }   
}