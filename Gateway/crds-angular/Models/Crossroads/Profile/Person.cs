using System.Collections.Generic;
using MinistryPlatform.Models;
using Newtonsoft.Json;
using HouseholdMember = MinistryPlatform.Models.HouseholdMember;


namespace crds_angular.Models.Crossroads.Profile
{
    public class Person
    {
        [JsonProperty(PropertyName = "addressId")]
        public int? AddressId { get; set; }

        [JsonProperty(PropertyName = "addressLine1")]
        public string AddressLine1 { get; set; }

        [JsonProperty(PropertyName = "addressLine2")]
        public string AddressLine2 { get; set; }

        [JsonProperty(PropertyName = "age")]
        public int Age { get; set; }

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

        [JsonProperty(PropertyName = "householdName")]
        public string HouseholdName { get; set; }

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

        [JsonProperty(PropertyName = "householdMembers")]
        public List<HouseholdMember> HouseholdMembers { get; set; }

        public List<ContactAttribute> Attributes { get; set; }

        public MyContact GetContact()
        {
            return new MyContact
            {
                Contact_ID = ContactId,
                Email_Address = EmailAddress,
                First_Name = FirstName,
                Middle_Name = MiddleName,
                Last_Name = LastName,
                Maiden_Name = MaidenName,
                Nickname = NickName,
                Mobile_Phone = MobilePhone,
                Mobile_Carrier = MobileCarrierId,
                Date_Of_Birth = DateOfBirth,
                Marital_Status_ID = MaritalStatusId,
                Gender_ID = GenderId,
                Employer_Name = EmployerName,
                Anniversary_Date = AnniversaryDate,
                Address_ID = AddressId,
                Address_Line_1 = AddressLine1,
                Address_Line_2 = AddressLine2,
                City = City,
                State = State,
                Postal_Code = PostalCode,
                Congregation_ID = CongregationId,
                Household_ID = HouseholdId
            };
        }

        public MinistryPlatform.Models.Household GetHousehold()
        {
            return new MinistryPlatform.Models.Household
            {
                Household_ID = HouseholdId,
                Home_Phone = HomePhone,
                Congregation_ID = CongregationId
            };
        }

        public crds_angular.Models.MP.Address GetAddress()
        {
            return new crds_angular.Models.MP.Address
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