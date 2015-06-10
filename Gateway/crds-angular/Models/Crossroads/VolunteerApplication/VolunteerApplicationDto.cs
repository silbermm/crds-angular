using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace crds_angular.Models.Crossroads.VolunteerApplication
{
    public class MinistryPlatformFieldAttribute : Attribute
    {
        public int Id;
        public string Label;
    }

    public class VolunteerApplicationDto
    {
        [JsonProperty(PropertyName = "contactId")]
        [Required]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "formId")]
        [Required]
        public int FormId { get; set; }

        [JsonProperty(PropertyName = "opportunityId")]
        [Required]
        public int OpportunityId { get; set; }

        [JsonProperty(PropertyName = "responseOpportunityId")]
        [Required]
        public int ResponseOpportunityId { get; set; }

        //[JsonProperty(PropertyName = "answers")]
        //[Required]
        //public Fields FormResponses { get; set; }
    }

    public class AdultApplicationDto : VolunteerApplicationDto
    {
        [JsonProperty(PropertyName = "answers")]
        [Required]
        public Fields FormResponses { get; set; }
    }

    public enum Gender
    {
        None, Male = 1, Female = 2
    }
    
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

        [JsonProperty(PropertyName = "site")]
        [Required]
        public CustomField SiteAttend { get; set; }

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

        [JsonProperty(PropertyName = "whereYouAre")]
        [Required]
        public CustomField WhereYouAre { get; set; }

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

        [JsonProperty(PropertyName = "availabilityOakley")]
        [Required]
        public CustomField AvailabilityOakley { get; set; }

        [JsonProperty(PropertyName = "availabilityFlorence")]
        [Required]
        public CustomField AvailabilityFlorence { get; set; }

        [JsonProperty(PropertyName = "availabilityWestSide")]
        [Required]
        public CustomField AvailabilityWestSide { get; set; }

        [JsonProperty(PropertyName = "availabilityMason")]
        [Required]
        public CustomField AvailabilityMason { get; set; }

        [JsonProperty(PropertyName = "availabilityClifton")]
        [Required]
        public CustomField AvailabilityClifton { get; set; }

        [JsonProperty(PropertyName = "serveServiceTimes")]
        [Required]
        public CustomField ServeServiceTimes { get; set; }

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

    public class CustomField
    {
        public int CrossroadsId { get; set; }
        public string Value { get; set; }

        //public int FormFieldId
        //{
        //    get
        //    {
        //        //call MP to get ID
        //        return 1;
        //    }
        //}
    }


    public class Fields
    {
        [JsonProperty(PropertyName = "howLongAttending")]
        [Required]
        public CustomField HowLongAttending { get; set; }

        [JsonProperty(PropertyName = "siteYouAttend")]
        public CustomField SiteYouAttend { get; set; }

        [JsonProperty(PropertyName = "whatServiceDoYouAttend")]
        public CustomField WhatServiceDoYouAttend { get; set; }

        [JsonProperty(PropertyName = "previousName")]
        public CustomField PreviousName { get; set; }

        [JsonProperty(PropertyName = "nameForNameTag")]
        public CustomField NameForNameTag { get; set; }

        [JsonProperty(PropertyName = "employer")]
        public string Employer { get; set; }

        [JsonProperty(PropertyName = "companyName")]
        public string CompanyName { get; set; }

        [JsonProperty(PropertyName = "position")]
        public string Position { get; set; }

        [JsonProperty(PropertyName = "workPhone")]
        public string WorkPhone { get; set; }

        [JsonProperty(PropertyName = "spouseName")]
        public string SpouseName { get; set; }

        [JsonProperty(PropertyName = "spouseGender")]
        public string SpouseGender { get; set; }

        [JsonProperty(PropertyName = "children")]
        public List<Child> Children { get; set; }

        [JsonProperty(PropertyName = "everBeenArrest")]
        [MinistryPlatformField(Id = 36)]
        public string EverBeenArrest { get; set; }

        [JsonProperty(PropertyName = "addictionConcern")]
        [MinistryPlatformField(Id = 38)]
        public string AddictionConcern { get; set; }

        [JsonProperty(PropertyName = "neglectingChild")]
        public string NeglectingChild { get; set; }

        [JsonProperty(PropertyName = "psychiatricDisorder")]
        [MinistryPlatformField(Id = 41)]
        public string PsychiatricDisorder { get; set; }

        [JsonProperty(PropertyName = "sexuallyActiveOutsideMarriage")]
        [MinistryPlatformField(Id = 44)]
        public string SexuallyActiveOutsideMarriage { get; set; }

        [JsonProperty(PropertyName = "spiritualOrientation")]
        [MinistryPlatformField(Id = 47)]
        public string SpiritualOrientation { get; set; }

        [JsonProperty(PropertyName = "spiritualOrientationExplain")]
        [MinistryPlatformField(Id = 48)]
        public string SpiritualOrientationExplain { get; set; }

        [JsonProperty(PropertyName = "whatPromptedApplication")]
        [MinistryPlatformField(Id = 50)]
        public string WhatPromptedApplication { get; set; }

        [JsonProperty(PropertyName = "specialTalents")]
        [MinistryPlatformField(Id = 51)]
        public string SpecialTalents { get; set; }

        [JsonProperty(PropertyName = "availabilityWeek")]
        [MinistryPlatformField(Id = 54)]
        public string AvailabilityWeek { get; set; }

        [JsonProperty(PropertyName = "availabilityWeekend")]
        [MinistryPlatformField(Id = 55)]
        public string AvailabilityWeekend { get; set; }

        [JsonProperty(PropertyName = "availabilitySiteName")]
        [MinistryPlatformField(Id = 56)]
        public string AvailabilitySiteName { get; set; }

        [JsonProperty(PropertyName = "availabilityServiceTimes")]
        [MinistryPlatformField(Id = 58)]
        public string AvailabilityServiceTimes { get; set; }

        [JsonProperty(PropertyName = "areaOfInterestServingInClassroom")]
        [MinistryPlatformField(Id = 60)]
        public string AreaOfInterestServingInClassroom { get; set; }

        [JsonProperty(PropertyName = "areaOfInterestWelcomingNewFamilies")]
        public string AreaOfInterestWelcomingNewFamilies { get; set; }

        [JsonProperty(PropertyName = "areaOfInterestHelpSpecialNeeds")]
        public string AreaOfInterestHelpSpecialNeeds { get; set; }

        [JsonProperty(PropertyName = "areaOfInterestTech")]
        public string AreaOfInterestTech { get; set; }

        [JsonProperty(PropertyName = "areaOfInterestRoomPrep")]
        public string AreaOfInterestRoomPrep { get; set; }

        [JsonProperty(PropertyName = "areaOfInterestAdminTasks")]
        public string AreaOfInterestAdminTasks { get; set; }

        [JsonProperty(PropertyName = "areaOfInterestShoppingForSupplies")]
        public string AreaOfInterestShoppingForSupplies { get; set; }

        [JsonProperty(PropertyName = "areaOfInterestCreatingWeekendExperience")]
        public string AreaOfInterestCreatingWeekendExperience { get; set; }

        [JsonProperty(PropertyName = "whatAgeBirthToTwo")]
        public string WhatAgeBirthToTwo { get; set; }

        [JsonProperty(PropertyName = "whatAgeOneToTwo")]
        public string WhatAgeOneToTwo { get; set; }

        [JsonProperty(PropertyName = "whatAgeThreeToPreK")]
        public string WhatAgeThreeToPreK { get; set; }

        [JsonProperty(PropertyName = "whatAgeKToFifth")]
        public string WhatAgeKToFifth { get; set; }

        [JsonProperty(PropertyName = "references")]
        public List<Reference> References { get; set; }

        [JsonProperty(PropertyName = "agree")]
        public string Agree { get; set; }

        [JsonProperty(PropertyName = "agreeDate")]
        public string AgreeDate { get; set; }
    }

    public class Child
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "birthdate")]
        public string Birthdate { get; set; }
    }

    public class Reference
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "lengthOfTimeKnown")]
        public string LengthOfTimeKnown { get; set; }

        [JsonProperty(PropertyName = "homePhone")]
        public string HomePhone { get; set; }

        [JsonProperty(PropertyName = "siteYouAttend")]
        public string MobilPhone { get; set; }

        [JsonProperty(PropertyName = "siteYouAttend")]
        public string WorkPhone { get; set; }

        [JsonProperty(PropertyName = "siteYouAttend")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "siteYouAttend")]
        public string NatureOfAssociation { get; set; }

        [JsonProperty(PropertyName = "siteYouAttend")]
        public string Occupation { get; set; }
    }
}