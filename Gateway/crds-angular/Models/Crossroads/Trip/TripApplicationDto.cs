using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using crds_angular.Models.Crossroads.Attribute;
using crds_angular.Models.Crossroads.Profile;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class TripApplicationDto
    {
        [JsonProperty(PropertyName = "contactId")]
        [Required]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "inviteGUID")]
        public string InviteGUID { get; set; }

        [JsonProperty(PropertyName = "pageTwo")]
        public ApplicationPageTwo PageTwo { get; set; }

        [JsonProperty(PropertyName = "pageThree")]
        public ApplicationPageThree PageThree { get; set; }

        [JsonProperty(PropertyName = "pageFour")]
        public ApplicationPageFour PageFour { get; set; }

        [JsonProperty(PropertyName = "pageFive")]
        public ApplicationPageFive PageFive { get; set; }

        [JsonProperty(PropertyName = "pageSix")]
        public ApplicationPageSix PageSix { get; set; }

        [JsonProperty(PropertyName = "pledgeCampaignId")]
        public int PledgeCampaignId { get; set; }

        public class ApplicationPageOne
        {
            [JsonProperty(PropertyName = "firstName")]
            [Required]
            public string FirstName { get; set; }

            [JsonProperty(PropertyName = "lastName")]
            [Required]
            public string LastName { get; set; }
        }

        public class ApplicationPageTwo
        {
            [JsonProperty(PropertyName = "allergies")]
            public string Allergies { get; set; }

            [JsonProperty(PropertyName = "conditions")]
            public string Conditions { get; set; }

            [JsonProperty(PropertyName = "guardianFirstName")]
            public string GuardianFirstName { get; set; }

            [JsonProperty(PropertyName = "guardianLastName")]
            public string GuardianLastName { get; set; }

            [JsonProperty(PropertyName = "referral")]
            [Required]
            public string Referral { get; set; }

            [JsonProperty(PropertyName = "scrubSizeTop")]
            public string ScrubSizeTop { get; set; }

            [JsonProperty(PropertyName = "scrubSizeBottom")]
            public string ScrubSizeBottom { get; set; }

            [JsonProperty(PropertyName = "spiritualLife")]
            public string[] SpiritualLife { get; set; }

            //[JsonProperty(PropertyName = "spiritualLifeObedience")]
            //public string SpiritualLifeObedience { get; set; }

            //[JsonProperty(PropertyName = "spiritualLifeReceived")]
            //public string SpiritualLifeReceived { get; set; }

            //[JsonProperty(PropertyName = "spiritualLifeReplicating")]
            //public string SpiritualLifeReplicating { get; set; }

            //[JsonProperty(PropertyName = "spiritualLifeSearching")]
            //public string SpiritualLifeSearching { get; set; }

            [JsonProperty(PropertyName = "tshirtSize")]
            [Required]
            public string TshirtSize { get; set; }

            [JsonProperty(PropertyName = "vegetarian")]
            [Required]
            public string Vegetarian { get; set; }

            [JsonProperty(PropertyName = "why")]
            [Required]
            public string Why { get; set; }
        }

        public class ApplicationPageThree
        {
            [JsonProperty(PropertyName = "emergencyContactEmail")]
            public string EmergencyContactEmail { get; set; }

            [JsonProperty(PropertyName = "emergencyContactFirstName")]
            [Required]
            public string EmergencyContactFirstName { get; set; }

            [JsonProperty(PropertyName = "emergencyContactLastName")]
            [Required]
            public string EmergencyContactLastName { get; set; }

            [JsonProperty(PropertyName = "emergencyContactPrimaryPhone")]
            [Required]
            public string EmergencyContactPrimaryPhone { get; set; }

            [JsonProperty(PropertyName = "emergencyContactSecondaryPhone")]
            public string EmergencyContactSecondaryPhone { get; set; }
        }

        public class ApplicationPageFour
        {
            [JsonProperty(PropertyName = "groupCommonName")]
            public string GroupCommonName { get; set; }

            [JsonProperty(PropertyName = "interestedInGroupLeader")]
            [Required]
            public string InterestedInGroupLeader { get; set; }

            [JsonProperty(PropertyName = "lottery")]
            public string Lottery { get; set; }

            [JsonProperty(PropertyName = "roommateFirstChoice")]
            public string RoommateFirstChoice { get; set; }

            [JsonProperty(PropertyName = "roommateSecondChoice")]
            public string RoommateSecondChoice { get; set; }

            [JsonProperty(PropertyName = "supportPersonEmail")]
            public string SupportPersonEmail { get; set; }

            [JsonProperty(PropertyName = "whyGroupLeader")]
            public string WhyGroupLeader { get; set; }
        }

        public class ApplicationPageFive
        {
            [JsonProperty(PropertyName = "previousTripExperience")]
            [Required]
            public string PreviousTripExperience { get; set; }

            [JsonProperty(PropertyName = "professionaSkills")]
            public string[] ProfessionalSkills { get; set; }

            //[JsonProperty(PropertyName = "professionalSkillBusiness")]
            //public string ProfessionalSkillBusiness { get; set; }

            //[JsonProperty(PropertyName = "professionalSkillConstruction")]
            //public string ProfessionalSkillConstruction { get; set; }

            //[JsonProperty(PropertyName = "professionalSkillDental")]
            //public string ProfessionalSkillDental { get; set; }

            //[JsonProperty(PropertyName = "professionalSkillEducation")]
            //public string ProfessionalSkillEducation { get; set; }

            //[JsonProperty(PropertyName = "professionalSkillInformationTech")]
            //public string ProfessionalSkillInformationTech { get; set; }

            //[JsonProperty(PropertyName = "professionalSkillMedia")]
            //public string ProfessionalSkillMedia { get; set; }

            //[JsonProperty(PropertyName = "professionalSkillMedical")]
            //public string ProfessionalSkillMedical { get; set; }

            //[JsonProperty(PropertyName = "professionalSkillMusic")]
            //public string ProfessionalSkillMusic { get; set; }

            //[JsonProperty(PropertyName = "professionalSkillOther")]
            //public string ProfessionalSkillOther { get; set; }

            //[JsonProperty(PropertyName = "professionalSkillPhotography")]
            //public string ProfessionalSkillPhotography { get; set; }

            //[JsonProperty(PropertyName = "professionalSkillSocialWorker")]
            //public string ProfessionalSkillSocialWorker { get; set; }

            //[JsonProperty(PropertyName = "professionalSkillStudent")]
            //public string ProfessionalSkillStudent { get; set; }

            [JsonProperty(PropertyName = "sponsorChildFirstName")]
            public string SponsorChildFirstName { get; set; }

            [JsonProperty(PropertyName = "sponsorChildInNicaragua")]
            public string SponsorChildInNicaragua { get; set; }

            [JsonProperty(PropertyName = "sponsorChildLastName")]
            public string SponsorChildLastName { get; set; }

            [JsonProperty(PropertyName = "sponsorChildNumber")]
            public string SponsorChildNumber { get; set; }
        }

        public class ApplicationPageSix
        {
            [JsonProperty(PropertyName = "validPassport")]
            public string ValidPassport { get; set; }

            [JsonProperty(PropertyName = "passportExpirationDate")]
            public string PassportExpirationDate { get; set; }

            [JsonProperty(PropertyName = "passportFirstName")]
            public string PassportFirstName { get; set; }

            [JsonProperty(PropertyName = "passportMiddleName")]
            public string PassportMiddleName { get; set; }

            [JsonProperty(PropertyName = "passportLastName")]
            public string PassportLastName { get; set; }

            [JsonProperty(PropertyName = "passportCountry")]
            public string PassportCountry { get; set; }

            [JsonProperty(PropertyName = "passportNumber")]
            public string PassportNumber { get; set; }

            [JsonProperty(PropertyName = "frequentFlyers")]
            public string[] FrequentFlyers { get; set; }

            //[JsonProperty(PropertyName = "deltaFrequentFlyer")]
            //public string DeltaFrequentFlyer { get; set; }

            //[JsonProperty(PropertyName = "southAfricanFrequentFlyer")]
            //public string SouthAfricanFrequentFlyer { get; set; }

            //[JsonProperty(PropertyName = "unitedFrequentFlyer")]
            //public string UnitedFrequentFlyer { get; set; }

            //[JsonProperty(PropertyName = "usAirwaysFrequentFlyer")]
            //public string UsAirwaysFrequentFlyer { get; set; }

            [JsonProperty(PropertyName = "internationalTravelExpericence")]
            public string InternationalTravelExpericence { get; set; }

            [JsonProperty(PropertyName = "experienceAbroad")]
            public string ExperienceAbroad { get; set; }

            [JsonProperty(PropertyName = "describeExperienceAbroad")]
            public string DescribeExperienceAbroad { get; set; }

            [JsonProperty(PropertyName = "pastAbuseHistory")]
            public string PastAbuseHistory { get; set; }
        }

    }
}
