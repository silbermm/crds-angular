using System;
using System.Collections.Generic;
using System.Linq;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class EventParticipantService : BaseService, IEventParticipantService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;

        public EventParticipantService(IMinistryPlatformService ministryPlatformService)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public List<TripParticipant> TripParticipants(string search)
        {
            try
            {
                var records =
                    WithApiLogin(
                        apiToken =>
                            (_ministryPlatformService.GetPageViewRecords("GoTripParticipants", apiToken,
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
                    EventType = viewRecord.ToString("Event_Type")
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("TripParticipants failed.  search: {0}", search), ex);
            }
        }
    }
}