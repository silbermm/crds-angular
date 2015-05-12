using System;

namespace crds_angular.Models.Crossroads
{
    public class CreateDonationDTO
    {
        public string program_id { get; set; }
        public int amount { get; set; }
        public int donor_id { get; set; }
    }
}