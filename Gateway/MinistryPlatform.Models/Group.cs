using System;
using System.Collections.Generic;

namespace MinistryPlatform.Models
{
    public class Group
    {
        private IList<int> participants = new List<int>();

        public int GroupId { get; set; }
        public string GroupRole { get; set; }
        public int GroupType { get; set; }
        public int TargetSize { get; set; }
        public string Name { get; set; }
        public IList<int> Participants { get { return (participants); } }
        public Boolean Full { get; set; }
        public Boolean WaitList { get; set; } 
        public int WaitListGroupId { get; set; } 
    }
}
