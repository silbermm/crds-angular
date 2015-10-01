using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class TripApplicationDto
    {
        [JsonProperty(PropertyName = "contactId")]
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
            public string FirstName { get; set; }

            [JsonProperty(PropertyName = "lastName")]
            public string LastName { get; set; }
        }

        public class ApplicationPageTwo
        {
            [JsonProperty(PropertyName = "allergies")]
            public TripApplicationField Allergies { get; set; }

            [JsonProperty(PropertyName = "conditions")]
            public TripApplicationField Conditions { get; set; }

            [JsonProperty(PropertyName = "guardianFirstName")]
            public TripApplicationField GuardianFirstName { get; set; }

            [JsonProperty(PropertyName = "guardianLastName")]
            public TripApplicationField GuardianLastName { get; set; }

            [JsonProperty(PropertyName = "referral")]
            public TripApplicationField Referral { get; set; }

            [JsonProperty(PropertyName = "scrubSize")]
            public TripApplicationField ScrubSize { get; set; }

            [JsonProperty(PropertyName = "spiritualLifeObedience")]
            public TripApplicationField SpiritualLifeObedience { get; set; }

            [JsonProperty(PropertyName = "spiritualLifeReceived")]
            public TripApplicationField SpiritualLifeReceived { get; set; }

            [JsonProperty(PropertyName = "spiritualLifeReplicating")]
            public TripApplicationField SpiritualLifeReplicating { get; set; }

            [JsonProperty(PropertyName = "spiritualLifeSearching")]
            public TripApplicationField SpiritualLifeSearching { get; set; }

            [JsonProperty(PropertyName = "tshirtSize")]
            public TripApplicationField TshirtSize { get; set; }

            [JsonProperty(PropertyName = "vegetarian")]
            public TripApplicationField Vegetarian { get; set; }

            [JsonProperty(PropertyName = "why")]
            public TripApplicationField Why { get; set; }
        }

        public class ApplicationPageThree
        {
            [JsonProperty(PropertyName = "emergencyContactEmail")]
            public TripApplicationField EmergencyContactEmail { get; set; }

            [JsonProperty(PropertyName = "emergencyContactFirstName")]
            public TripApplicationField EmergencyContactFirstName { get; set; }

            [JsonProperty(PropertyName = "emergencyContactLastName")]
            public TripApplicationField EmergencyContactLastName { get; set; }

            [JsonProperty(PropertyName = "emergencyContactPrimaryPhone")]
            public TripApplicationField EmergencyContactPrimaryPhone { get; set; }

            [JsonProperty(PropertyName = "emergencyContactSecondaryPhone")]
            public TripApplicationField EmergencyContactSecondaryPhone { get; set; }
        }

        public class ApplicationPageFour
        {
            [JsonProperty(PropertyName = "groupCommonName")]
            public TripApplicationField GroupCommonName { get; set; }

            [JsonProperty(PropertyName = "interestedInGroupLeader")]
            public TripApplicationField InterestedInGroupLeader { get; set; }

            [JsonProperty(PropertyName = "lottery")]
            public TripApplicationField Lottery { get; set; }

            [JsonProperty(PropertyName = "roommateFirstChoice")]
            public TripApplicationField RoommateFirstChoice { get; set; }

            [JsonProperty(PropertyName = "roommateSecondChoice")]
            public TripApplicationField RoommateSecondChoice { get; set; }

            [JsonProperty(PropertyName = "supportPersonEmail")]
            public TripApplicationField SupportPersonEmail { get; set; }

            [JsonProperty(PropertyName = "whyGroupLeader")]
            public TripApplicationField WhyGroupLeader { get; set; }
        }

        public class ApplicationPageFive
        {
            [JsonProperty(PropertyName = "previousTripExperience")]
            public TripApplicationField PreviousTripExperience { get; set; }

            [JsonProperty(PropertyName = "professionalSkillBusiness")]
            public TripApplicationField ProfessionalSkillBusiness { get; set; }

            [JsonProperty(PropertyName = "professionalSkillConstruction")]
            public TripApplicationField ProfessionalSkillConstruction { get; set; }

            [JsonProperty(PropertyName = "professionalSkillDental")]
            public TripApplicationField ProfessionalSkillDental { get; set; }

            [JsonProperty(PropertyName = "professionalSkillEducation")]
            public TripApplicationField ProfessionalSkillEducation { get; set; }

            [JsonProperty(PropertyName = "professionalSkillInformationTech")]
            public TripApplicationField ProfessionalSkillInformationTech { get; set; }

            [JsonProperty(PropertyName = "professionalSkillMedia")]
            public TripApplicationField ProfessionalSkillMedia { get; set; }

            [JsonProperty(PropertyName = "professionalSkillMedical")]
            public TripApplicationField ProfessionalSkillMedical { get; set; }

            [JsonProperty(PropertyName = "professionalSkillMusic")]
            public TripApplicationField ProfessionalSkillMusic { get; set; }

            [JsonProperty(PropertyName = "professionalSkillOther")]
            public TripApplicationField ProfessionalSkillOther { get; set; }

            [JsonProperty(PropertyName = "professionalSkillPhotography")]
            public TripApplicationField ProfessionalSkillPhotography { get; set; }

            [JsonProperty(PropertyName = "professionalSkillSocialWorker")]
            public TripApplicationField ProfessionalSkillSocialWorker { get; set; }

            [JsonProperty(PropertyName = "professionalSkillStudent")]
            public TripApplicationField ProfessionalSkillStudent { get; set; }

            [JsonProperty(PropertyName = "sponsorChildFirstName")]
            public TripApplicationField SponsorChildFirstName { get; set; }

            [JsonProperty(PropertyName = "sponsorChildInNicaragua")]
            public TripApplicationField SponsorChildInNicaragua { get; set; }

            [JsonProperty(PropertyName = "sponsorChildLastName")]
            public TripApplicationField SponsorChildLastName { get; set; }

            [JsonProperty(PropertyName = "sponsorChildNumber")]
            public TripApplicationField SponsorChildNumber { get; set; }
        }

        public class ApplicationPageSix
        {
            [JsonProperty(PropertyName = "validPassport")]
            public TripApplicationField ValidPassport { get; set; }

            [JsonProperty(PropertyName = "passportExpirationDate")]
            public TripApplicationField PassportExpirationDate { get; set; }

            [JsonProperty(PropertyName = "passportFirstName")]
            public TripApplicationField PassportFirstName { get; set; }

            [JsonProperty(PropertyName = "passportMiddleName")]
            public TripApplicationField PassportMiddleName { get; set; }

            [JsonProperty(PropertyName = "passportLastName")]
            public TripApplicationField PassportLastName { get; set; }

            [JsonProperty(PropertyName = "passportCountry")]
            public TripApplicationField PassportCountry { get; set; }

            [JsonProperty(PropertyName = "passportBirthday")]
            public TripApplicationField PassportBirthday { get; set; }

            [JsonProperty(PropertyName = "deltaFrequentFlyer")]
            public TripApplicationField DeltaFrequentFlyer { get; set; }

            [JsonProperty(PropertyName = "southAfricanFrequentFlyer")]
            public TripApplicationField SouthAfricanFrequentFlyer { get; set; }

            [JsonProperty(PropertyName = "unitedFrequentFlyer")]
            public TripApplicationField UnitedFrequentFlyer { get; set; }

            [JsonProperty(PropertyName = "usAirwaysFrequentFlyer")]
            public TripApplicationField UsAirwaysFrequentFlyer { get; set; }

            [JsonProperty(PropertyName = "internationalTravelExpericence")]
            public TripApplicationField InternationalTravelExpericence { get; set; }

            [JsonProperty(PropertyName = "experienceAbroad")]
            public TripApplicationField ExperienceAbroad { get; set; }

            [JsonProperty(PropertyName = "describeExperienceAbroad")]
            public TripApplicationField DescribeExperienceAbroad { get; set; }

            [JsonProperty(PropertyName = "pastAbuseHistory")]
            public TripApplicationField PastAbuseHistory { get; set; }
        }

        public class TripApplicationField
        {
            [JsonProperty(PropertyName = "attributeId")]
            public int AttributeId { get; set; }

            [JsonProperty(PropertyName = "formFieldId")]
            public int FormFieldId { get; set; }

            [JsonProperty(PropertyName = "value")]
            public string Value { get; set; }
        }
    }
}