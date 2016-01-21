using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Events;
using Crossroads.Utilities.Functions;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Services;
using log4net;
using MinistryPlatform.Translation.Models.EventReservations;
using MinistryPlatform.Translation.Models.People;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using WebGrease.Css.Extensions;
using Event = MinistryPlatform.Models.Event;
using IEventService = crds_angular.Services.Interfaces.IEventService;
using IGroupService = MinistryPlatform.Translation.Services.Interfaces.IGroupService;
using TranslationEventService = MinistryPlatform.Translation.Services.Interfaces.IEventService;

namespace crds_angular.Services
{
    public class EventService : MinistryPlatformBaseService, IEventService
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(EventService));

        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly TranslationEventService _eventService;
        private readonly IGroupService _groupService;
        private readonly ICommunicationService _communicationService;
        private readonly IContactService _contactService;
        private readonly IContentBlockService _contentBlockService;
        private readonly IApiUserService _apiUserService;
        private readonly IContactRelationshipService _contactRelationshipService;
        private readonly IGroupParticipantService _groupParticipantService;
        private readonly IParticipantService _participantService;
        private readonly IRoomService _roomService;
        private readonly IEquipmentService _equipmentService;

        private readonly List<string> TABLE_HEADERS = new List<string>()
        {
            "Event Date",
            "Registered User",
            "Start Time",
            "End Time",
            "Location"
        };


        public EventService(TranslationEventService eventService,
                            IGroupService groupService,
                            ICommunicationService communicationService,
                            IContactService contactService,
                            IContentBlockService contentBlockService,
                            IConfigurationWrapper configurationWrapper,
                            IApiUserService apiUserService,
                            IContactRelationshipService contactRelationshipService,
                            IGroupParticipantService groupParticipantService,
                            IParticipantService participantService,
                            IRoomService roomService,
                            IEquipmentService equipmentService)
        {
            _eventService = eventService;
            _groupService = groupService;
            _communicationService = communicationService;
            _contactService = contactService;
            _contentBlockService = contentBlockService;
            _configurationWrapper = configurationWrapper;
            _apiUserService = apiUserService;
            _contactRelationshipService = contactRelationshipService;
            _groupParticipantService = groupParticipantService;
            _participantService = participantService;
            _roomService = roomService;
            _equipmentService = equipmentService;
        }

        public EventToolDto GetEventReservation(int eventId)
        {
            try
            {
                var dto = new EventToolDto();

                var e = this.GetEvent(eventId);
                dto.Title = e.EventTitle;
                dto.CongregationId = e.CongregationId;
                dto.EndDateTime = e.EventEndDate;
                dto.StartDateTime = e.EventStartDate;

                var rooms = _roomService.GetRoomReservations(eventId);
                var roomDto = new List<EventRoomDto>();

                foreach (var room in rooms){
                
                    var equipmentDto = new List<EventRoomEquipmentDto>();
                    var equipment = _equipmentService.GetEquipmentReservations(eventId, room.RoomId);
                    foreach (var equipmentReservation in equipment)
                    {
                        var eq = new EventRoomEquipmentDto();
                        eq.Cancelled = equipmentReservation.Cancelled;
                        eq.EquipmentId = equipmentReservation.EquipmentId;
                        eq.QuantityRequested = equipmentReservation.QuantityRequested;
                        eq.EquipmentReservationId = equipmentReservation.EventEquipmentId;
                        equipmentDto.Add(eq);
                    }

                    var r = new EventRoomDto();
                    r.Cancelled = room.Cancelled;
                    r.Equipment = equipmentDto;
                    r.Hidden = room.Hidden;
                    r.LayoutId = room.RoomLayoutId;
                    r.Notes = room.Notes;
                    r.RoomId = room.RoomId;
                    r.RoomReservationId = room.EventRoomId;

                    roomDto.Add(r);
                }
                dto.Rooms = roomDto;

                return dto;
            }
            catch (Exception ex)
            {
                var msg = "Event Service: CreateEventReservation";
                _logger.Error(msg, ex);
                throw new Exception(msg, ex);   
            }
        }

        public bool UpdateEventReservation(EventToolDto eventReservation, int eventId, string token)
        {
            try
            {
                foreach (var room in eventReservation.Rooms)
                {
                    if (room.RoomReservationId == 0)
                    {
                        AddRoom(eventId, room, token);
                    }
                    else
                    {
                        UpdateRoom(eventId, room);
                    }

                    foreach (var equipment in room.Equipment)
                    {
                        if (equipment.EquipmentReservationId == 0)
                        {
                            AddEquipment(equipment, eventId, room, token);
                        }
                        else
                        {
                            UpdateEquipment(equipment, eventId, room);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = "Event Service: CreateEventReservation";
                _logger.Error(msg, ex);
                throw new Exception(msg, ex);
            }
            return true;
        }

        public bool CreateEventReservation(EventToolDto eventTool, string token)
        {
            try
            {
                var eventId = AddEvent(eventTool, token);

                foreach (var room in eventTool.Rooms)
                {
                    AddRoom(eventId, room, token);

                    foreach (var equipment in room.Equipment)
                    {
                        AddEquipment(equipment, eventId, room, token);
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = "Event Service: CreateEventReservation";
                _logger.Error(msg, ex);
                throw new Exception(msg, ex);
            }
            return true;
        }

        private void AddEquipment(EventRoomEquipmentDto equipment, int eventId, EventRoomDto room, string token)
        {
            var equipmentReservation = new EquipmentReservationDto();
            equipmentReservation.Cancelled = false;
            equipmentReservation.EquipmentId = equipment.EquipmentId;
            equipmentReservation.EventId = eventId;
            equipmentReservation.QuantityRequested = equipment.QuantityRequested;
            equipmentReservation.RoomId = room.RoomId;
            var equipmentReservationId = _equipmentService.CreateEquipmentReservation(equipmentReservation, token);
        }

        private void UpdateEquipment(EventRoomEquipmentDto equipment, int eventId, EventRoomDto room)
        {
            var equipmentReservation = new EquipmentReservationDto();
            equipmentReservation.Cancelled = equipment.Cancelled;
            equipmentReservation.EquipmentId = equipment.EquipmentId;
            equipmentReservation.EventEquipmentId = equipment.EquipmentReservationId;
            equipmentReservation.EventId = eventId;
            equipmentReservation.QuantityRequested = equipment.QuantityRequested;
            equipmentReservation.RoomId = room.RoomId;
            _equipmentService.UpdateEquipmentReservation(equipmentReservation);
        }

        private void AddRoom(int eventId, EventRoomDto room, string token)
        {
            var roomReservation = new RoomReservationDto();
            roomReservation.Cancelled = false;
            roomReservation.EventId = eventId;
            roomReservation.Hidden = room.Hidden;
            roomReservation.Notes = room.Notes;
            roomReservation.RoomId = room.RoomId;
            roomReservation.RoomLayoutId = room.LayoutId;
            var roomReservationId = _roomService.CreateRoomReservation(roomReservation, token);
        }

        private void UpdateRoom(int eventId, EventRoomDto room)
        {
            var roomReservation = new RoomReservationDto();
            roomReservation.Cancelled = room.Cancelled;
            roomReservation.EventId = eventId;
            roomReservation.EventRoomId = room.RoomReservationId;
            roomReservation.Hidden = room.Hidden;
            roomReservation.Notes = room.Notes;
            roomReservation.RoomId = room.RoomId;
            roomReservation.RoomLayoutId = room.LayoutId;
            _roomService.UpdateRoomReservation(roomReservation);
        }

        private int AddEvent(EventToolDto eventTool, string token)
        {
            var eventDto = new EventReservationDto();
            eventDto.CongregationId = eventTool.CongregationId;
            eventDto.ContactId = eventTool.ContactId;
            eventDto.Description = eventTool.Description;
            eventDto.DonationBatchTool = eventTool.DonationBatchTool;
            eventDto.EndDateTime = eventTool.EndDateTime;
            eventDto.EventTypeId = eventTool.EventTypeId;
            eventDto.MeetingInstructions = eventTool.MeetingInstructions;
            eventDto.MinutesSetup = eventTool.MinutesSetup;
            eventDto.MinutesTeardown = eventTool.MinutesTeardown;
            eventDto.ProgramId = eventTool.ProgramId;
            if (eventTool.ReminderDaysId > 0)
            {
                eventDto.ReminderDaysId = eventTool.ReminderDaysId;
            }
            eventDto.SendReminder = eventTool.SendReminder;
            eventDto.StartDateTime = eventTool.StartDateTime;
            eventDto.Title = eventTool.Title;
            var eventId = _eventService.CreateEvent(eventDto, token);
            return eventId;
        }

        public Event GetEvent(int eventId)
        {
            return _eventService.GetEvent(eventId);
        }

        public void RegisterForEvent(EventRsvpDto eventDto, string token)
        {
            var defaultGroupRoleId = AppSetting("Group_Role_Default_ID");
            var today = DateTime.Today;
            try
            {
                var saved = eventDto.Participants.Select(participant =>
                {
                    var groupParticipantId = _groupParticipantService.Get(eventDto.GroupId, participant.ParticipantId);
                    if (groupParticipantId == 0)
                    {
                        groupParticipantId = _groupService.addParticipantToGroup(participant.ParticipantId,
                                                                                 eventDto.GroupId,
                                                                                 defaultGroupRoleId,
                                                                                 participant.ChildcareRequested,
                                                                                 today);
                    }

                    // validate that there is not a participant record before creating
                    var retVal =
                        Functions.IntegerReturnValue(
                            () =>
                                !_eventService.EventHasParticipant(eventDto.EventId, participant.ParticipantId)
                                    ? _eventService.RegisterParticipantForEvent(participant.ParticipantId, eventDto.EventId, eventDto.GroupId, groupParticipantId)
                                    : 1);

                    return new RegisterEventObj()
                    {
                        EventId = eventDto.EventId,
                        ParticipantId = participant.ParticipantId,
                        RegisterResult = retVal,
                        ChildcareRequested = participant.ChildcareRequested
                    };
                }).ToList();

                SendRsvpMessage(saved, token);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Unable to add event participant: " + e.Message);
            }
        }

        public IList<Models.Crossroads.Events.Event> EventsReadyForPrimaryContactReminder(string token)
        {
            var pageViewId = AppSetting("EventsReadyForPrimaryContactReminder");
            var search = "";
            var events = _eventService.EventsByPageViewId(token, pageViewId, search);
            var eventList = events.Select(evt => new Models.Crossroads.Events.Event()
            {
                name = evt.EventTitle,
                EventId = evt.EventId,
                EndDate = evt.EventEndDate,
                StartDate = evt.EventStartDate,
                EventType = evt.EventType,
                location = evt.Congregation,
                PrimaryContactEmailAddress = evt.PrimaryContact.EmailAddress,
                PrimaryContactId = evt.PrimaryContact.ContactId
            });
            
            return eventList.ToList();
        }

        public IList<Models.Crossroads.Events.Event> EventsReadyForReminder(string token)
        {
            var pageId = AppSetting("EventsReadyForReminder");
            var events = _eventService.EventsByPageId(token, pageId);
            var eventList = events.Select(evt => new Models.Crossroads.Events.Event()
            {
                name = evt.EventTitle,
                EventId = evt.EventId,
                EndDate = evt.EventEndDate,
                StartDate = evt.EventStartDate,
                EventType = evt.EventType,
                location = evt.Congregation,
                PrimaryContactEmailAddress = evt.PrimaryContact.EmailAddress,
                PrimaryContactId = evt.PrimaryContact.ContactId
            });
            // Childcare will be included in the email for event, so don't send a duplicate.
            return eventList.Where(evt => evt.EventType != "Childcare").ToList();
        }

        public IList<Participant> EventParticpants(int eventId, string token)
        {
            return _eventService.EventParticipants(token, eventId).ToList();
        }

        public void SendReminderEmails()
        {
            var token = _apiUserService.GetToken();
            var eventList = EventsReadyForReminder(token);

            eventList.ForEach(evt =>
            {
                // get the participants...
                var participants = EventParticpants(evt.EventId, token);

                // does the event have a childcare event?
                var childcare = GetChildcareEvent(evt.EventId);
                var childcareParticipants = childcare != null ? EventParticpants(childcare.EventId, token) : new List<Participant>();

                participants.ForEach(participant => SendEventReminderEmail(evt, participant, childcare, childcareParticipants, token));
                _eventService.SetReminderFlag(evt.EventId, token);
            });
        }

        public void SendPrimaryContactReminderEmails()
        {
            var token = _apiUserService.GetToken();
            var eventList = EventsReadyForPrimaryContactReminder(token);

            eventList.ForEach(evt =>
            {
                SendPrimaryContactReminderEmail(evt, token);

            });
        }

        private void SendEventReminderEmail(Models.Crossroads.Events.Event evt, Participant participant, Event childcareEvent, IList<Participant> children, string token)
        {
            var mergeData = new Dictionary<string, object>
            {
                {"Nickname", participant.Nickname},
                {"Event_Title", evt.name},
                {"Event_Start_Date", evt.StartDate.ToShortDateString()},
                {"Event_Start_Time", evt.StartDate.ToShortTimeString()},
                {"cmsChildcareEventReminder", string.Empty},
                {"Childcare_Children", string.Empty},
                {"Childcare_Contact", string.Empty} // Set these three parameters no matter what...
            };

            if (children.Any())
            {
                // determine if any of the children are related to the participant
                var mine = MyChildrenParticipants(participant.ContactId, children, token);
                // build the HTML for the [Childcare] data
                if (mine.Any())
                {
                    mergeData.Add("cmsChildcareEventReminder", _contentBlockService["cmsChildcareEventReminder"].Content);
                    var childcareString = ChildcareData(mine);
                    mergeData.Add("Childcare_Children", childcareString);
                    mergeData.Add("Childcare_Contact", new HtmlElement("span", "If you need to cancel, please email " + childcareEvent.PrimaryContact.EmailAddress).Build());
                }
            }
            var defaultContact = _contactService.GetContactById(AppSetting("DefaultContactEmailId"));
            var comm = _communicationService.GetTemplateAsCommunication(
                AppSetting("EventReminderTemplateId"),
                defaultContact.Contact_ID,
                defaultContact.Email_Address,
                evt.PrimaryContactId,
                evt.PrimaryContactEmailAddress,
                participant.ContactId,
                participant.EmailAddress,
                mergeData);
            _communicationService.SendMessage(comm);
        }

        private void SendPrimaryContactReminderEmail(Models.Crossroads.Events.Event evt, string token)
        {
            var mergeData = new Dictionary<string, object>
            {
                {"Event_ID", evt.EventId},
                {"Event_Title", evt.name},
                {"Event_Start_Date", evt.StartDate.ToShortDateString()},
                {"Event_Start_Time", evt.StartDate.ToShortTimeString()},
                {"Base_Url", _configurationWrapper.GetConfigValue("BaseMPUrl")}
              
            };
           
            var defaultContact = _contactService.GetContactById(AppSetting("DefaultContactEmailId"));
            var comm = _communicationService.GetTemplateAsCommunication(
               AppSetting("EventPrimaryContactReminderTemplateId"),
               defaultContact.Contact_ID,
               defaultContact.Email_Address,
               evt.PrimaryContactId,
               evt.PrimaryContactEmailAddress,
               evt.PrimaryContactId,
               evt.PrimaryContactEmailAddress,               
               mergeData);
            _communicationService.SendMessage(comm);
        }

        public List<Participant> MyChildrenParticipants(int contactId, IList<Participant> children, string token)
        {
            var relationships = _contactRelationshipService.GetMyCurrentRelationships(contactId, token);
            var mine = children.Where(child => relationships.Any(rel => rel.Contact_Id == child.ContactId)).ToList();
            return mine;
        }

        private String ChildcareData(IList<Participant> children)
        {
            var el = new HtmlElement("span",
                                     new Dictionary<string, string>(),
                                     "You have indicated that you need childcare for the following children:")
                .Append(new HtmlElement("ul").Append(children.Select(child => new HtmlElement("li", child.DisplayName)).ToList()));
            return el.Build();
        }

        private void SendRsvpMessage(List<RegisterEventObj> saved, string token)
        {
            var evnt = _eventService.GetEvent(saved.First().EventId);
            var childcareRequested = saved.Any(s => s.ChildcareRequested);
            var loggedIn = _contactService.GetMyProfile(token);

            var childcareHref = new HtmlElement("a",
                                                new Dictionary<string, string>()
                                                {
                                                    {
                                                        "href",
                                                        string.Format("https://{0}/childcare/{1}", _configurationWrapper.GetConfigValue("BaseUrl"), evnt.EventId)
                                                    }
                                                },
                                                "this link").Build();
            var childcare = _contentBlockService["eventRsvpChildcare"].Content.Replace("[url]", childcareHref);

            var mergeData = new Dictionary<string, object>
            {
                {"Event_Name", evnt.EventTitle},
                {"HTML_Table", SetupTable(saved, evnt).Build()},
                {"Childcare", (childcareRequested) ? childcare : ""}
            };
            var defaultContact = _contactService.GetContactById(AppSetting("DefaultContactEmailId"));
            var comm = _communicationService.GetTemplateAsCommunication(
                AppSetting("OneTimeEventRsvpTemplate"),
                defaultContact.Contact_ID,
                defaultContact.Email_Address,
                evnt.PrimaryContact.ContactId,
                evnt.PrimaryContact.EmailAddress,
                loggedIn.Contact_ID,
                loggedIn.Email_Address,
                mergeData
                );

            _communicationService.SendMessage(comm);
        }

        private HtmlElement SetupTable(List<RegisterEventObj> regData, Event evnt)
        {
            var tableAttrs = new Dictionary<string, string>()
            {
                {"width", "100%"},
                {"border", "1"},
                {"cellspacing", "0"},
                {"cellpadding", "5"}
            };

            var cellAttrs = new Dictionary<string, string>()
            {
                {"align", "center"}
            };

            var htmlrows = regData.Select(rsvp =>
            {
                var p = _contactService.GetContactByParticipantId(rsvp.ParticipantId);
                return new HtmlElement("tr")
                    .Append(new HtmlElement("td", cellAttrs, evnt.EventStartDate.ToShortDateString()))
                    .Append(new HtmlElement("td", cellAttrs, p.First_Name + " " + p.Last_Name))
                    .Append(new HtmlElement("td", cellAttrs, evnt.EventStartDate.ToShortTimeString()))
                    .Append(new HtmlElement("td", cellAttrs, evnt.EventEndDate.ToShortTimeString()))
                    .Append(new HtmlElement("td", cellAttrs, evnt.Congregation));
            }).ToList();

            return new HtmlElement("table", tableAttrs)
                .Append(SetupTableHeader)
                .Append(htmlrows);
        }

        private HtmlElement SetupTableHeader()
        {
            var headers = TABLE_HEADERS.Select(el => new HtmlElement("th", el)).ToList();
            return new HtmlElement("tr", headers);
        }

        private class RegisterEventObj
        {
            public int RegisterResult { get; set; }
            public int ParticipantId { get; set; }
            public int EventId { get; set; }
            public bool ChildcareRequested { get; set; }
        }

        public Event GetMyChildcareEvent(int parentEventId, string token)
        {
            var participantRecord = _participantService.GetParticipantRecord(token);
            if (!_eventService.EventHasParticipant(parentEventId, participantRecord.ParticipantId))
            {
                return null;
            }
            // token user is part of parent event, retrieve childcare event
            var childcareEvent = GetChildcareEvent(parentEventId);
            return childcareEvent;
        }

        public Event GetChildcareEvent(int parentEventId)
        {
            var childEvents = _eventService.GetEventsByParentEventId(parentEventId);
            var childcareEvents = childEvents.Where(childEvent => childEvent.EventType == "Childcare").ToList();

            if (childcareEvents.Count == 0)
            {
                return null;
            }
            if (childcareEvents.Count > 1)
            {
                throw new ApplicationException(string.Format("Mulitple Childcare Events Exist, parent event id: {0}", parentEventId));
            }
            return childcareEvents.First();
        }
    }
}