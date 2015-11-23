using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using crds_angular.Models.Crossroads.Events;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Event = MinistryPlatform.Models.Event;
using IEventService = crds_angular.Services.Interfaces.IEventService;
using TranslationEventService = MinistryPlatform.Translation.Services.Interfaces.IEventService;

namespace crds_angular.Services
{
    public class EventService : MinistryPlatformBaseService, IEventService
    {

        private readonly TranslationEventService _eventService;
        private readonly ICommunicationService _communicationService;
        private readonly IContactService _contactService;
        

        private readonly List<string> TABLE_HEADERS = new List<string>()
        {
            "Event Date",
            "Registered User",
            "Start Time",
            "End Time",
            "Location"
        };

        public EventService(TranslationEventService eventService, ICommunicationService communicationService, IContactService contactService)
        {
            this._eventService = eventService;
            this._communicationService = communicationService;
            this._contactService = contactService;
        }

        public Event GetEvent(int eventId)
        {
            return _eventService.GetEvent(eventId);
        }

        public void RegisterForEvent(List<EventRsvpDTO> eventDto, String token)
        {
            try
            {
                var saved = eventDto.Select(dto =>
                {
                    var retVal = _eventService.RegisterParticipantForEvent(dto.ParticipantId, dto.EventId, dto.GroupId);
                    return new RegisterEventObj()
                    {
                        EventId = dto.EventId,
                        ParticipantId = dto.ParticipantId,
                        RegisterResult = retVal
                    };
                }).ToList();

                SendRsvpMessage(saved, token);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Unable to add event participant: " + e.Message);
            }
        }

        private void SendRsvpMessage(List<RegisterEventObj> saved, String token)
        {
            var evnt = _eventService.GetEvent(saved.First().EventId);
            var loggedIn = _contactService.GetMyProfile(token);
            var mergeData = new Dictionary<string, object>
            {
                {"Event_Name", evnt.EventTitle},
                {"HTML_Table", SetupTable(saved, evnt).Build()}
            };

            var comm = _communicationService.GetTemplateAsCommunication(
                AppSetting("OneTimeEventRsvpTemplate"),
                evnt.PrimaryContact.ContactId,
                evnt.PrimaryContact.EmailAddress,
                evnt.PrimaryContact.ContactId,
                evnt.PrimaryContact.EmailAddress,
                loggedIn.Contact_ID,
                loggedIn.Email_Address,
                mergeData
                );

            _communicationService.SendMessage(comm);
        }

        private HtmlElement SetupTable(List<RegisterEventObj> regData, Event evnt )
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
                    .Append(new HtmlElement("td", cellAttrs,  evnt.EventStartDate.ToShortDateString()))
                    .Append(new HtmlElement("td", cellAttrs, p.First_Name + " " + p.Last_Name))
                    .Append(new HtmlElement("td", cellAttrs, evnt.EventStartDate.ToShortTimeString()))
                    .Append(new HtmlElement("td", cellAttrs, evnt.EventEndDate.ToShortTimeString()))
                    .Append(new HtmlElement("td", cellAttrs, evnt.EventLocation));
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
        }
    }
}