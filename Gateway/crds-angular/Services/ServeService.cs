using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using Event = MinistryPlatform.Models.Event;

namespace crds_angular.Services
{
    public class ServeService : MinistryPlatformBaseService, IServeService
    {
        private IGroupService _groupService;
        private IContactRelationshipService _contactRelationshipService;
        private IPersonService _personService;
        private IAuthenticationService _authenticationService;
        private IOpportunityService _opportunityService;
        private IEventService _eventService;

        public ServeService(IGroupService groupService, IContactRelationshipService contactRelationshipService,
            IPersonService personService, IAuthenticationService authenticationService,
            IOpportunityService opportunityService, IEventService eventService)
        {
            this._groupService = groupService;
            this._contactRelationshipService = contactRelationshipService;
            this._personService = personService;
            //this._contactService = contactService;
            this._opportunityService = opportunityService;
            this._authenticationService = authenticationService;
            this._eventService = eventService;
        }

        public List<FamilyMember> GetMyImmediateFamily(int contactId, string token)
        {
            var contactRelationships =
                _contactRelationshipService.GetMyImmediatieFamilyRelationships(contactId, token).ToList();
            var familyMembers = Mapper.Map<List<ContactRelationship>, List<FamilyMember>>(contactRelationships);

            //now get info for Contact
            var myProfile = _personService.GetLoggedInUserProfile(token);
            var me = new FamilyMember
            {
                ContactId = myProfile.ContactId,
                Email = myProfile.EmailAddress,
                LastName = myProfile.LastName,
                PreferredName = myProfile.NickName ?? myProfile.FirstName
            };
            familyMembers.Add(me);

            return familyMembers;
        }

        public List<ServingDay> GetServingDays(string token)
        {
            var servingTeams = GetServingTeams(token);
            var serveDays = new List<ServingDay>();

            foreach (var team in servingTeams)
            {
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
                            Capacity = opportunity.Capacity,
                            RoleId = opportunity.OpportunityId,
                            SlotsTaken =
                                this._opportunityService.GetOpportunitySignupCount(opportunity.OpportunityId, e.EventId,
                                    token)
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
                                                    EmailAddress = teamMember.EmailAddress
                                                };
                                                existingTeam.Members.Add(member);
                                            }
                                            member.Roles.Add(serveRole);
                                        }
                                    }
                                }
                                else
                                {
                                    serveTime.ServingTeams.Add(NewServingTeam(team, opportunity, serveRole));
                                }
                            }
                            else
                            {
                                //time not in list
                                serveTime = new ServingTime {Time = e.EventStartDate.TimeOfDay.ToString()};
                                serveTime.ServingTeams.Add(NewServingTeam(team, opportunity, serveRole));
                                serveDay.ServeTimes.Add(serveTime);
                            }
                        }
                        else
                        {
                            //day not in list add it
                            serveDay = new ServingDay
                            {
                                Day = e.EventStartDate.Date.ToString("d"),
                                Date = e.EventStartDate,
                                EventType = opportunity.EventType,
                                EventTypeId = opportunity.EventTypeId
                            };
                            var serveTime = new ServingTime {Time = e.EventStartDate.TimeOfDay.ToString()};
                            serveTime.ServingTeams.Add(NewServingTeam(team, opportunity, serveRole));

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
                    Day = serveDay.Day,
                    EventType = serveDay.EventType,
                    EventTypeId = serveDay.EventTypeId
                };
                var sortedServeTimes = serveDay.ServeTimes.OrderBy(s => s.Time).ToList();
                sortedServeDay.ServeTimes = sortedServeTimes;
                sortedServeDays.Add(sortedServeDay);
            }

            return sortedServeDays;
        }

        public List<ServingTeam> GetServingTeams(string token)
        {
            var contactId = _authenticationService.GetContactId(token);
            var servingTeams = new List<ServingTeam>();

            //Go get family
            var familyMembers = GetMyImmediateFamily(contactId, token);
            foreach (var familyMember in familyMembers)
            {
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
                                EmailAddress = familyMember.Email
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

        public bool SaveServeResponse(string token, 
            int contactid, 
            int opportunityId, 
            int eventTypeId,
            DateTime startDate,
            DateTime endDate)
        {
            //get participant id for Contact
            var participant = _authenticationService.GetParticipantRecord(token);

            //get events in range
            var events = _eventService.GetEventsByTypeForRange(eventTypeId, startDate, endDate, token);
            foreach (var e in events)
            {
                //for each event in range create an event participant & opportunity response
                _eventService.registerParticipantForEvent(participant.ParticipantId, e.EventId);
                var comments = string.Empty; //anything of value to put in comments?
                _opportunityService.RespondToOpportunity(token, opportunityId, comments);
            }

            return true;
        }


        private static TeamMember NewTeamMember(FamilyMember familyMember, Group group)
        {
            var teamMember = new TeamMember
            {
                ContactId = familyMember.ContactId,
                Name = familyMember.PreferredName,
                LastName = familyMember.LastName,
                EmailAddress = familyMember.Email
            };

            var role = new ServeRole {Name = @group.GroupRole};
            teamMember.Roles = new List<ServeRole> {role};

            return teamMember;
        }

        private static TeamMember NewTeamMember(TeamMember teamMember, ServeRole role)
        {
            var newTeamMember = new TeamMember
            {
                Name = teamMember.Name,
                LastName = teamMember.LastName,
                ContactId = teamMember.ContactId,
                EmailAddress = teamMember.EmailAddress
            };
            newTeamMember.Roles.Add(role);

            return newTeamMember;
        }

        private static List<TeamMember> NewTeamMembersWithRoles(List<TeamMember> teamMembers,
            string opportunityRoleTitle, ServeRole teamRole)
        {
            var members = new List<TeamMember>();
            foreach (var teamMember in teamMembers)
            {
                foreach (var role in teamMember.Roles)
                {
                    if (role.Name == opportunityRoleTitle)
                    {
                        members.Add(NewTeamMember(teamMember, teamRole));
                    }
                }
            }
            return members;
        }

        private static ServingTeam NewServingTeam(ServingTeam team, Opportunity opportunity, ServeRole tmpRole)
        {
            var servingTeam = new ServingTeam
            {
                Name = team.Name,
                GroupId = team.GroupId,
                Members = NewTeamMembersWithRoles(team.Members, opportunity.RoleTitle, tmpRole),
                PrimaryContact = team.PrimaryContact
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
                //DateOnly = e.EventStartDate.Date.ToString("d"),
                //TimeOnly = e.EventStartDate.TimeOfDay.ToString(),
                EventId = e.EventId,
                EventType = e.EventType
            })
                .Where(
                    e =>
                        e.EventStartDate.Month == today.Month && e.EventStartDate.Day >= today.Day &&
                        e.EventStartDate.Year == today.Year)
                .ToList();
        }
    }
}