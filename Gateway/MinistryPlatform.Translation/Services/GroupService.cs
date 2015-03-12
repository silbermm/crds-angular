using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinistryPlatform.Translation.Services
{
    public class GroupService : IGroupService
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void addContactToGroup(String groupId, String contactId, String groupRoleId, DateTime startDate, DateTime? endDate, Boolean? employeeIndicator)
        {
            logger.Debug("Adding contact " + contactId + " to group " + groupId);
        }

        public void addContactToGroup(String groupId, String contactId)
        {
            logger.Debug("Adding contact " + contactId + " to group " + groupId);
        }
    }
}
