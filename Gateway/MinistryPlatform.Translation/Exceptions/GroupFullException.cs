using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Translation.Models;

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
