using System;
using System.Collections.Generic;

namespace MinistryPlatform.Models
{
    public class Group
    {
        private IList<int> participants = new List<int>();

        public int GroupId { get; set; }
        //public string GroupName { get; set; }
        public string GroupRole { get; set; }

        public int RecordId { get; set; }
        public int TargetSize { get; set; }
        public string Name { get; set; }
        public IList<int> Participants { get { return (participants); } }
        public Boolean Full { get; set; }
    }
}
