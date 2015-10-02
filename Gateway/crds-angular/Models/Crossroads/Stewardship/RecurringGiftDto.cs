using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

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
        [JsonProperty(PropertyName = "interval")]
        [Required]
        public string PlanInterval { get; set; }
        /// <summary>
        /// The date on which the recurring gift should begin.
        /// </summary>
        [JsonProperty(PropertyName = "start_date")]
        [Required]
        public DateTime StartDate { get; set; }
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
    }
}
