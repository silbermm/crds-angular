using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.VolunteerApplication
{
    public class StudentApplicationDto : VolunteerApplicationDto
    {
        [JsonProperty(PropertyName = "firstName")]
        [Required]
        public CustomField FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        [Required]
        public CustomField LastName { get; set; }

        [JsonProperty(PropertyName = "middleInitial")]
        public CustomField MiddleInitial { get; set; }

        [JsonProperty(PropertyName = "email")]
        [Required]
        public CustomField Email { get; set; }

        [JsonProperty(PropertyName = "nameForNameTag")]
        [Required]
        public CustomField NameForNameTag { get; set; }

        [JsonProperty(PropertyName = "birthDate")]
        [Required]
        public CustomField BirthDate { get; set; }

        [JsonProperty(PropertyName = "gender")]
        [Required]
        public CustomField Gender { get; set; }

        [JsonProperty(PropertyName = "howLongAttending")]
        [Required]
        public CustomField HowLongAttending { get; set; }

        [JsonProperty(PropertyName = "serviceAttend")]
        [Required]
        public CustomField ServiceAttend { get; set; }

        [JsonProperty(PropertyName = "streetAddress")]
        [Required]
        public CustomField StreetAddress { get; set; }

        [JsonProperty(PropertyName = "city")]
        [Required]
        public CustomField City { get; set; }

        [JsonProperty(PropertyName = "state")]
        [Required]
        public CustomField State { get; set; }

        [JsonProperty(PropertyName = "zip")]
        [Required]
        public CustomField Zip { get; set; }

        [JsonProperty(PropertyName = "mobilePhone")]
        [Required]
        public CustomField MobilePhone { get; set; }

        [JsonProperty(PropertyName = "homePhone")]
        [Required]
        public CustomField HomePhone { get; set; }

        [JsonProperty(PropertyName = "school")]
        [Required]
        public CustomField School { get; set; }

        [JsonProperty(PropertyName = "grade")]
        [Required]
        public CustomField Grade { get; set; }

        [JsonProperty(PropertyName = "religionSearchingForAnswers")]
        [Required]
        public CustomField ReligionSearchingForAnswers { get; set; }

        [JsonProperty(PropertyName = "religionReceivedJesus")]
        [Required]
        public CustomField ReligionReceivedJesus { get; set; }

        [JsonProperty(PropertyName = "religionFocusingOnObedience")]
        [Required]
        public CustomField ReligionFocusingOnObedience { get; set; }

        [JsonProperty(PropertyName = "religionReplicating")]
        [Required]
        public CustomField ReligionReplicating { get; set; }

        [JsonProperty(PropertyName = "explainFaith")]
        [Required]
        public CustomField ExplainFaith { get; set; }

        [JsonProperty(PropertyName = "whyServe")]
        [Required]
        public CustomField WhyServe { get; set; }

        [JsonProperty(PropertyName = "specialTalents")]
        [Required]
        public CustomField SpecialTalents { get; set; }

        [JsonProperty(PropertyName = "availabilityDuringWeek")]
        [Required]
        public CustomField AvailabilityDuringWeek { get; set; }

        [JsonProperty(PropertyName = "availabilityDuringWeekend")]
        [Required]
        public CustomField AvailabilityDuringWeekend { get; set; }

        [JsonProperty(PropertyName = "availabilityWeekendSite")]
        public CustomField AvailabilityWeekendSite { get; set; }

        [JsonProperty(PropertyName = "serveAgeKids1to2")]
        [Required]
        public CustomField ServeAgeKids1To2 { get; set; }

        [JsonProperty(PropertyName = "serveAgeKids3toPreK")]
        [Required]
        public CustomField ServeAgeKids3ToPreK { get; set; }

        [JsonProperty(PropertyName = "serveAgeKidsKto5Grade")]
        [Required]
        public CustomField ServeAgeKidsKto5Grade { get; set; }

        [JsonProperty(PropertyName = "reference1Name")]
        [Required]
        public CustomField Reference1Name { get; set; }

        [JsonProperty(PropertyName = "reference1timeKnown")]
        [Required]
        public CustomField Reference1TimeKnown { get; set; }

        [JsonProperty(PropertyName = "reference1homePhone")]
        [Required]
        public CustomField Reference1HomePhone { get; set; }

        [JsonProperty(PropertyName = "reference1mobilePhone")]
        [Required]
        public CustomField Reference1MobilePhone { get; set; }

        [JsonProperty(PropertyName = "reference1workPhone")]
        [Required]
        public CustomField Reference1WorkPhone { get; set; }

        [JsonProperty(PropertyName = "reference1email")]
        [Required]
        public CustomField Reference1Email { get; set; }

        [JsonProperty(PropertyName = "reference1association")]
        [Required]
        public CustomField Reference1Association { get; set; }

        [JsonProperty(PropertyName = "reference1occupation")]
        [Required]
        public CustomField Reference1Occupation { get; set; }


        //ref 2
        [JsonProperty(PropertyName = "reference2Name")]
        [Required]
        public CustomField Reference2Name { get; set; }

        [JsonProperty(PropertyName = "reference2timeKnown")]
        [Required]
        public CustomField Reference2TimeKnown { get; set; }

        [JsonProperty(PropertyName = "reference2homePhone")]
        [Required]
        public CustomField Reference2HomePhone { get; set; }

        [JsonProperty(PropertyName = "reference2mobilePhone")]
        [Required]
        public CustomField Reference2MobilePhone { get; set; }

        [JsonProperty(PropertyName = "reference2workPhone")]
        [Required]
        public CustomField Reference2WorkPhone { get; set; }

        [JsonProperty(PropertyName = "reference2email")]
        [Required]
        public CustomField Reference2Email { get; set; }

        [JsonProperty(PropertyName = "reference2association")]
        [Required]
        public CustomField Reference2Association { get; set; }

        [JsonProperty(PropertyName = "reference2occupation")]
        [Required]
        public CustomField Reference2Occupation { get; set; }

        [JsonProperty(PropertyName = "parentLastName")]
        [Required]
        public CustomField ParentLastName { get; set; }

        [JsonProperty(PropertyName = "parentFirstName")]
        [Required]
        public CustomField ParentFirstName { get; set; }

        [JsonProperty(PropertyName = "parentHomePhone")]
        [Required]
        public CustomField ParentHomePhone { get; set; }

        [JsonProperty(PropertyName = "parentMobilePhone")]
        [Required]
        public CustomField ParentMobilePhone { get; set; }

        [JsonProperty(PropertyName = "parentEmail")]
        [Required]
        public CustomField ParentEmail { get; set; }

        [JsonProperty(PropertyName = "parentSignature")]
        [Required]
        public CustomField ParentSignature { get; set; }

        [JsonProperty(PropertyName = "parentSignatureDate")]
        [Required]
        public CustomField ParentSignatureDate { get; set; }

        [JsonProperty(PropertyName = "studentSignature")]
        [Required]
        public CustomField StudentSignature { get; set; }

        [JsonProperty(PropertyName = "studentSignatureDate")]
        [Required]
        public CustomField StudentSignatureDate { get; set; }


    }
}