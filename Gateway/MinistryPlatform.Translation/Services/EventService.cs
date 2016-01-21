using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models.EventReservations;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class EventService : BaseService, IEventService
    {
        private readonly log4net.ILog _logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly int _eventParticipantSubPageId = Convert.ToInt32(AppSettings("EventsParticipants"));
        private readonly int _eventParticipantPageId = Convert.ToInt32(AppSettings("EventParticipant"));

        private readonly int _eventParticipantStatusDefaultId =
            Convert.ToInt32(AppSettings("Event_Participant_Status_Default_ID"));

        private readonly int _eventPageNeedReminders =
            Convert.ToInt32(AppSettings("EventsReadyForReminder"));

        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly IGroupService _groupService;

        public EventService(IMinistryPlatformService ministryPlatformService,
                            IAuthenticationService authenticationService,
                            IConfigurationWrapper configurationWrapper,
                            IGroupService groupService)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
            _groupService = groupService;
        }

        public int CreateEvent(EventReservationDto eventReservationReservation, string token)
        {
            var eventPageId = _configurationWrapper.GetConfigIntValue("Events");

            var eventDictionary = new Dictionary<string, object>
            {
                {"Congregation_ID", eventReservationReservation.CongregationId},
                {"Primary_Contact", eventReservationReservation.ContactId},
                {"Description", eventReservationReservation.Description},
                {"On_Donation_Batch_Tool", eventReservationReservation.DonationBatchTool},
                {"Event_End_Date", eventReservationReservation.EndDateTime},
                {"Event_Type_ID", eventReservationReservation.EventTypeId},
                {"Meeting_Instructions", eventReservationReservation.MeetingInstructions},
                {"Minutes_for_Setup", eventReservationReservation.MinutesSetup},
                {"Minutes_for_Cleanup", eventReservationReservation.MinutesTeardown},
                {"Program_ID", eventReservationReservation.ProgramId},
                {"Reminder_Days_Prior_ID", eventReservationReservation.ReminderDaysId},
                {"Send_Reminder", eventReservationReservation.SendReminder},
                {"Event_Start_Date", eventReservationReservation.StartDateTime},
                {"Event_Title", eventReservationReservation.Title},
                {"Visibility_Level_ID", _configurationWrapper.GetConfigIntValue("EventVisibilityLevel")}
            };

            try
            {
                return (_ministryPlatformService.CreateRecord(eventPageId, eventDictionary, token, true));
            }
            catch (Exception e)
            {
                var msg = string.Format("Error creating Event Reservation, eventReservationReservation: {0}", eventReservationReservation);
                _logger.Error(msg, e);
                throw (new ApplicationException(msg, e));
            }
        }

        public int SafeRegisterParticipant(int eventId, int participantId, int groupId = 0, int groupParticipantId = 0)
        {
            var eventParticipantId = GetEventParticipantRecordId(eventId, participantId);
            if (eventParticipantId != 0)
            {
                return eventParticipantId;
            }
            eventParticipantId = RegisterParticipantForEvent(participantId, eventId, groupId, groupParticipantId);
            return eventParticipantId;
        }

        public int RegisterParticipantForEvent(int participantId, int eventId, int groupId = 0, int groupParticipantId = 0)
        {
            _logger.Debug("Adding participant " + participantId + " to event " + eventId);
            var values = new Dictionary<string, object>
            {
                {"Participant_ID", participantId},
                {"Event_ID", eventId},
                {"Participation_Status_ID", _eventParticipantStatusDefaultId},
            };

            if (groupId != 0)
            {
                values.Add("Group_ID", groupId);
            }
            if (groupParticipantId != 0)
            {
                values.Add("Group_Participant_ID", groupParticipantId);
            }

            int eventParticipantId;
            try
            {
                eventParticipantId =
                    WithApiLogin<int>(
                        apiToken =>
                        {
                            return
                                (_ministryPlatformService.CreateSubRecord(_eventParticipantSubPageId,
                                                                          eventId,
                                                                          values,
                                                                          apiToken,
                                                                          true));
                        });
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("registerParticipantForEvent failed.  Participant Id: {0}, Event Id: {1}",
                                  participantId,
                                  eventId),
                    ex.InnerException);
            }

            _logger.Debug(string.Format("Added participant {0} to event {1}; record id: {2}",
                                        participantId,
                                        eventId,
                                        eventParticipantId));
            return (eventParticipantId);
        }

        public int UnregisterParticipantForEvent(int participantId, int eventId)
        {
            _logger.Debug("Removing participant " + participantId + " from event " + eventId);

            int eventParticipantId;
            try
            {
                // go get record id to delete
                var recordId = GetEventParticipantRecordId(eventId, participantId);
                eventParticipantId = _ministryPlatformService.DeleteRecord(_eventParticipantPageId, recordId, null, ApiLogin());
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("unRegisterParticipantForEvent failed.  Participant Id: {0}, Event Id: {1}",
                                  participantId,
                                  eventId),
                    ex.InnerException);
            }

            _logger.Debug(string.Format("Removed participant {0} from event {1}; record id: {2}",
                                        participantId,
                                        eventId,
                                        eventParticipantId));
            return (eventParticipantId);
        }

        public Event GetEvent(int eventId)
        {
            var token = ApiLogin();
            var searchString = string.Format("{0},", eventId);
            var r = _ministryPlatformService.GetPageViewRecords("EventsWithDetail", token, searchString);
            if (r.Count == 1)
            {
                var record = r[0];
                var e = new Event
                {
                    CongregationId = record.ToInt("Congregation_ID"),
                    EventEndDate = record.ToDate("Event_End_Date"),
                    EventId = record.ToInt("Event_ID"),
                    EventStartDate = record.ToDate("Event_Start_Date"),
                    EventTitle = record.ToString("Event_Title"),
                    ParentEventId = record.ToNullableInt("Parent_Event_ID"),
                    PrimaryContact = new Contact
                    {
                        ContactId = record.ToInt("Contact_ID"),
                        EmailAddress = record.ToString("Email_Address")
                    }
                };


                return e;
            }
            if (r.Count == 0)
            {
                return null;
            }
            throw new ApplicationException(string.Format("Duplicate Event ID detected: {0}", eventId));
        }

        public int GetEventParticipantRecordId(int eventId, int participantId)
        {
            var search = "," + eventId + "," + participantId;
            var participant = _ministryPlatformService.GetPageViewRecords("EventParticipantByEventIdAndParticipantId", ApiLogin(), search).FirstOrDefault();
            return participant == null ? 0 : participant.ToInt("Event_Participant_ID");
        }

        public bool EventHasParticipant(int eventId, int participantId)
        {
            var searchString = "," + eventId + "," + participantId;
            var records = _ministryPlatformService.GetPageViewRecords("EventParticipantByEventIdAndParticipantId", ApiLogin(), searchString);
            return records.Count != 0;
        }

        public List<Event> GetEvents(string eventType, string token)
        {
            //this is using the basic Events page, any concern there?
            var pageId = Convert.ToInt32(ConfigurationManager.AppSettings["Events"]);
            var search = ",," + eventType;
            var records = _ministryPlatformService.GetRecordsDict(pageId, token, search);

            return records.Select(record => new Event
            {
                EventTitle = (string) record["Event_Title"],
                EventType = (string) record["Event_Type"],
                EventStartDate = (DateTime) record["Event_Start_Date"],
                EventEndDate = (DateTime) record["Event_End_Date"],
                EventId = (int) record["dp_RecordID"]
            }).ToList();
        }

        public List<Event> GetEventsByTypeForRange(int eventTypeId, DateTime startDate, DateTime endDate, string token)
        {
            const string viewKey = "EventsWithEventTypeId";
            var search = ",," + eventTypeId;
            var eventRecords = _ministryPlatformService.GetPageViewRecords(viewKey, token, search);

            var events = eventRecords.Select(record => new Event
            {
                EventTitle = record.ToString("Event Title"),
                EventType = record.ToString("Event Type"),
                EventStartDate = record.ToDate("Event Start Date", true),
                EventEndDate = record.ToDate("Event End Date", true),
                EventId = record.ToInt("dp_RecordID")
            }).ToList();

            //now we have a list, filter by date range.
            var filteredEvents =
                events.Where(e => e.EventStartDate.Date >= startDate.Date && e.EventStartDate.Date <= endDate.Date)
                    .ToList();
            return filteredEvents;
        }

        public List<Event> GetEventsByParentEventId(int parentEventId)
        {
            var token = ApiLogin();
            var searchStr = string.Format(",,,{0}", parentEventId);
            var records = _ministryPlatformService.GetPageViewRecords("EventsByParentEventID", token, searchStr);

            var events = records.Select(record => new Event
            {
                EventTitle = record.ToString("Event_Title"),
                EventType = record.ToString("Event_Type"),
                EventStartDate = record.ToDate("Event_Start_Date", true),
                EventEndDate = record.ToDate("Event_End_Date", true),
                EventId = record.ToInt("Event_ID"),
                PrimaryContact = new Contact
                {
                    ContactId = record.ToInt("Contact_ID"),
                    EmailAddress = record.ToString("Email_Address")
                }
            }).ToList();

            return events;
        }

        public IEnumerable<Event> EventsByPageId(string token, int pageViewId)
        {
            return _ministryPlatformService.GetRecordsDict(pageViewId, token).Select(record => new Event()
            {
                EventId = (int) record["Event_ID"],
                EventTitle = (string) record["Event_Title"],
                EventStartDate = (DateTime) record["Event_Start_Date"],
                EventEndDate = (DateTime) record["Event_End_Date"],
                EventType = record.ToString("Event_Type"),
                PrimaryContact = new Contact()
                {
                    ContactId = record.ToInt("Primary_Contact_ID"),
                    EmailAddress = record.ToString("Primary_Contact_Email_Address")
                }
            }).ToList();
        }

        public IEnumerable<Event> EventsByPageViewId(string token, int pageViewId, string searchString)
        {
            return _ministryPlatformService.GetPageViewRecords(pageViewId, token, searchString).Select(record => new Event()
            {
                EventId = (int)record["Event_ID"],
                EventTitle = (string)record["Event_Title"],
                EventStartDate = (DateTime)record["Event_Start_Date"],
                EventEndDate = (DateTime)record["Event_End_Date"],
                EventType = record.ToString("Event_Type"),
                PrimaryContact = new Contact()
                {
                    ContactId = record.ToInt("Primary_Contact_ID"),
                    EmailAddress = record.ToString("Primary_Contact_Email_Address")
                }
            }).ToList();
        }

        public IEnumerable<MinistryPlatform.Translation.Models.People.Participant> EventParticipants(string token, int eventId)
        {
            return
                _ministryPlatformService.GetSubpageViewRecords("EventParticipantSubpageRegisteredView", eventId, token)
                    .Select(person => new MinistryPlatform.Translation.Models.People.Participant()
                    {
                        ParticipantId = person.ToInt("Participant_ID"),
                        ContactId = person.ToInt("Contact_ID"),
                        EmailAddress = person.ToString("Email_Address"),
                        DisplayName = person.ToString("Display_Name"),
                        Nickname = person.ToString("Nickname"),
                        GroupName = person.ToString("Group_Name")
                    });
        }

        public void SetReminderFlag(int eventId, string token)
        {
            var dict = new Dictionary<string, object>
            {
                {"Event_ID", eventId},
                {"Reminder_Sent", 1}
            };
            _ministryPlatformService.UpdateRecord(_eventPageNeedReminders, dict, token);
        }

        public List<Group> GetGroupsForEvent(int eventId)
        {
            return _groupService.GetGroupsForEvent(eventId);
        }
    }
}