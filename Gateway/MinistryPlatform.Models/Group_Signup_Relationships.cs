using System;

namespace MinistryPlatform.Models
{
    public class GroupSignupRelationships
    {
        public int RelationshipId { get; set; }
        public int? RelationshipMinAge { get; set; }
        public int? RelationshipMaxAge { get; set; }
    }
}