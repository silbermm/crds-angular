﻿using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class TripGift
    {
        [JsonProperty(PropertyName = "donationDistributionId")]
        public int DonationDistributionId { get; set; }

        [JsonProperty(PropertyName = "donorId")]
        public int DonorId { get; set; }

        [JsonProperty(PropertyName = "donorNickname")]
        public string DonorNickname { get; set; }
        
        [JsonProperty(PropertyName = "donorLastName")]
        public string DonorLastName { get; set; }

        [JsonProperty(PropertyName = "donorEmail")]
        public string DonorEmail { get; set; }

        [JsonProperty(PropertyName = "donationAmount")]
        public int DonationAmount { get; set; }

        [JsonProperty(PropertyName = "donationDate")]
        public string DonationDate { get; set; }

        [JsonProperty(PropertyName = "registeredDonor")]
        public bool RegisteredDonor { get; set; }

        [JsonProperty(PropertyName = "anonymous")]
        public bool Anonymous { get; set; }

        [JsonProperty(PropertyName = "messageSent")]
        public bool MessageSent { get; set; }

        [JsonProperty(PropertyName = "paymentTypeId")]
        public int PaymentTypeId { get; set; }
    }
}