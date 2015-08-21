﻿using System;

namespace MinistryPlatform.Models
{
    public class PledgeCampaign
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Goal { get; set; }
        public int FormId { get; set; }
        public string FormTitle { get; set; }
        public int YoungestAgeAllowed { get; set; }
    }
}