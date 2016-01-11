using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using crds_angular.Enum;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Opportunity;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Extensions;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Services;
using log4net;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;
using WebGrease.Css.Extensions;
using IGroupService = MinistryPlatform.Translation.Services.Interfaces.IGroupService;

namespace crds_angular.Services
{
    internal class MailRow
    {
        public string EventDate { get; set; }
        public string OpportunityName { get; set; }
        public string ShiftTime { get; set; }
        public string Location { get; set; }
    }

    public class ServeService : MinistryPlatformBaseService, IServeService
    {
        private readonly IContactService _contactService;
        private readonly IContactRelationshipService _contactRelationshipService;
        private readonly MinistryPlatform.Translation.Services.Interfaces.IEventService _eventService;
        private readonly IGroupParticipantService _groupParticipantService;
        private readonly IGroupService _groupService;
        private readonly IOpportunityService _opportunityService;
        private readonly IParticipantService _participantService;
        private readonly ICommunicationService _communicationService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IApiUserService _apiUserService;
        private readonly IResponseService _responseService;

        private readonly List<string> TABLE_HEADERS = new List<string>()
        {
            "Event Date",
            "Opportunity Name",
            "Shift Start and End",
            "Location"
        };

        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ServeService(IContactService contactService,
                            IContactRelationshipService contactRelationshipService,
                            IOpportunityService opportunityService,
                            MinistryPlatform.Translation.Services.Interfaces.IEventService eventService,
                            IParticipantService participantService,
                            IGroupParticipantService groupParticipantService,
                            IGroupService groupService,
                            ICommunicationService communicationService,
                            IAuthenticationService authenticationService,
                            IConfigurationWrapper configurationWrapper,
                            IApiUserService apiUserService,
                            IResponseService responseService)
        {
            _contactService = contactService;
            _contactRelationshipService = contactRelationshipService;
            _opportunityService = opportunityService;
            _eventService = eventService;
            _participantService = participantService;
            _groupParticipantService = groupParticipantService;
            _groupService = groupService;
            _communicationService = communicationService;
            _authenticationService = authenticationService;
            _configurationWrapper = configurationWrapper;
            _apiUserService = apiUserService;
            _responseService = responseService;
        }

        public List<FamilyMember> GetImmediateFamilyParticipants(string token)
        {
            var relationships = new List<FamilyMember>();
            var contactId = _authenticationService.GetContactId(token);
            var me = _participantService.GetParticipant(contactId);
            var myParticipant = new FamilyMember
            {
                ContactId = contactId,
                Email = me.EmailAddress,
                LoggedInUser = true,
                ParticipantId = me.ParticipantId,
                PreferredName = me.PreferredName,
                Age = me.Age
            };
            relationships.Add(myParticipant);

            // get family for contact Id
            var contactRelationships =
                _contactRelationshipService.GetMyImmediateFamilyRelationships(contactId, token).ToList();
            var family = contactRelationships.Select(contact => new FamilyMember
            {
                ContactId = contact.Contact_Id,
                Email = contact.Email_Address,
                LastName = contact.Last_Name,
                LoggedInUser = false,
                ParticipantId = contact.Participant_Id,
                PreferredName = contact.Preferred_Name,
                RelationshipId = contact.Relationship_Id,
                Age = contact.Age,
                HighSchoolGraduationYear = contact.HighSchoolGraduationYear
            }).ToList();

            relationships.AddRange(family);

            return relationships.OrderByDescending(s => s.Age).ToList();
        }

        public DateTime GetLastServingDate(int opportunityId, string token)
        {
            logger.Debug(string.Format("GetLastOpportunityDate({0}) ", opportunityId));
            return _opportunityService.GetLastOpportunityDate(opportunityId, token);
        }

