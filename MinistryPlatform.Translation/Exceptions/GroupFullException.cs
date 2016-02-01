using System;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Exceptions
{
    public class GroupFullException : Exception
    {
        private Group group;
        public Group GroupDetails { get { return (group); } }
        public GroupFullException(Group group)
            : base("Group is full: " + group.Participants.Count + " > " + group.TargetSize)
        {
            this.group = group;
        }
    }
}
