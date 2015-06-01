using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using crds_angular.Enum;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Extensions;
using log4net;
using log4net.Repository.Hierarchy;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using IGroupService = MinistryPlatform.Translation.Services.Interfaces.IGroupService;

namespace crds_angular.Services
{
    public class ServeService : MinistryPlatformBaseService, IServeService
    {
        private readonly IContactService _contactService;
        private readonly IContactRelationshipService _contactRelationshipService;
        private readonly IEventService _eventService;
        private readonly IGroupParticipantService _groupParticipantService;
        private readonly IGroupService _groupService;
        private readonly IOpportunityService _opportunityService;
        private readonly IParticipantService _participantService;
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ServeService(IContactService contactService, IContactRelationshipService contactRelationshipService,
            IOpportunityService opportunityService, IEventService eventService, IParticipantService participantService,
            IGroupParticipantService groupParticipantService, IGroupService groupService)
        {
            _contactService = contactService;
            _contactRelationshipService = contactRelationshipService;
            _opportunityService = opportunityService;
            _eventService = eventService;
            _participantService = participantService;
            _groupParticipantService = groupParticipantService;
            _groupService = groupService;
        }

        public List<FamilyMember> GetImmediateFamilyParticipants(int contactId, string token)
        {
            var relationships = new List<FamilyMember>();
            // get for contact Id
            var me = _participantService.GetParticipant(contactId);
            var myParticipant = new FamilyMember
            {
                ContactId = contactId,
                Email = me.EmailAddress,
                LoggedInUser = true,
                ParticipantId = me.ParticipantId,
                PreferredName = me.PreferredName
            };
            relationships.Add(myParticipant);

            // get family for contact Id
            var contactRelationships =
                _contactRelationshipService.GetMyImmediatieFamilyRelationships(contactId, token).ToList();
            var family = contactRelationships.Select(contact => new FamilyMember
            {
                ContactId = contact.Contact_Id,
                Email = contact.Email_Address,
                LastName = contact.Last_Name,
                LoggedInUser = false,
                ParticipantId = contact.Participant_Id,
                PreferredName = contact.Preferred_Name
            }).ToList();

            relationships.AddRange(family.OrderBy(o=>o.PreferredName));

            return relationships;
        }

        public DateTime GetLastServingDate(int opportunityId, string token)
        {
            logger.Debug(string.Format("GetLastOpportunityDate({0}) ", opportunityId));
            return _opportunityService.GetLastOpportunityDate(opportunityId, token);
        }

