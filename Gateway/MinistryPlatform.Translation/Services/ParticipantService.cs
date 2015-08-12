using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class ParticipantService : BaseService, IParticipantService
    {
        private IMinistryPlatformService _ministryPlatformService;

        public ParticipantService(IMinistryPlatformService ministryPlatformService, IAuthenticationService authenticationService , IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
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
                        apiToken =>
                            (_ministryPlatformService.GetPageViewRecords("ParticipantByContactId", apiToken, searchStr,
                                "")));
                var record = records.Single();
                participant = new Participant
                {
                    ParticipantId = record.ToInt("dp_RecordID"),
                    EmailAddress = record.ToString("Email Address"),
                    PreferredName = record.ToString("Nickname"), 
                    DisplayName =  record.ToString("Display Name")
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("GetParticipant failed.  Contact Id: {0}", contactId), ex);
            }


            return participant;
        }

        public List<Response> GetParticipantResponses(int participantId)
        {
            try
            {
                var records =
                    WithApiLogin<List<Dictionary<string, object>>>(
                        apiToken =>
                            (_ministryPlatformService.GetSubpageViewRecords("ParticipantResponsesWithEventId",
                                participantId, apiToken, "", "")));
                return records.Select(viewRecord => new Response
                {
                    Opportunity_ID = viewRecord.ToInt("Opportunity ID"),
                    Participant_ID = viewRecord.ToInt("Participant ID"),
                    Response_Result_ID = viewRecord.ToInt("Response Result ID"),
                    Event_ID = viewRecord.ToInt("Event ID")
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("GetParticipantResponses failed.  Participant Id: {0}", participantId), ex);
            }
        }
    }
}