        public List<QualifiedServerDto> GetQualifiedServers(int groupId, int opportunityId, string token)
        {
            var qualifiedServers = new List<QualifiedServerDto>();
            var immediateFamilyParticipants = GetImmediateFamilyParticipants(token);

            foreach (var participant in immediateFamilyParticipants)
            {
                var membership = _groupService.ParticipantQualifiedServerGroupMember(groupId, participant.ParticipantId);
                
                var opportunityResponse = _opportunityService.GetMyOpportunityResponses(participant.ContactId,
                                                                                        opportunityId);
                var qualifiedServer = new QualifiedServerDto();
                qualifiedServer.ContactId = participant.ContactId;
                qualifiedServer.Email = participant.Email;
                qualifiedServer.LastName = participant.LastName;
                qualifiedServer.LoggedInUser = participant.LoggedInUser;
                qualifiedServer.MemberOfGroup = membership;
                qualifiedServer.Pending = opportunityResponse != null;
                qualifiedServer.ParticipantId = participant.ParticipantId;
                qualifiedServer.PreferredName = participant.PreferredName;
                qualifiedServers.Add(qualifiedServer);
            }
            return qualifiedServers;
        }

        public List<ServingDay> GetServingDays(string token, int contactId, long from, long to)
        {
            var family = GetImmediateFamilyParticipants(token);
            var participants = family.OrderBy(f => f.ParticipantId).Select(f => f.ParticipantId).ToList();
            var servingParticipants = _groupParticipantService.GetServingParticipants(participants, from, to, contactId);
            var servingDays = new List<ServingDay>();
            var dayIndex = 0;

            foreach (var record in servingParticipants)
            {
                var eventDateOnly = record.EventStartDateTime.Date.ToString("d");
                var day = servingDays.SingleOrDefault(d => d.Day == eventDateOnly);
                if (day != null)
                {
                    //this day is already in list

                    var time =
                        day.ServeTimes.SingleOrDefault(t => t.Time == record.EventStartDateTime.TimeOfDay.ToString());
                    if (time != null)
                    {
                        //this time already in collection

                        var team = time.ServingTeams.SingleOrDefault(t => t.GroupId == record.GroupId);
                        if (team != null)
                        {
                            //team already in collection
                            if (!team.PastDeadline)
                            {
                                team.PastDeadline = (record.EventStartDateTime.AddDays(0 - record.OpportunitySignUpDeadline) < DateTime.Today);
                            }
                            var member = team.Members.SingleOrDefault(m => m.ContactId == record.ContactId);
                            if (member == null)
                            {
                                team.Members.Add(NewTeamMember(record));
                            }
                            else
                            {
                                member.Roles.Add(NewServingRole(record));
                                //if we have already found rsvp, skip
                                if (member.ServeRsvp == null)
                                {
                                    member.ServeRsvp = NewServeRsvp(record);
                                }
                            }
                        }
                        else
                        {
                            time.ServingTeams.Add(NewServingTeam(record));
                        }
                    }
                    else
                    {
                        day.ServeTimes.Add(NewServingTime(record));
                    }
                }
                else
                {
                    //new day for the list
                    var servingDay = new ServingDay();
                    dayIndex = dayIndex + 1;
                    servingDay.Index = dayIndex;
                    servingDay.Day = record.EventStartDateTime.Date.ToString("d");
                    servingDay.Date = record.EventStartDateTime;
                    servingDay.ServeTimes = new List<ServingTime> {NewServingTime(record)};

                    servingDays.Add(servingDay);
                }
            }

            return servingDays;
        }

        public Capacity OpportunityCapacity(int opportunityId, int eventId, int? minNeeded, int? maxNeeded)
        {               
            var opportunity = _opportunityService.GetOpportunityResponses(opportunityId, _apiUserService.GetToken());
            var min = minNeeded;
            var max = maxNeeded;
            var signedUp = opportunity.Count(r => r.Event_ID == eventId);

            var capacity = new Capacity {Display = true};

            if (max == null && min == null)
            {
                capacity.Display = false;
                return capacity;
            }

            int calc;
            if (max == null)
            {
                capacity.Minimum = min.GetValueOrDefault();

                //is this valid?? max is null so put min value in max?
                capacity.Maximum = capacity.Minimum;

                calc = capacity.Minimum - signedUp;
            }
            else if (min == null)
            {
                capacity.Maximum = max.GetValueOrDefault();
                //is this valid??
                capacity.Minimum = capacity.Maximum;
                calc = capacity.Maximum - signedUp;
            }
            else
            {
                capacity.Maximum = max.GetValueOrDefault();
                capacity.Minimum = min.GetValueOrDefault();
                calc = capacity.Minimum - signedUp;
            }

            if (signedUp < capacity.Maximum && signedUp < capacity.Minimum)
            {
                capacity.Message = string.Format("{0} Needed", calc);
                capacity.BadgeType = BadgeType.LabelInfo.ToString();
                capacity.Available = calc;
                capacity.Taken = signedUp;
            }
            else if (signedUp >= capacity.Maximum)
            {
                capacity.Message = "Full";
                capacity.BadgeType = BadgeType.LabelDefault.ToString();
                capacity.Available = calc;
                capacity.Taken = signedUp;
            }

            return capacity;
        }

