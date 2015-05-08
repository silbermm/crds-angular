using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using AutoMapper;
using crds_angular.Enum;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Services.Interfaces;
using log4net;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using Event = MinistryPlatform.Models.Event;
using Response = MinistryPlatform.Models.Response;

namespace crds_angular.Services
{
    public class ServeService : MinistryPlatformBaseService, IServeService
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IGroupService _groupService;
        private IContactRelationshipService _contactRelationshipService;
        private IPersonService _personService;
        private IAuthenticationService _authenticationService;
        private IOpportunityService _opportunityService;
        private IEventService _eventService;
        private IParticipantService _participantService;
        private IGroupParticipantService _groupParticipantService;

        public ServeService(IGroupService groupService, IContactRelationshipService contactRelationshipService,
            IPersonService personService, IAuthenticationService authenticationService,
            IOpportunityService opportunityService, IEventService eventService, IParticipantService participantService,
            IGroupParticipantService groupParticipantService)
        {
            this._groupService = groupService;
            this._contactRelationshipService = contactRelationshipService;
            this._personService = personService;
            this._opportunityService = opportunityService;
            this._authenticationService = authenticationService;
            this._eventService = eventService;
            this._participantService = participantService;
            this._groupParticipantService = groupParticipantService;
        }

        private List<Response> GetParticipantResponses(int participantId)
        {
            logger.Debug(string.Format("GetParticipantResponses({0}) ", participantId));
            return _participantService.GetParticipantResponses(participantId);
        }

        public List<FamilyMemberDto> GetImmediateFamilyParticipants(int contactId, string token)
        {
            var contactRelationships =
                _contactRelationshipService.GetMyImmediatieFamilyRelationships(contactId, token).ToList();
            return contactRelationships.Select(contact => new FamilyMemberDto
            {
                ContactId = contact.Contact_Id,
                Email = contact.Email_Address,
                LastName = contact.Last_Name,
                ParticipantId = contact.Participant_Id,
                PreferredName = contact.Preferred_Name
            }).ToList();
        }

        public List<FamilyMember> GetMyImmediateFamily(int contactId, string token)
        {
            logger.Debug(string.Format("GetMyImmediatieFamilyRelationships({0}) ", contactId));
            var contactRelationships =
                _contactRelationshipService.GetMyImmediatieFamilyRelationships(contactId, token).ToList();
            var familyMembers = Mapper.Map<List<ContactRelationship>, List<FamilyMember>>(contactRelationships);

            foreach (var familyMember in familyMembers)
            {
                logger.Debug(string.Format("GetParticipant({0}) ", familyMember.ContactId));
                familyMember.Participant = _participantService.GetParticipant(familyMember.ContactId);
                var responses = GetParticipantResponses(familyMember.Participant.ParticipantId);
                familyMember.Responses = responses;
            }

            //now get info for Contact
            logger.Debug(string.Format("GetLoggedInUserProfile() "));
            var myProfile = _personService.GetLoggedInUserProfile(token);
            var me = new FamilyMember
            {
                ContactId = myProfile.ContactId,
                Email = myProfile.EmailAddress,
                LastName = myProfile.LastName,
                PreferredName = myProfile.NickName ?? myProfile.FirstName,
                Participant = _participantService.GetParticipant(myProfile.ContactId)
            };
            me.Responses = GetParticipantResponses(me.Participant.ParticipantId);
            familyMembers.Add(me);

            return familyMembers;
        }

