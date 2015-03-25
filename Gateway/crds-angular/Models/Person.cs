using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using MinistryPlatform.Models;
using Newtonsoft.Json;

namespace crds_angular.Models
{
    public class Person
    {
        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "middleName")]
        public string MiddleName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "maidenName")]
        public string MaidenName { get; set; }

        [JsonProperty(PropertyName = "nickName")]
        public string NickName { get; set; }

        [JsonProperty(PropertyName = "mobilePhone")]
        public string MobilePhone { get; set; }

        [JsonProperty(PropertyName = "dateOfBirth")]
        public string DateOfBirth { get; set; }

        [JsonProperty(PropertyName = "maritalStatusId")]
        public int? MaritalStatusId { get; set; }

        [JsonProperty(PropertyName = "genderId")]
        public int? GenderId { get; set; }

        [JsonProperty(PropertyName = "employerName")]
        public string EmployerName { get; set; }

        [JsonProperty(PropertyName = "anniversaryDate")]
        public string AnniversaryDate { get; set; }

        [JsonProperty(PropertyName = "addressLine1")]
        public string AddressLine1 { get; set; }

        [JsonProperty(PropertyName = "addressLine2")]
        public string AddressLine2 { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty(PropertyName = "homePhone")]
        public string HomePhone { get; set; }

        [JsonProperty(PropertyName = "foreignCountry")]
        public string ForeignCountry { get; set; }

        [JsonProperty(PropertyName = "county")]
        public string County { get; set; }

        [JsonProperty(PropertyName = "congregationId")]
        public int? CongregationId { get; set; }

        [JsonProperty(PropertyName = "householdId")]
        public int HouseholdId { get; set; }

        [JsonProperty(PropertyName = "householdPositionId")]
        public string HouseholdPositionId { get; set; }

        [JsonProperty(PropertyName = "addressId")]
        public int? AddressId { get; set; }

        [JsonProperty(PropertyName = "bulkEmailOptOut")]
        public bool BulkEmailOptOut { get; set; }

        [JsonProperty(PropertyName = "bulkSmsOptOut")]
        public bool BulkSmsOptOut { get; set; }

        [JsonProperty(PropertyName = "bulkMailOptOut")]
        public bool BulkMailOptOut { get; set; }

        [JsonProperty(PropertyName = "mobileCarrierId")]
        public int? MobileCarrierId { get; set; }

        public MyContact GetContact()
        {
                return new MyContact
            {
                ContactId = this.ContactId,
                EmailAddress = this.EmailAddress,
                FirstName = this.FirstName,
                MiddleName = this.MiddleName,
                LastName = this.LastName,
                MaidenName = this.MaidenName,
                NickName = this.NickName,
                MobilePhone = this.MobilePhone,
                MobileCarrierId = this.MobileCarrierId,
                DateOfBirth = this.DateOfBirth,
                MaritalStatusId = this.MaritalStatusId,
                GenderId = this.GenderId,
                EmployerName = this.EmployerName,
                AnniversaryDate = this.AnniversaryDate,
                AddressId = this.AddressId,
                AddressLine1 = this.AddressLine1,
                AddressLine2 = this.AddressLine2,
                City = this.City,
                State = this.State,
                PostalCode = this.PostalCode,
                CongregationId = this.CongregationId,
                HouseholdId = this.HouseholdId
                

            };
        } 

        public Models.MP.Household GetHousehold()
        {
            return new Models.MP.Household
            {
                Household_ID = this.HouseholdId,
                Home_Phone = this.HomePhone,
                Congregation_ID = this.CongregationId,
                Household_Position_ID = this.HouseholdPositionId
            };
        }

        public Models.MP.Address GetAddress()
        {
            return new Models.MP.Address
            {
                Address_ID = this.AddressId,
                Address_Line_1 = this.AddressLine1,
                Address_Line_2 = this.AddressLine2,
                City = this.City,
                State = this.State,
                Postal_Code = this.PostalCode,
                Foreign_Country = this.ForeignCountry,
                County = this.County,
            };
        }
    }   
}