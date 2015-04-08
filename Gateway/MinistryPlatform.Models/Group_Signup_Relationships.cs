using System;
using System.Data.SqlTypes;

namespace MinistryPlatform.Models
{
    public class GroupSignupRelationships
    {
        public int RelationshipId { get; set; }
        public Byte RelationshipMinAge { get; set; }
        public Byte RelationshipMaxAge { get; set; }
    }
}