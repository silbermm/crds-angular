using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using crds_angular.Enum;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Services.Interfaces;
using log4net;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Services
{
    public class ServeService : MinistryPlatformBaseService, IServeService
    {
        private readonly IContactRelationshipService _contactRelationshipService;
        private readonly IEventService _eventService;
        private readonly IGroupParticipantService _groupParticipantService;
        private readonly IOpportunityService _opportunityService;
        private readonly IParticipantService _participantService;
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ServeService(IContactRelationshipService contactRelationshipService,
            IOpportunityService opportunityService, IEventService eventService, IParticipantService participantService,
            IGroupParticipantService groupParticipantService)
        {
            _contactRelationshipService = contactRelationshipService;
            _opportunityService = opportunityService;
            _eventService = eventService;
            _participantService = participantService;
            _groupParticipantService = groupParticipantService;
        }

        public List<FamilyMemberDto> GetImmediateFamilyParticipants(int contactId, string token)
        {
            var contactRelationships =
                _contactRelationshipService.GetMyImmediatieFamilyRelationships(contactId, token).ToList();
            var relationships = contactRelationships.Select(contact => new FamilyMemberDto
            {
                ContactId = contact.Contact_Id,
                Email = contact.Email_Address,
                LastName = contact.Last_Name,
                ParticipantId = contact.Participant_Id,
                PreferredName = contact.Preferred_Name
            }).ToList();

            var me = _participantService.GetParticipant(contactId);
            var myParticipant = new FamilyMemberDto
            {
                ContactId = contactId,
                Email = me.EmailAddress,
                ParticipantId = me.ParticipantId
            };

            relationships.Add(myParticipant);
            return relationships;
        }

        public DateTime GetLastServingDate(int opportunityId, string token)
        {
            logger.Debug(string.Format("GetLastOpportunityDate({0}) ", opportunityId));
            return _opportunityService.GetLastOpportunityDate(opportunityId, token);
        }

        public List<ServingDay> GetServingDays(string token, int contactId)
        {
            var family = GetImmediateFamilyParticipants(contactId, token);
            var participants = family.OrderBy(f => f.ParticipantId).Select(f => f.ParticipantId).ToList();
            var servingParticipants = _groupParticipantService.GetServingParticipants(participants);
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
                    servingDay.ServeTimes = new List<ServingTime> { NewServingTime(record) };

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

            var capacity = new Capacity { Display = true };

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
            int eventTypeId,
            DateTime startDate,
            DateTime endDate,
            bool signUp, bool alternateWeeks)
        {
            //get participant id for Contact
            var participant = _participantService.GetParticipant(contactId);
            //get events in range
            var events = _eventService.GetEventsByTypeForRange(eventTypeId, startDate, endDate, token);
            var includeThisWeek = true;
            foreach (var e in events)
            {
                if ((!alternateWeeks) || includeThisWeek)
                {
                    //for each event in range create an event participant & opportunity response
                    if (signUp)
                    {
                        _eventService.registerParticipantForEvent(participant.ParticipantId, e.EventId);
                    }
                    var comments = string.Empty; //anything of value to put in comments?
                    _opportunityService.RespondToOpportunity(participant.ParticipantId, opportunityId, comments,
                        e.EventId, signUp);
                }
                includeThisWeek = !includeThisWeek;
            }

            return true;
        }

        private ServeRole NewServingRole(GroupServingParticipant record)
        {
            return new ServeRole
            {
                Name = record.OpportunityTitle + " " + record.OpportunityRoleTitle,
                RoleId = record.OpportunityId,
                Minimum = record.OpportunityMinimumNeeded,
                Maximum = record.OpportunityMaximumNeeded
            };
        }

        private ServingTime NewServingTime(GroupServingParticipant record)
        {
            return new ServingTime
            {
                Index = record.RowNumber,
                ServingTeams = new List<ServingTeam> { NewServingTeam(record) },
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
                Members = new List<TeamMember> { NewTeamMember(record) },
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
                Participant = new Participant { ParticipantId = record.ParticipantId }
            };

            member.Roles.Add(NewServingRole(record));

            if (record.Rsvp != null)
            {
                member.ServeRsvp = new ServeRsvp { Attending = (bool)record.Rsvp, RoleId = record.OpportunityId };
            }
            return member;
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
            return r.Count <= 0 ? null : new ServeRsvp { Attending = (r[0] == 1), RoleId = opportunityId };
        }
    }
}