        public List<int> GetUpdatedOpportunities(string token, SaveRsvpDto dto, Func<Participant, MinistryPlatform.Models.Event, bool> saveFunc = null)
        {
            var updatedEvents = new List<int>();

            //get participant id for Contact
            var participant = _participantService.GetParticipant(dto.ContactId);

            //get events in range
            var events = GetEventsInRange(token, dto.EventTypeId, dto.StartDateUnix.FromUnixTime(), dto.EndDateUnix.FromUnixTime());
            var increment = dto.AlternateWeeks ? 14 : 7;
            var sequenceDate = dto.StartDateUnix.FromUnixTime();
            for (var i = 0; i < events.Count(); i++)
            {
                var @event = events[i];
                sequenceDate = IncrementSequenceDate(@event, sequenceDate, increment);
                if (@event.EventStartDate.Date != sequenceDate.Date)
                {
                    continue;
                }
                updatedEvents.Add(@event.EventId);
                if (saveFunc != null)
                {
                    saveFunc(participant, @event);
                }
                sequenceDate = sequenceDate.AddDays(increment);
            }
            return updatedEvents;
        }

        public List<int> SaveServeRsvp(string token, SaveRsvpDto dto)
        {
            List<MailRow> mailRows = new List<MailRow>();

            var templateId = GetRsvpTemplate(dto.SignUp);
            var opportunity = GetOpportunity(token, dto.OpportunityId, dto.OpportunityIds);
            var groupContact = _contactService.GetContactById(opportunity.GroupContactId);

            var toContact = _contactService.GetContactById(dto.ContactId);
            Opportunity previousOpportunity = null;
            try
            {
                var updatedEvents = GetUpdatedOpportunities(token,
                                                            dto,
                                                            (participant, e) =>
                                                            {
                                                                DateTime from = DateTime.Today.Add(opportunity.ShiftStart);
                                                                DateTime to = DateTime.Today.Add(opportunity.ShiftEnd);
                                                                mailRows.Add(new MailRow()
                                                                {
                                                                    EventDate = e.EventStartDate.ToShortDateString(),
                                                                    Location = opportunity.Room,
                                                                    OpportunityName = opportunity.OpportunityName,
                                                                    ShiftTime = from.ToString("hh:mm tt") + " - " + to.ToString("hh:mm tt")
                                                                });
                                                                var response = CreateRsvp(token, dto.OpportunityId, dto.OpportunityIds, dto.SignUp, participant, e, groupContact);
                                                                previousOpportunity = PreviousOpportunity(response, previousOpportunity);
                                                                templateId = GetTemplateId(templateId, response);
                                                                return true;
                                                            });
                var table = SetupHTMLTable(mailRows).Build();
                var mergeData = SetupMergeData(dto.ContactId,
                                               dto.OpportunityId,
                                               previousOpportunity,
                                               opportunity,
                                               dto.StartDateUnix.FromUnixTime(),
                                               dto.EndDateUnix.FromUnixTime(),
                                               groupContact,
                                               table);

                var communication = SetupCommunication(templateId, groupContact, toContact, mergeData);
                _communicationService.SendMessage(communication);
                return updatedEvents;
            }
            catch (Exception e)
            {
                //var communication = SetupCommunication(templateId, groupContact, toContact);
                var table = SetupHTMLTable(mailRows).Build();
                var communication = SetupCommunication(AppSetting("SignupToServeFailedMessage"),
                                                       groupContact,
                                                       toContact,
                                                       new Dictionary<string, object>
                                                       {
                                                           {"Html_Table", table}
                                                       });
                _communicationService.SendMessage(communication);
                return new List<int>();
            }
        }

