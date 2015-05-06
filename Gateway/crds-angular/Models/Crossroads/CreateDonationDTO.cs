using System;

namespace crds_angular.test.controllers
{
    public class CreateDonationDTO
    {
        public int programId { get; set; }
        public int donationAmt { get; set; }
        public int donorId { get; set; }
    }
}