        public List<QualifiedServerDto> GetQualifiedServers(int groupId, int contactId, string token)
        {
            var qualifiedServers = new List<QualifiedServerDto>();
            var immediateFamilyParticipants = GetImmediateFamilyParticipants(contactId, token);

            foreach (var participant in immediateFamilyParticipants)
            {
                var membership = _groupService.ParticipantGroupMember(groupId, participant.ParticipantId);
                var opportunityResponse = _opportunityService.GetMyOpportunityResponses(participant.ContactId,115, token);
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
            var family = GetImmediateFamilyParticipants(contactId, token);
            var participants = family.OrderBy(f => f.ParticipantId).Select(f => f.ParticipantId).ToList();
            var servingParticipants = _groupParticipantService.GetServingParticipants(participants, from, to);
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

        public Capacity OpportunityCapacity(int opportunityId, int eventId, int? minNeeded, int? maxNeeded, string token)
        {
            var opportunity = _opportunityService.GetOpportunityResponses(opportunityId, token);
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

        public bool SaveServeRsvp(string token,
            int contactId,
            int opportunityId,
            List<int> opportunityIds, 
            int eventTypeId,
            DateTime startDate,
            DateTime endDate,
            bool signUp, 
            bool alternateWeeks)
        {
            //get participant id for Contact
            var participant = _participantService.GetParticipant(contactId);
            //get events in range
            var events = _eventService.GetEventsByTypeForRange(eventTypeId, startDate, endDate, token);
            var includeThisWeek = true;
            var templateId = AppSetting("RsvpYesTemplate");
            var deletedRSVPS = new List<int>();
            var prevOpp = 0;

            foreach (var e in events)
            {
                if ((!alternateWeeks) || includeThisWeek)
                {
                    //for each event in range create an event participant & opportunity response
                    if (signUp)                    
                    {
                        _eventService.registerParticipantForEvent(participant.ParticipantId, e.EventId);

                        // Make sure we are only rsvping for 1 opportunity by removing all existing responses
                        foreach( var oid in opportunityIds)
                        {
                            var deletedResponse = _opportunityService.DeleteResponseToOpportunities(participant.ParticipantId, oid, e.EventId);
                            if (deletedResponse != 0)
                            {
                                deletedRSVPS.Add(deletedResponse);
                            }
                        }

                        if (deletedRSVPS.Count > 0)
                        {
                            templateId = AppSetting("RsvpChangeTemplate");
                            if (opportunityIds.Count == deletedRSVPS.Count)
                            {
                                //Changed from NO to YES. 
                                prevOpp = 0;
                            }
                            else
                            {
                                //Changed yes to yes
                                prevOpp = deletedRSVPS.First();
                            }
                        }

                        var comments = string.Empty; //anything of value to put in comments?
                        _opportunityService.RespondToOpportunity(participant.ParticipantId, opportunityId, comments,
                            e.EventId, true);
                    }
                    else
                    {
                        try
                        {
                            templateId = AppSetting("RsvpChangeTemplate");
                            opportunityId = opportunityIds.First();
                            //if there is already a participant, remove it because they've changed to "No"
                            _eventService.unRegisterParticipantForEvent(participant.ParticipantId, e.EventId);
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
                            var updatedOpp = _opportunityService.RespondToOpportunity(participant.ParticipantId, oid, comments,
                                e.EventId, false);
                            if (updatedOpp > 0)
                            {
                                prevOpp = updatedOpp;
                            }
                        }
                        
                    }
                    
                }
                includeThisWeek = !includeThisWeek;
            }

            //Send Confirmation Email Asynchronously
            var thread = new Thread(() => SendRSVPConfirmation(contactId, opportunityId, prevOpp, startDate, endDate, templateId, token));
            thread.Start();

            return true;
        }

        private void SendRSVPConfirmation(int contactId, int opportunityId, int prevOppId, DateTime startDate, DateTime endDate, int templateId, string token)
        {
            var template = CommunicationService.GetTemplate(templateId, token);

            //Go get Opportunity deets
            var opp = _opportunityService.GetOpportunityById(opportunityId, token);

            //Go get Previous Opportunity deets
            var prevOpp = new Opportunity();
            if (prevOppId > 0)
            {
                prevOpp = _opportunityService.GetOpportunityById(opportunityId, token);
            }
            else
            {
                prevOpp.OpportunityName = "No";
            }

            //Go get from/to contact info
            var fromEmail = _contactService.GetContactEmail(opp.GroupContactId);
            var toEmail = _contactService.GetContactEmail(contactId);

            var comm = new Communication
            {
                AuthorUserId = 5,
                DomainId = 1,
                EmailBody = template.Body,
                EmailSubject = template.Subject,
                FromContactId = opp.GroupContactId,
                FromEmailAddress = fromEmail,
                ReplyContactId = opp.GroupContactId,
                ReplyToEmailAddress = fromEmail,
                ToContactId = contactId,
                ToEmailAddress = toEmail
            };

            var mergeData = new Dictionary<string, object>
            {
                {"Opportunity_Name", opp.OpportunityName},
                {"Start_Date", startDate.ToShortDateString()},
                {"End_Date", endDate.ToShortDateString()},
                {"Shift_Start", opp.ShiftStart.FormatAsString()},
                {"Shift_End", opp.ShiftEnd.FormatAsString()},
                {"Room", opp.Room ?? string.Empty},
                {"Group_Contact", opp.GroupContactName},
                {"Group_Name", opp.GroupName},
                {"Previous_Opportunity_Name", prevOpp.OpportunityName}
            };

            CommunicationService.SendMessage(comm, mergeData, token);
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
                PrimaryContact = record.GroupPrimaryContactEmail
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
                return new ServeRsvp { Attending = false, RoleId = 0};
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