        public void SendReminderEmails()
        {
            var token = _apiUserService.GetToken();

            var reminders = _responseService.GetServeReminders(token);
            var serveReminders = reminders.Select(Mapper.Map<ServeReminder>);
            
            var fromId = AppSetting("DefaultContactEmailId");
            var fromEmail = _contactService.GetContactById(fromId).Email_Address;

            serveReminders.ForEach(reminder =>
            {
                var contact = _contactService.GetContactById(reminder.SignedupContactId);

                // is there a template set?
                var templateId = reminder.TemplateId ?? AppSetting("DefaultServeReminderTemplate");
                var mergeData = new Dictionary<string, object>(){
                    {"Opportunity_Title", reminder.OpportunityTitle},
                    {"Nickname", contact.Nickname},
                    {"Event_Start_Date", reminder.EventStartDate.ToShortDateString()},
                    {"Event_End_Date", reminder.EventEndDate.ToShortDateString()},
                    {"Shift_Start", reminder.ShiftStart.FormatAsString()},
                    {"Shift_End", reminder.ShiftEnd.FormatAsString()}
                 };
                var communication = _communicationService.GetTemplateAsCommunication(templateId,
                                                                                     fromId,
                                                                                     fromEmail,
                                                                                     reminder.OpportunityContactId,
                                                                                     reminder.OpportunityEmailAddress,
                                                                                     reminder.SignedupContactId,
                                                                                     reminder.SignedupEmailAddress,
                                                                                     mergeData);
                _communicationService.SendMessage(communication);

            });

        }

        public List<GroupContactDTO> PotentialVolunteers(int groupId, Models.Crossroads.Events.Event evnt, List<GroupParticipant> groupMembers)
        {
            var responses = _opportunityService.GetContactsOpportunityResponseByGroupAndEvent(groupId, evnt.EventId).Select(res =>
            {
                var r = new OpportunityResponseDto()
                {
                    EventId = res.Event_ID,
                    OpportunityEvent = evnt,
                    ParticipantId = res.Participant_ID,
                    ResponseResultId = res.Response_Result_ID,
                    OpportunityId = -1,
                    ResponseId = -1,
                    Closed = false,
                    ResponseDate = res.Response_Date,
                    ContactId = res.Contact_ID
                };
                return r;
            }).ToList();

            //var filteredGroupMembers = new List<GroupContactDTO>();
            return groupMembers.Where(gm =>
            {
                // did this person respond?
                //var responded = responses.All(r => r.ContactId == gm.ContactId);
                var responded = false;
                responses.ForEach(r =>
                {
                    if (r.ContactId == gm.ContactId)
                    {
                        responded = true;
                    }
                });
                if(responded)
                {
                    return false;
                }
                var respondedOnWeekend = RespondedOnWeekend(evnt, gm);
                return !respondedOnWeekend;
            }).ToList().Select( m => new GroupContactDTO()
            {
                ContactId = m.ContactId,
                DisplayName = String.Format("{0}, {1}", m.LastName, m.NickName)
            }).ToList();

        }

        private bool RespondedOnWeekend(Models.Crossroads.Events.Event evnt, GroupParticipant gm)
        {

            // this person did not respond, they are a potential contact so far
            // now determine if this event is a weekend event...
            if (evnt.StartDate.IsWeekend())
            {
                // this is a weekend... make sure that the person did not respond 
                return SearchForResponsesByParticipantAndDate(gm.ParticipantId, evnt.StartDate.DayOfWeek == DayOfWeek.Saturday ? evnt.StartDate.AddDays(1).ToMinistryPlatformSearchFormat() : evnt.StartDate.AddDays(-1).ToMinistryPlatformSearchFormat());
            }
            return false;
        }

        private bool SearchForResponsesByParticipantAndDate(int participantId, string dateToSearch)
        {
            // any response at all on Sunday?
            var search = string.Format(",,{0},,,,{1}", participantId, dateToSearch);
            var otherResponses = _opportunityService.SearchResponseByGroupAndEvent(search);
            return otherResponses.Any();
        }

