using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services
{
    public interface IGroupService
    {
        int addParticipantToGroup(int participantId, int groupId, int groupRoleId, DateTime startDate,
            DateTime? endDate = null, Boolean? employeeRole = false);

        IList<Event> getAllEventsForGroup(int groupId);

        Group getGroupDetails(int groupId);
    }
}