        public List<ServingDay> GetServingDaysFaster(string token)
        {
            //token doesn't seem to be needed? Can we remove? 

            //needs to accept an array/list of partiticpant ids as parm

            var servingParticipants = _groupParticipantService.GetServingParticipants();
            //does this need to be sorted?

            var servingDays = new List<ServingDay>();

            foreach (var record in servingParticipants)
            {
                //list.Any(cus => cus.FirstName == "John");
                var day = servingDays.SingleOrDefault(d => d.Date == record.EventStartDateTime);
                if (day != null)
                {
                    //this day already in list

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
                            if (member != null)
                            {
                                //member already on team
                                //TO-DO figure out how to do capacity
                                member.Roles.Add(new ServeRole { Capacity = null, Name = record.OpportunityRoleTitle, RoleId = record.GroupRoleId });
                            }
                            else
                            {
                                team.Members.Add(TeamMemberFaster(record));
                            }
                        }
                        else
                        {
                            time.ServingTeams.Add(ServingTeamFaster(record));
                        }
                    }
                    else
                    {
                        //var servingTime = new ServingTime();
                        //servingTime.Time = record.EventStartDateTime.TimeOfDay.ToString();
                        //servingTime.ServingTeams = null;


                        day.ServeTimes.Add(ServingTimeFaster(record));

                    }
                }
                else
                {
                    //new day for the list

                    // new day
                    var servingDay = new ServingDay();
                    servingDay.Day = record.EventStartDateTime.Date.ToString("d");
                    servingDay.Date = record.EventStartDateTime;
                    servingDay.ServeTimes = new List<ServingTime> { ServingTimeFaster(record) };

                    servingDays.Add(servingDay);

                }
                
            }

            return servingDays;
        }

        private static ServingTime ServingTimeFaster(GroupServingParticipant record)
        {
            var servingTime = new ServingTime();
            servingTime.ServingTeams = new List<ServingTeam> {ServingTeamFaster(record)};
            servingTime.Time = record.EventStartDateTime.TimeOfDay.ToString();
            return servingTime;
        }

        private static ServingTeam ServingTeamFaster(GroupServingParticipant record)
        {
            var servingTeam = new ServingTeam();
            servingTeam.EventType = record.EventType;
            servingTeam.EventTypeId = record.EventTypeId;
            servingTeam.GroupId = record.GroupId;
            servingTeam.Members = new List<TeamMember> {TeamMemberFaster(record)};
            servingTeam.Name = record.GroupName;
            servingTeam.PrimaryContact = record.GroupPrimaryContactEmail;
            return servingTeam;
        }

        private static TeamMember TeamMemberFaster(GroupServingParticipant record)
        {
// new team member
            var member = new TeamMember();
            member.ContactId = record.ContactId;
            member.EmailAddress = record.ParticipantEmail;
            member.LastName = record.ParticipantLastName;
            member.Name = record.ParticipantNickname;
            member.Participant = new Participant {ParticipantId = record.ParticipantId};
            //member.Responses = null;  //don't think we need this

            ////TO-DO figure out how to do capacity
            member.Roles = new List<ServeRole>
            {
                new ServeRole {Capacity = null, Name = record.OpportunityRoleTitle, RoleId = record.GroupRoleId}
            };
            if (record.Rsvp != null)
            {
                member.ServeRsvp = new ServeRsvp {Attending = (bool) record.Rsvp, RoleId = record.GroupRoleId};
            }
            return member;
        }

        public List<ServingDay> GetServingDays(string token)
        {
            var servingTeams = GetServingTeams(token);
            var serveDays = new List<ServingDay>();

            foreach (var team in servingTeams)
            {
                logger.Debug("GetOpportunitiesForGroup: " + team.GroupId);
                var opportunities = this._opportunityService.GetOpportunitiesForGroup(team.GroupId, token);

                if (opportunities == null) continue;

                foreach (var opportunity in opportunities)
                {
                    if (opportunity.EventType == null) continue;

                    //team events
                    var events = ParseServingEvents(opportunity.Events);

                    foreach (var e in events)
                    {
                        var serveRole = new ServeRole
                        {
                            Name = opportunity.OpportunityName + " " + opportunity.RoleTitle,
                            Capacity =
                                this.OpportunityCapacity(opportunity, e.EventId, token),
                            RoleId = opportunity.OpportunityId
                        };

                        var serveDay = serveDays.SingleOrDefault(r => r.Day == e.EventStartDate.Date.ToString("d"));
                        if (serveDay != null)
                        {
                            //day already in list

                            //check if time is in list
                            var serveTime =
                                serveDay.ServeTimes.SingleOrDefault(s => s.Time == e.EventStartDate.TimeOfDay.ToString());
                            if (serveTime != null)
                            {
                                //time in list
                                //is team already in list??
                                var existingTeam = serveTime.ServingTeams.SingleOrDefault(t => t.GroupId == team.GroupId);
                                if (existingTeam != null)
                                {
                                    //team exists
                                    foreach (var teamMember in team.Members)
                                    {
                                        foreach (var role in teamMember.Roles)
                                        {
                                            if (role.Name != opportunity.RoleTitle) continue;

                                            TeamMember member = null;
                                            try
                                            {
                                                member =
                                                    existingTeam.Members.SingleOrDefault(
                                                        m => m.ContactId == teamMember.ContactId);
                                            }
                                            catch (Exception ex)
                                            {
                                                var roleMsg = string.Format("Opportunity Role: {0}",
                                                    opportunity.RoleTitle);
                                                var contactMsg = string.Format("Contact Id: {0}",
                                                    teamMember.ContactId);
                                                var teamMsg = string.Format("Team: {0}", team.Name);
                                                var message = string.Format("{0} {1} {2}", roleMsg, contactMsg,
                                                    teamMsg);
                                                throw new ApplicationException(
                                                    "Duplicate Group Member: " + message, ex);
                                            }
                                            if (member == null)
                                            {
                                                member = new TeamMember
                                                {
                                                    ContactId = teamMember.ContactId,
                                                    Name = teamMember.Name,
                                                    LastName = teamMember.LastName,
                                                    EmailAddress = teamMember.EmailAddress,
                                                    Participant = teamMember.Participant,
                                                    Responses = teamMember.Responses,
                                                    ServeRsvp =
                                                        GetRsvp(opportunity.OpportunityId, e.EventId,
                                                            teamMember)
                                                };
                                                existingTeam.Members.Add(member);
                                            }
                                            else
                                            {
                                                //does member have rsvp for this role?
                                                if (member.ServeRsvp == null)
                                                {
                                                    member.ServeRsvp = GetRsvp(opportunity.OpportunityId, e.EventId,
                                                        member);
                                                }
                                            }
                                            member.Roles.Add(serveRole);
                                        }
                                    }
                                }
                                else
                                {
                                    serveTime.ServingTeams.Add(NewServingTeam(team, opportunity, serveRole, e.EventId));
                                }
                            }
                            else
                            {
                                //time not in list
                                serveTime = new ServingTime {Time = e.EventStartDate.TimeOfDay.ToString()};
                                serveTime.ServingTeams.Add(NewServingTeam(team, opportunity, serveRole, e.EventId));

                                serveDay.ServeTimes.Add(serveTime);
                            }
                        }
                        else
                        {
                            //day not in list add it
                            serveDay = new ServingDay
                            {
                                Day = e.EventStartDate.Date.ToString("d"),
                                Date = e.EventStartDate
                            };
                            var serveTime = new ServingTime {Time = e.EventStartDate.TimeOfDay.ToString()};
                            serveTime.ServingTeams.Add(NewServingTeam(team, opportunity, serveRole, e.EventId));

                            serveDay.ServeTimes.Add(serveTime);
                            serveDays.Add(serveDay);
                        }
                    }
                }
            }

            //sort everything for front end
            var preSortedServeDays = serveDays.OrderBy(s => s.Date).ToList();
            var sortedServeDays = new List<ServingDay>();
            foreach (var serveDay in preSortedServeDays)
            {
                var sortedServeDay = new ServingDay
                {
                    Day = serveDay.Day
                };
                var sortedServeTimes = serveDay.ServeTimes.OrderBy(s => s.Time).ToList();
                sortedServeDay.ServeTimes = sortedServeTimes;
                sortedServeDays.Add(sortedServeDay);
            }

            return sortedServeDays;
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

        //public for testing
        public Capacity OpportunityCapacity(Opportunity opportunity, int eventId, string token)
        {
            var min = opportunity.MinimumNeeded;
            var max = opportunity.MaximumNeeded;
            var signedUp = opportunity.Responses.Count(r => r.Event_ID == eventId);

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

        public List<ServingTeam> GetServingTeams(string token)
        {
            logger.Debug(string.Format("GetContactId() "));
            var contactId = _authenticationService.GetContactId(token);
            var servingTeams = new List<ServingTeam>();

            //Go get family
            var familyMembers = GetMyImmediateFamily(contactId, token);
            foreach (var familyMember in familyMembers)
            {
                logger.Debug(string.Format("GetServingTeams({0}) ", familyMember.ContactId));
                var groups = this._groupService.GetServingTeams(familyMember.ContactId, token);
                foreach (var group in groups)
                {
                    var servingTeam = servingTeams.SingleOrDefault(t => t.GroupId == group.GroupId);
                    if (servingTeam != null)
                    {
                        //is this person already on team?
                        var member = servingTeam.Members.SingleOrDefault(m => m.ContactId == familyMember.ContactId);
                        if (member == null)
                        {
                            member = new TeamMember
                            {
                                ContactId = familyMember.ContactId,
                                Name = familyMember.PreferredName,
                                LastName = familyMember.LastName,
                                EmailAddress = familyMember.Email,
                                Participant = familyMember.Participant,
                                Responses = familyMember.Responses
                            };
                            servingTeam.Members.Add(member);
                        }
                        var role = new ServeRole {Name = group.GroupRole};
                        member.Roles.Add(role);
                    }
                    else
                    {
                        servingTeam = new ServingTeam
                        {
                            Name = group.Name,
                            GroupId = group.GroupId,
                            PrimaryContact = group.PrimaryContact
                        };
                        var groupMembers = new List<TeamMember> {NewTeamMember(familyMember, group)};
                        servingTeam.Members = groupMembers;
                        servingTeams.Add(servingTeam);
                    }
                }
            }
            return servingTeams;
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

        private TeamMember NewTeamMember(FamilyMember familyMember, Group group)
        {
            var teamMember = new TeamMember
            {
                ContactId = familyMember.ContactId,
                Name = familyMember.PreferredName,
                LastName = familyMember.LastName,
                EmailAddress = familyMember.Email,
                Participant = familyMember.Participant,
                Responses = familyMember.Responses
            };

            var role = new ServeRole {Name = @group.GroupRole};
            teamMember.Roles = new List<ServeRole> {role};

            return teamMember;
        }

        private TeamMember NewTeamMember(TeamMember teamMember, ServeRole role, int eventId)
        {
            var tm = new TeamMember();
            tm.Name = teamMember.Name;
            tm.LastName = teamMember.LastName;
            tm.ContactId = teamMember.ContactId;
            tm.EmailAddress = teamMember.EmailAddress;
            tm.ServeRsvp = GetRsvp(role.RoleId, eventId, teamMember);

            tm.Participant = teamMember.Participant;
            tm.Responses = teamMember.Responses;

            var newTeamMember = new TeamMember
            {
                Name = teamMember.Name,
                LastName = teamMember.LastName,
                ContactId = teamMember.ContactId,
                EmailAddress = teamMember.EmailAddress,
                ServeRsvp = GetRsvp(role.RoleId, eventId, teamMember),
                Participant = teamMember.Participant,
                Responses = teamMember.Responses
            };
            newTeamMember.Roles.Add(role);

            return newTeamMember;
        }

        private List<TeamMember> NewTeamMembersWithRoles(List<TeamMember> teamMembers,
            string opportunityRoleTitle, ServeRole teamRole, int eventId)
        {
            var members = new List<TeamMember>();
            foreach (var teamMember in teamMembers)
            {
                foreach (var role in teamMember.Roles)
                {
                    if (role.Name == opportunityRoleTitle)
                    {
                        members.Add(NewTeamMember(teamMember, teamRole, eventId));
                    }
                }
            }
            return members;
        }

        private ServingTeam NewServingTeam(ServingTeam team, Opportunity opportunity, ServeRole serveRole, int eventId)
        {
            var servingTeam = new ServingTeam
            {
                Name = team.Name,
                GroupId = team.GroupId,
                Members = NewTeamMembersWithRoles(team.Members, opportunity.RoleTitle, serveRole, eventId),
                PrimaryContact = team.PrimaryContact,
                EventType = opportunity.EventType,
                EventTypeId = opportunity.EventTypeId
            };
            return servingTeam;
        }

        private static List<Event> ParseServingEvents(IEnumerable<Event> events)
        {
            var today = DateTime.Now;
            return events.Select(e => new Event
            {
                EventTitle = e.EventTitle,
                EventStartDate = e.EventStartDate,
                EventId = e.EventId,
                EventType = e.EventType
            })
                .Where(
                    e =>
                        e.EventStartDate.Month == today.Month && e.EventStartDate.Day >= today.Day &&
                        e.EventStartDate.Year == today.Year)
                .ToList();
        }

        public DateTime GetLastServingDate(int opportunityId, string token)
        {
            logger.Debug(string.Format("GetLastOpportunityDate({0}) ", opportunityId));
            return _opportunityService.GetLastOpportunityDate(opportunityId, token);
        }
    }
}