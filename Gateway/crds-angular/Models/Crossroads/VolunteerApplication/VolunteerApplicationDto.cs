using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace crds_angular.Models.Crossroads.VolunteerApplication
{
    public class VolunteerApplicationDto
    {
        [JsonProperty(PropertyName = "howLongAttending")]
        [Required]
        public string HowLongAttending { get; set; }

        [JsonProperty(PropertyName = "siteYouAttend")]
        public string SiteYouAttend { get; set; }

        [JsonProperty(PropertyName = "whatServiceDoYouAttend")]
        public string WhatServiceDoYouAttend { get; set; }

        [JsonProperty(PropertyName = "previousName")]
        public string PreviousName { get; set; }

        [JsonProperty(PropertyName = "nameForNameTag")]
        public string NameForNameTag { get; set; }

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
        public string EverBeenArrest { get; set; }

        [JsonProperty(PropertyName = "addictionConcern")]
        public string AddictionConcern { get; set; }

       [JsonProperty(PropertyName = "neglectingChild")]
        public string NeglectingChild { get; set; }

        [JsonProperty(PropertyName = "psychiatricDisorder")]
        public string PsychiatricDisorder { get; set; }

        [JsonProperty(PropertyName = "sexuallyActiveOutsideMarriage")]
        public string SexuallyActiveOutsideMarriage { get; set; }

        [JsonProperty(PropertyName = "spiritualOrientation")]
        public string SpiritualOrientation { get; set; }

        [JsonProperty(PropertyName = "spiritualOrientationExplain")]
        public string SpiritualOrientationExplain { get; set; }

        [JsonProperty(PropertyName = "whatPromptedApplication")]
        public string WhatPromptedApplication { get; set; }

        [JsonProperty(PropertyName = "specialTalents")]
        public string SpecialTalents { get; set; }

        [JsonProperty(PropertyName = "availabilityWeek")]
        public string AvailabilityWeek { get; set; }

        [JsonProperty(PropertyName = "availabilityWeekend")]
        public string AvailabilityWeekend { get; set; }

        [JsonProperty(PropertyName = "availabilitySiteName")]
        public string AvailabilitySiteName { get; set; }

        [JsonProperty(PropertyName = "availabilityServiceTimes")]
        public string AvailabilityServiceTimes { get; set; }

        [JsonProperty(PropertyName = "areaOfInterestServingInClassroom")]
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
}