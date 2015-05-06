using System;

namespace crds_angular.test.controllers
{
    public class CreateDonationDTO
    {
        public int program_id { get; set; }
        public int amount { get; set; }
        public int donor_id { get; set; }
    }
}