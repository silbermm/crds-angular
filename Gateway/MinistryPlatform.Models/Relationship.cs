using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinistryPlatform.Models
{
    public class Relationship
    {
        public int RelationshipID { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public int RelatedContactID{ get; set; }
    }
}