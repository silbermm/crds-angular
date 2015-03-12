using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MinistryPlatform.Translation.Utils;

namespace MinistryPlatform.Translation.Services
{
    public class GroupService : BaseService, IGroupService
    {
        readonly private log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly int GroupPageId = Convert.ToInt32(AppSettings("Groups"));
        private readonly int GroupParticipantPageId = Convert.ToInt32(AppSettings("GroupsParticipants"));

        public int addUserToGroup(String userToken, String groupId, String groupRoleId, DateTime startDate, DateTime? endDate = null, Boolean? employeeRole = false)
        {
            var participant = AuthenticationService.GetParticipantRecord(userToken);
            var participantId = participant.ParticipantId;
            logger.Debug("Adding participant " + participantId + " to group " + groupId);
            var values = new Dictionary<string, object>
            {
                { "Group_ID", groupId },
                { "Participant_ID", participantId },
                { "Group_Role_ID", Convert.ToInt32(groupRoleId) },
                { "Start_Date", startDate },
                { "End_Date", endDate},
                { "Employee_Role", employeeRole }
            };
            logger.Debug("Data map for group participant: " + values);
            int groupParticipantId = MinistryPlatformService.CreateSubRecord(GroupParticipantPageId, Convert.ToInt32(groupId), values, userToken);

            logger.Debug("Added participant " + participantId + " to group " + groupId + ": record id: " + groupParticipantId);
            return (groupParticipantId);
        }
    }
}
