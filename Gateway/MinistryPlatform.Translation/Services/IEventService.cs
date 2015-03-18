using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services
{
    public interface IEventService
    {
        int registerParticipantForEvent(int participantId, int eventId);
    }
}
