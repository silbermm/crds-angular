using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class ParticipantService: BaseService , IParticipantService
    {
        private IMinistryPlatformService _ministryPlatformService;

        public ParticipantService(IMinistryPlatformService ministryPlatformService)
        {
            this._ministryPlatformService = ministryPlatformService;
        }

        public Participant GetParticipant(int contactId)
        {
            Participant participant;
            //var records = new List<Dictionary<string, object>>();
            try
            {
                var searchStr = contactId.ToString() + ",";
                var records =
                    WithApiLogin<List<Dictionary<string, object>>>(
                        apiToken => (_ministryPlatformService.GetPageViewRecords("ParticipantByContactId", apiToken, searchStr, "")));
                var record = records.Single();
                 participant = new Participant
                {
                    ParticipantId = record.ToInt("dp_RecordID"),
                    EmailAddress = record.ToString("Email Address"),
                    PreferredName = record.ToString("Nickname")
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("GetParticipant failed.  Contact Id: {0}",contactId), ex);
            }
            

            return participant;
        }
    }
}
