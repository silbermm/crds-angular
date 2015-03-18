using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinistryPlatform.Translation.Models
{
    public class Group
    {
        private int recordId;
        private int targetSize;
        private string name;
        private IList<int> participants = new List<int>();
        private Boolean full;
        private Boolean waitList;
        private IList<int> waitListGroupId = new List<int>();

        public int RecordId
        {
            get { return (recordId); }
            set { recordId = value; }
        }

        public int TargetSize
        {
            get { return (targetSize); }
            set { targetSize = value; }
        }

        public string Name
        {
            get { return (name); }
            set { name = value; }
        }

        public IList<int> Participants
        {
            get { return (participants); }
        }

        public Boolean Full
        {
            get { return (full); }
            set { full = value; }
        }

        public Boolean WaitList
        {
            get { return (waitList); }
            set { waitList = value; }
        }

        public IList<int> WaitListGroupId
        {
            get { return (waitListGroupId); }
        }
    }
}