        private static DateTime IncrementSequenceDate(Event @event, DateTime sequenceDate, int increment)
        {
            if (@event.EventStartDate.Date > sequenceDate.Date)
            {
                sequenceDate = sequenceDate.AddDays(increment);
            }
            return sequenceDate;
        }

        private static int GetTemplateId(int templateId, Dictionary<string, object> response)
        {
            templateId = (templateId != AppSetting("RsvpChangeTemplate"))
                ? response.ToInt("templateId")
                : templateId;
            return templateId;
        }

        private static Opportunity PreviousOpportunity(Dictionary<string, object> response, Opportunity previousOpportunity)
        {
            if (response.ToNullableObject<Opportunity>("previousOpportunity") != null)
            {
                previousOpportunity = response.ToNullableObject<Opportunity>("previousOpportunity");
            }
            return previousOpportunity;
        }

        private Dictionary<string, object> CreateRsvp(string token,
                                                      int opportunityId,
                                                      List<int> opportunityIds,
                                                      bool signUp,
                                                      Participant participant,
                                                      Event @event,
                                                      MyContact groupLeader)
        {
            var response = signUp
                ? HandleYesRsvp(participant, @event, opportunityId, opportunityIds, token)
                : HandleNoRsvp(participant, @event, opportunityIds, token, groupLeader);
            return response;
        }

        private Opportunity GetOpportunity(string token, int opportunityId, List<int> opportunityIds)
        {
            var opportunity = (opportunityId > 0)
                ? _opportunityService.GetOpportunityById(opportunityId, token)
                : _opportunityService.GetOpportunityById(opportunityIds.FirstOrDefault(), token);
            return opportunity;
        }

        private static int GetRsvpTemplate(bool signUp)
        {
            var templateId = signUp ? AppSetting("RsvpYesTemplate") : AppSetting("RsvpNoTemplate");
            return templateId;
        }

        private List<Event> GetEventsInRange(string token, int eventTypeId, DateTime startDate, DateTime endDate)
        {
            var events =
                _eventService.GetEventsByTypeForRange(eventTypeId, startDate, endDate, token)
                    .OrderBy(o => o.EventStartDate)
                    .ToList();
            return events;
        }

        private Dictionary<string, object> HandleYesRsvp(Participant participant,
                                                         Event e,
                                                         int opportunityId,
                                                         IReadOnlyCollection<int> opportunityIds,
                                                         String token)
        {
            var templateId = AppSetting("RsvpYesTemplate");
            var deletedRSVPS = new List<int>();
            Opportunity previousOpportunity = null;

            var opportunity = _opportunityService.GetOpportunityById(opportunityId,token);
            //Try to register this user for the event
            _eventService.RegisterParticipantForEvent(participant.ParticipantId, e.EventId, opportunity.GroupId);

            // Make sure we are only rsvping for 1 opportunity by removing all existing responses
            deletedRSVPS.AddRange(from oid in opportunityIds
                let deletedResponse =
                                      _opportunityService.DeleteResponseToOpportunities(participant.ParticipantId, oid, e.EventId)
                where deletedResponse != 0
                select oid);

            if (deletedRSVPS.Count > 0)
            {
                templateId = AppSetting("RsvpChangeTemplate");
                if (opportunityIds.Count != deletedRSVPS.Count)
                {
                    //Changed yes to yes
                    //prevOppId = deletedRSVPS.First();
                    previousOpportunity = _opportunityService.GetOpportunityById(deletedRSVPS.First(), token);
                }
            }
            var comments = string.Empty; //anything of value to put in comments?
            _opportunityService.RespondToOpportunity(participant.ParticipantId,
                                                     opportunityId,
                                                     comments,
                                                     e.EventId,
                                                     true);
            return new Dictionary<string, object>()
            {
                {"templateId", templateId},
                {"previousOpportunity", previousOpportunity},
                {"rsvp", true}
            };
        }

