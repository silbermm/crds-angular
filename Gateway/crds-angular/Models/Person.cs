﻿using crds_angular.Models.MP;
using MinistryPlatform.Models;
using Newtonsoft.Json;

namespace crds_angular.Models
{
    public class Person
    {
        [JsonProperty(PropertyName = "addressId")]
        public int? AddressId { get; set; }

        [JsonProperty(PropertyName = "addressLine1")]
        public string AddressLine1 { get; set; }

        [JsonProperty(PropertyName = "addressLine2")]
        public string AddressLine2 { get; set; }

        [JsonProperty(PropertyName = "anniversaryDate")]
        public string AnniversaryDate { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "congregationId")]
        public int? CongregationId { get; set; }

        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "dateOfBirth")]
        public string DateOfBirth { get; set; }

        [JsonProperty(PropertyName = "emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "employerName")]
        public string EmployerName { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "foreignCountry")]
        public string ForeignCountry { get; set; }

        [JsonProperty(PropertyName = "genderId")]
        public int? GenderId { get; set; }

        [JsonProperty(PropertyName = "homePhone")]
        public string HomePhone { get; set; }

        [JsonProperty(PropertyName = "householdId")]
        public int HouseholdId { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "maidenName")]
        public string MaidenName { get; set; }

        [JsonProperty(PropertyName = "maritalStatusId")]
        public int? MaritalStatusId { get; set; }

        [JsonProperty(PropertyName = "middleName")]
        public string MiddleName { get; set; }

        [JsonProperty(PropertyName = "mobileCarrierId")]
        public int? MobileCarrierId { get; set; }

        [JsonProperty(PropertyName = "mobilePhone")]
        public string MobilePhone { get; set; }

        [JsonProperty(PropertyName = "nickName")]
        public string NickName { get; set; }

        [JsonProperty(PropertyName = "postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        public MyContact GetContact()
        {
            return new MyContact
            {
                ContactId = ContactId,
                EmailAddress = EmailAddress,
                FirstName = FirstName,
                MiddleName = MiddleName,
                LastName = LastName,
                MaidenName = MaidenName,
                NickName = NickName,
                MobilePhone = MobilePhone,
                MobileCarrierId = MobileCarrierId,
                DateOfBirth = DateOfBirth,
                MaritalStatusId = MaritalStatusId,
                GenderId = GenderId,
                EmployerName = EmployerName,
                AnniversaryDate = AnniversaryDate,
                AddressId = AddressId,
                AddressLine1 = AddressLine1,
                AddressLine2 = AddressLine2,
                City = City,
                State = State,
                PostalCode = PostalCode,
                CongregationId = CongregationId,
                HouseholdId = HouseholdId
            };
        }

        public Household GetHousehold()
        {
            return new Household
            {
                Household_ID = HouseholdId,
                Home_Phone = HomePhone,
                Congregation_ID = CongregationId
            };
        }

        public Address GetAddress()
        {
            return new Address
            {
                Address_ID = AddressId,
                Address_Line_1 = AddressLine1,
                Address_Line_2 = AddressLine2,
                City = City,
                State = State,
                Postal_Code = PostalCode,
                Foreign_Country = ForeignCountry
            };
        }
    }
}