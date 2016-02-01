﻿using System;

namespace MinistryPlatform.Models.DTO
{
    public class CreateDonationDistDto
    {
        public Decimal Amount { get; set; }
        public int DonorId { get; set; }
        public int CongregationId { get; set; }
        public string PaymentType { get; set; }
        public string ProgramId { get; set; }
        public string ProgramName { get; set; }
        public int? RecurringGiftId { get; set; }
        public int? DonorAccountId { get; set; }
        public int Frequency { get; set; }
        public string Recurrence { get; set; }
        public int? DayOfWeek { get; set; }
        public int? DayOfMonth { get; set; }
        public DateTime? StartDate { get; set; }
        public string SubscriptionId { get; set; }
        public int ConsecutiveFailureCount { get; set; }
        public string StripeCustomerId { get; set; }
        public string StripeAccountId { get; set; }
    }
}
