using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace crds_angular.Models.Crossroads.Stewardship
{
    /// <summary>
    /// The data required to setup a recurring gift in MinistryPlatform and Stripe.
    /// </summary>
    public class RecurringGiftDto
    {
        /// <summary>
        /// The Stripe token representing a card or bank account, to be used for this recurring gift. 
        /// </summary>
        [JsonProperty(PropertyName = "stripe_token_id")]
        [Required]
        public string StripeTokenId { get; set; }
        /// <summary>
        /// This is a decimal dollar amount of the recurring gift.
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        [Required]
        public decimal PlanAmount { get; set; }
        /// <summary>
        /// The ID of the program this recurring gift will give to.
        /// </summary>
        [JsonProperty(PropertyName = "program")]
        [Required]
        public string Program { get; set; }
        /// <summary>
        /// Either "week" or "month", indicating the recurrence interval.
        /// </summary>
        [JsonProperty(PropertyName = "interval"), JsonConverter(typeof(StringEnumConverter))]
        [Required]
        public PlanInterval PlanInterval { get; set; }
        /// <summary>
        /// The date on which the recurring gift should begin.
        /// </summary>
        [JsonProperty(PropertyName = "start_date")]
        [Required]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// The date on which the recurring gift should end.
        /// </summary>
        [JsonProperty(PropertyName = "end_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// The email address of the donor.  This is not required or used on input, but will be returned on output.
        /// </summary>
        [JsonProperty(PropertyName = "email")]
        public string EmailAddress { get; set; }
        /// <summary>
        /// The id of the recurring gift created in MinistryPlatform.  This is not required or used on input, but will be returned on output.
        /// </summary>
        [JsonProperty(PropertyName = "recurring_gift_id")]
        public int RecurringGiftId { get; set; }
        /// <summary>
        /// The id of the donor of the recurring gift created in MinistryPlatform.  This is not required or used on input, but will be returned on output.
        /// </summary>
        [JsonProperty(PropertyName = "donor_id")]
        public int DonorID { get; set; }
        /// <summary>
        /// The recurrence of the recurring gift created in MinistryPlatform.  This is not required or used on input, but will be returned on output.
        /// </summary>
        [JsonProperty(PropertyName = "recurrence")]
        public string Recurrence { get; set; }
        /// <summary>
        /// The Congregation Name of the recurring gift created in MinistryPlatform.  This is not required or used on input, but will be returned on output.
        /// </summary>
        [JsonProperty(PropertyName = "congregation_name")]
        public string CongregationName { get; set; }
        /// <summary>
        /// The Subscription ID of the recurring gift created in Strip.  This is not required or used on input, but will be returned on output.
        /// </summary>
        [JsonProperty(PropertyName = "subscription_id")]
        public string SubscriptionID { get; set; }
        /// <summary>
        /// The Subscription ID of the recurring gift created in Strip.  This is not required or used on input, but will be returned on output.
        /// </summary>
        [JsonProperty(PropertyName = "consecutive_failure_count")]
        public string FailCount { get; set; }
        /// <summary>
        /// The Source of the recurring gift created in MinistryPlatform (Credit Card/Last 4/Visa).  This is not required or used on input, but will be returned on output.
        /// </summary>
        [JsonProperty(PropertyName = "source", NullValueHandling = NullValueHandling.Ignore)]
        public DonationSourceDTO Source { get; set; }
        /// <summary>
        /// The Name of the program this recurring gift will give to.
        /// </summary>
        [JsonProperty(PropertyName = "program_name")]
        public string ProgramName { get; set; }
    }

    [DataContract]
    public enum PlanInterval
    {
        [EnumMember(Value = "week")]
        Weekly,
        [EnumMember(Value = "month")]
        Monthly
    }
}