using System;

namespace MinistryPlatform.Models
{
    public class TripDistribution
    {
        public int ContactId { get; set; }
        public int EventTypeId { get; set; }
        public int EventId { get; set; }
        public string EventTitle { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public int TotalPledge { get; set; }
        public DateTime CampaignStartDate { get; set; }
        public DateTime CampaignEndDate { get; set; }
        public string DonorNickname { get; set; }
        public string DonorFirstName { get; set; }
        public string DonorLastName { get; set; }
        public string DonorEmail { get; set; } 
        public DateTime DonationDate { get; set; }
        public int DonationAmount { get; set; }
        public bool AnonymousGift { get; set; }
        public bool RegisteredDonor { get; set; }
        public bool MessageSent { get; set; }
    }
}