using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class EventParticipantService : BaseService, IEventParticipantService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;

        public EventParticipantService(IMinistryPlatformService ministryPlatformService, IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public bool AddDocumentsToTripParticipant(List<TripDocuments> documents, int eventParticipantId)
        {
            try
            {
                var token = ApiLogin();
                foreach (var d in documents)
                {
                    var values = new Dictionary<string, object>
                    {
                        {"Document_ID", d.DocumentId},
                        {"Received", false}
                    };
                    _ministryPlatformService.CreateSubRecord("EventParticipantDocuments", eventParticipantId, values, token, true);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("AddDocumentsToTripParticipant failed.  Event Participant: {0}", eventParticipantId),
                    ex);
            }
        }

        public List<TripParticipant> TripParticipants(string search)
        {
            try
            {
                var records =
                    WithApiLogin(
                        apiToken =>
                            (_ministryPlatformService.GetPageViewRecords("GoTripParticipants",
                                                                         apiToken,
                                                                         search)));
                return records.Select(viewRecord => new TripParticipant
                {
                    EventParticipantId = viewRecord.ToInt("Event_Participant_ID"),
                    EventId = viewRecord.ToInt("Event_ID"),
                    EventTitle = viewRecord.ToString("Event_Title"),
                    Nickname = viewRecord.ToString("Nickname"),
                    Lastname = viewRecord.ToString("Last_Name"),
                    EmailAddress = viewRecord.ToString("Email_Address"),
                    EventStartDate = viewRecord.ToDate("Event_Start_Date"),
                    EventEndDate = viewRecord.ToDate("Event_End_Date"),
                    EventType = viewRecord.ToString("Event_Type"),
                    ParticipantId = viewRecord.ToInt("Participant_ID"),
                    ProgramId = viewRecord.ToInt("Program_ID"),
                    ProgramName = viewRecord.ToString("Program_Name"),
                    CampaignId = viewRecord.ToInt("Campaign_ID"),
                    CampaignName = viewRecord.ToString("Campaign_Name"),
                    DonorId = viewRecord.ToInt("Donor_ID"),
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("TripParticipants failed.  search: {0}", search),
                    ex);
            }
        }
    }
}