        private Dictionary<string, object> HandleNoRsvp(Participant participant,
                                                        Event e,
                                                        List<int> opportunityIds,
                                                        string token,
                                                        MyContact groupLeader)
        {
            int templateId;
            Opportunity previousOpportunity = null;

            try
            {
                templateId = AppSetting("RsvpNoTemplate");
                //opportunityId = opportunityIds.First();
                _eventService.UnregisterParticipantForEvent(participant.ParticipantId, e.EventId);
            }
            catch (ApplicationException ex)
            {
                logger.Debug(ex.Message + ": There is no need to remove the event participant because there is not one.");
                templateId = AppSetting("RsvpNoTemplate");
            }

            // Responding no means that we are saying no to all opportunities for this group for this event            
            foreach (var oid in opportunityIds)
            {
                var comments = string.Empty; //anything of value to put in comments?
                var updatedOpp = _opportunityService.RespondToOpportunity(participant.ParticipantId,
                                                                          oid,
                                                                          comments,
                                                                          e.EventId,
                                                                          false);
                if (updatedOpp > 0)
                {
                    previousOpportunity = _opportunityService.GetOpportunityById(oid, token);
                }
            }

            if (previousOpportunity != null)
            {
                var emailName = participant.DisplayName;
                var emailEmail = participant.EmailAddress;
                var emailTeamName = previousOpportunity.GroupName;
                var emailOpportunityName = previousOpportunity.OpportunityName;
                var emailEventDateTime = e.EventStartDate.ToString();

                SendCancellationMessage(groupLeader, emailName, emailEmail, emailTeamName, emailOpportunityName, emailEventDateTime);
            }

            return new Dictionary<string, object>()
            {
                {"templateId", templateId},
                {"previousOpportunity", previousOpportunity},
                {"rsvp", false}
            };
        }

        private void SendCancellationMessage(MyContact groupLeader, string volunteerName, string volunteerEmail, string teamName, string opportunityName, string eventDateTime)
        {
            var templateId = AppSetting("RsvpYesToNo");

            var mergeData = new Dictionary<string, object>
            {
                {"VolunteerName", volunteerName},
                {"VolunteerEmail", volunteerEmail},
                {"TeamName", teamName},
                {"OpportunityName", opportunityName},
                {"EventDateTime", eventDateTime}
            };

            var communication = SetupCommunication(templateId, groupLeader, groupLeader, mergeData);

            _communicationService.SendMessage(communication);
        }

        private Communication SetupCommunication(int templateId, MyContact groupContact, MyContact toContact, Dictionary<string, object> mergeData)
        {
            var template = _communicationService.GetTemplate(templateId);
            var defaultContact = _contactService.GetContactById(_configurationWrapper.GetConfigIntValue("DefaultContactEmailId"));
            return new Communication
            {
                AuthorUserId = 5,
                DomainId = 1,
                EmailBody = template.Body,
                EmailSubject = template.Subject,
                FromContact = new Contact {ContactId = defaultContact.Contact_ID, EmailAddress = defaultContact.Email_Address},
                ReplyToContact = new Contact {ContactId = groupContact.Contact_ID, EmailAddress = groupContact.Email_Address},
                ToContacts = new List<Contact> {new Contact {ContactId = toContact.Contact_ID, EmailAddress = toContact.Email_Address}},
                MergeData = mergeData
            };
        }

        private HtmlElement SetupHTMLTable(List<MailRow> content)
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

            List<HtmlElement> rows = content.Select(rowObj =>
            {
                return new HtmlElement("tr")
                    .Append(new HtmlElement("td", cellAttrs, rowObj.EventDate))
                    .Append(new HtmlElement("td", cellAttrs, rowObj.OpportunityName))
                    .Append(new HtmlElement("td", cellAttrs, rowObj.ShiftTime))
                    .Append(new HtmlElement("td", cellAttrs, rowObj.Location));
            }).ToList();

            return new HtmlElement("table", tableAttrs)
                .Append(SetupTableHeader)
                .Append(rows);
        }

        private HtmlElement SetupTableHeader()
        {
            var headers = TABLE_HEADERS.Select(el => new HtmlElement("th", el)).ToList();
            return new HtmlElement("tr", headers);
        }

