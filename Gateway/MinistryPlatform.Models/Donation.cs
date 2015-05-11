﻿using System;

namespace MinistryPlatform.Models
{
    public class Donation
    {
        public int donationId { get; set; }
        public int donorId { get; set; }
        public int donationAmt { get; set; }
        public DateTime donationDate { get; set; }
        public int paymentTypeId { get; set; }
    }
}