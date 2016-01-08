using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Exceptions;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;
using Newtonsoft.Json.Bson;

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

        //Get Participant IDs of a contact
        public Participant GetParticipantRecord(string token)
        {
            var results = _ministryPlatformService.GetRecordsDict("MyParticipantRecords", token);
            Dictionary<string, object> result = null;
            try
            {
                result = results.SingleOrDefault();
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "Sequence contains more than one element")
                {
                    throw new MultipleRecordsException("Multiple Participant records found! Only one participant allowed per Contact.");
                }
            }

            if (result == null)
            {
                return null;
            }
            var participant = new Participant
            {
                ContactId = result.ToInt("Contact_ID"),
                ParticipantId = result.ToInt("dp_RecordID"),
                EmailAddress = result.ToString("Email_Address"),
                PreferredName = result.ToString("Nickname"),
                DisplayName = result.ToString("Display_Name")
            };

            return participant;
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
                    ContactId = record.ToInt("Contact ID"),
                    ParticipantId = record.ToInt("dp_RecordID"),
                    EmailAddress = record.ToString("Email Address"),
                    PreferredName = record.ToString("Nickname"), 
                    DisplayName =  record.ToString("Display Name"), 
                    Age = record.ToInt("Age")
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("GetParticipant failed.  Contact Id: {0}", contactId), ex);
            }


            return participant;
        }

        public void UpdateParticipant(Dictionary<string, object> participant)
        {
            var apiToken = ApiLogin();
            try
            {
                _ministryPlatformService.UpdateRecord(_configurationWrapper.GetConfigIntValue("Participants"), participant, apiToken);
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                   string.Format("Unable to update the participant.  Participant Id: {0}", participant["Participant_ID"]), e);
            }

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