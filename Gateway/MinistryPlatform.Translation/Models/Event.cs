using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinistryPlatform.Translation.Models
{
    public class Event
    {
        private int eventId;
        private IList<int> participants = new List<int>();

        public int EventId
        {
            get { return (eventId); }
            set { eventId = value; }
        }

        public IList<int> Participants
        {
            get { return (participants); }
        }
    }
}
