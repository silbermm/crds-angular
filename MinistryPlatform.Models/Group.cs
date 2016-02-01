using System;
using System.Collections.Generic;

namespace MinistryPlatform.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string GroupRole { get; set; }
        public int GroupType { get; set; }
        public int TargetSize { get; set; }
        public string Name { get; set; }
        public IList<GroupParticipant> Participants { get; set; }
        public Boolean Full { get; set; }
        public Boolean WaitList { get; set; }
        public int WaitListGroupId { get; set; }
        public string PrimaryContact { get; set; }
        public int EventTypeId { get; set; }
        public bool ChildCareAvailable { get; set; }
        public string Congregation { get; set; }
        public int MinimumAge { get; set; }

        public Group()
        {
            Participants = new List<GroupParticipant>();
            ChildCareAvailable = false;
        }
    }
}