        private Dictionary<string, object> SetupMergeData(int contactId,
                                                          int opportunityId,
                                                          Opportunity previousOpportunity,
                                                          Opportunity currentOpportunity,
                                                          DateTime startDate,
                                                          DateTime endDate,
                                                          MyContact groupContact,
                                                          String htmlTable)
        {
            MyContact volunteer = _contactService.GetContactById(contactId);
            return new Dictionary<string, object>
            {
                {"Opportunity_Name", opportunityId == 0 ? "Not Available" : currentOpportunity.OpportunityName},
                {"Start_Date", startDate.ToShortDateString()},
                {"End_Date", endDate.ToShortDateString()},
                {"Shift_Start", currentOpportunity.ShiftStart.FormatAsString() ?? string.Empty},
                {"Shift_End", currentOpportunity.ShiftEnd.FormatAsString() ?? string.Empty},
                {"Room", currentOpportunity.Room ?? string.Empty},
                {"Group_Contact", groupContact.Nickname + " " + groupContact.Last_Name},
                {"Group_Name", currentOpportunity.GroupName},
                {
                    "Previous_Opportunity_Name",
                    previousOpportunity != null ? previousOpportunity.OpportunityName : @"Not Available"
                },
                {"Volunteer_Name", volunteer.Nickname + " " + volunteer.Last_Name},
                {"Html_Table", htmlTable}
            };
        }

        private ServeRole NewServingRole(GroupServingParticipant record)
        {
            return new ServeRole
            {
                Name = record.OpportunityTitle + " " + record.OpportunityRoleTitle,
                RoleId = record.OpportunityId,
                Room = record.Room,
                Minimum = record.OpportunityMinimumNeeded,
                Maximum = record.OpportunityMaximumNeeded,
                ShiftEndTime = record.OpportunityShiftEnd.FormatAsString(),
                ShiftStartTime = record.OpportunityShiftStart.FormatAsString()
            };
        }

        private ServingTime NewServingTime(GroupServingParticipant record)
        {
            return new ServingTime
            {
                Index = record.RowNumber,
                ServingTeams = new List<ServingTeam> {NewServingTeam(record)},
                Time = record.EventStartDateTime.TimeOfDay.ToString()
            };
        }

        private ServingTeam NewServingTeam(GroupServingParticipant record)
        {
            return new ServingTeam
            {
                Index = record.RowNumber,
                EventId = record.EventId,
                EventType = record.EventType,
                EventTypeId = record.EventTypeId,
                GroupId = record.GroupId,
                Members = new List<TeamMember> {NewTeamMember(record)},
                Name = record.GroupName,
                PrimaryContact = record.GroupPrimaryContactEmail,
                PastDeadline = (record.EventStartDateTime.AddDays(0 - record.OpportunitySignUpDeadline) < DateTime.Today),
                PastDeadlineMessage = record.DeadlinePassedMessage
            };
        }

        private TeamMember NewTeamMember(GroupServingParticipant record)
        {
            // new team member
            var member = new TeamMember
            {
                ContactId = record.ContactId,
                EmailAddress = record.ParticipantEmail,
                Index = record.RowNumber,
                LastName = record.ParticipantLastName,
                Name = record.ParticipantNickname,
                Participant = new Participant {ParticipantId = record.ParticipantId}
            };

            member.Roles.Add(NewServingRole(record));

            member.ServeRsvp = NewServeRsvp(record);
            return member;
        }

        private ServeRsvp NewServeRsvp(GroupServingParticipant record)
        {
            if (record.Rsvp != null && !((bool) record.Rsvp))
            {
                return new ServeRsvp {Attending = false, RoleId = 0};
            }
            else if (record.Rsvp != null && ((bool) record.Rsvp))
            {
                return new ServeRsvp {Attending = (bool) record.Rsvp, RoleId = record.OpportunityId};
            }
            return null;
        }

        //public for testing
        public ServeRsvp GetRsvp(int opportunityId, int eventId, TeamMember member)
        {
            if (member.Responses == null)
            {
                return null;
            }
            var r =
                member.Responses.Where(t => t.Opportunity_ID == opportunityId && t.Event_ID == eventId)
                    .Select(t => t.Response_Result_ID)
                    .ToList();
            return r.Count <= 0 ? null : new ServeRsvp {Attending = (r[0] == 1), RoleId = opportunityId};
        }
    }
}