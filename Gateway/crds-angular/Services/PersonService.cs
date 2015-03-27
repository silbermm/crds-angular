using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using crds_angular.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Attribute = MinistryPlatform.Models.Attribute;
using Event = MinistryPlatform.Models.Event;
using ServingTeam = crds_angular.Models.Crossroads.Serve.ServingTeam;

namespace crds_angular.Services
{
    public class PersonService : MinistryPlatformBaseService, IPersonService
    {
        private IGroupService _groupService;
        private IContactRelationshipService _contactRelationshipService;
        private IContactService _contactService;
        private IOpportunityService _opportunityService;

        public PersonService(IGroupService groupService, IContactRelationshipService contactRelationshipService,
            IContactService contactService, IOpportunityService opportunityService)
        {
            this._groupService = groupService;
            this._contactRelationshipService = contactRelationshipService;
            this._contactService = contactService;
            this._opportunityService = opportunityService;
        }

        public void SetProfile(String token, Person person)
        {
            var contactDictionary = getDictionary(person.GetContact());
            var householdDictionary = getDictionary(person.GetHousehold());
            var addressDictionary = getDictionary(person.GetAddress());
            addressDictionary.Add("State/Region", addressDictionary["State"]);

            MinistryPlatformService.UpdateRecord(AppSetting("MyContact"), contactDictionary, token);

            if (addressDictionary["Address_ID"] != null)
            {
                //address exists, update it
                MinistryPlatformService.UpdateRecord(AppSetting("MyAddresses"), addressDictionary, token);
            }
            else
            {
                //address does not exist, create it, then attach to household
                var addressId = MinistryPlatformService.CreateRecord(AppSetting("MyAddresses"), addressDictionary, token);
                householdDictionary.Add("Address_ID", addressId);
            }
            MinistryPlatformService.UpdateRecord(AppSetting("MyHousehold"), householdDictionary, token);
        }

        public List<Skill> GetLoggedInUserSkills(int contactId, string token)
        {
            return GetSkills(contactId, token);
        }

        public Person GetLoggedInUserProfile(String token)
        {
            var contact = _contactService.GetMyProfile(token);

            var person = new Person
            {
                ContactId = contact.ContactId,
                EmailAddress = contact.EmailAddress,
                NickName = contact.NickName,
                FirstName = contact.FirstName,
                MiddleName = contact.MiddleName,
                LastName = contact.LastName,
                MaidenName = contact.MaidenName,
                MobilePhone = contact.MobilePhone,
                MobileCarrierId = contact.MobileCarrierId,
                DateOfBirth = contact.DateOfBirth,
                MaritalStatusId = contact.MaritalStatusId,
                GenderId = contact.GenderId,
                EmployerName = contact.EmployerName,
                AddressLine1 = contact.AddressLine1,
                AddressLine2 = contact.AddressLine2,
                City = contact.City,
                State = contact.State,
                PostalCode = contact.PostalCode,
                AnniversaryDate = contact.AnniversaryDate,
                ForeignCountry = contact.ForeignCountry,
                HomePhone = contact.HomePhone,
                CongregationId = contact.CongregationId,
                HouseholdId = contact.HouseholdId,
                AddressId = contact.AddressId
            };

            return person;
        }

        private List<Skill> GetSkills(int recordId, string token)
        {
            var attributes = GetMyRecords.GetMyAttributes(recordId, token);

            var skills =
                Mapper.Map<List<Attribute>, List<Skill>>(attributes);

            return skills;
        }

        public List<FamilyMember> GetMyImmediateFamily(int contactId, string token)
        {
            var contactRelationships = _contactRelationshipService.GetMyImmediatieFamilyRelationships(contactId, token).ToList();
            var familyMembers = Mapper.Map<List<Contact_Relationship>, List<FamilyMember>>(contactRelationships);

            //now get info for Contact
            var myProfile = GetLoggedInUserProfile(token);
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

        public List<ServingTeam> GetServingTeams(int contactId, string token)
        {
            var servingTeams = new List<ServingTeam>();

            //Go get family
            var familyMembers = GetMyImmediateFamily(contactId, token);
            foreach (var familyMember in familyMembers)
            {
                var groups = this._groupService.GetMyServingTeams(familyMember.ContactId, token);
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
                                LastName = familyMember.LastName
                            };
                            servingTeam.Members.Add(member);
                        }
                        var role = new ServeRole {Name = group.GroupRole};
                        member.Roles.Add(role);
                    }
                    else
                    {
                        servingTeam = new ServingTeam {Name = group.Name, GroupId = group.GroupId};
                        var groupMembers = new List<TeamMember> {NewTeamMember(familyMember, group)};
                        servingTeam.Members = groupMembers;
                        servingTeams.Add(servingTeam);
                    }
                }
            }
            return servingTeams;
        }

        private static TeamMember NewTeamMember(FamilyMember familyMember, Group group)
        {
            var teamMember = new TeamMember { ContactId = familyMember.ContactId, Name = familyMember.PreferredName, LastName = familyMember.LastName};

            var role = new ServeRole {Name = @group.GroupRole};
            teamMember.Roles = new List<ServeRole> {role};

            return teamMember;
        }

        private static TeamMember NewTeamMember(TeamMember teamMember, ServeRole role)
        {
            var newTeamMember2 = new TeamMember { Name = teamMember.Name, LastName = teamMember.LastName, ContactId = teamMember.ContactId };
            newTeamMember2.Roles.Add(role);

            return newTeamMember2;
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
                Members = NewTeamMembersWithRoles(team.Members, opportunity.RoleTitle, tmpRole)
            };
            return servingTeam;
        }

        public List<ServingDay> GetServingDays(List<ServingTeam> teams, string token)
        {
            var serveDays = new List<ServingDay>();

            foreach (var team in teams)
            {
                var opportunities = this._opportunityService.GetOpportunitiesForGroup(team.GroupId, token);

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
                            SlotsTaken =
                                this._opportunityService.GetOpportunitySignupCount(opportunity.OpportunityId, e.EventId,
                                    token)
                        };

                        var serveDay = serveDays.SingleOrDefault(r => r.Day == e.DateOnly);
                        if (serveDay != null)
                        {
                            //day already in list

                            //check if time is in list
                            var serveTime = serveDay.ServeTimes.SingleOrDefault(s => s.Time == e.TimeOnly);
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
                                                    LastName = teamMember.LastName
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
                                serveTime = new ServingTime {Time = e.TimeOnly};
                                serveTime.ServingTeams.Add(NewServingTeam(team, opportunity, serveRole));
                                serveDay.ServeTimes.Add(serveTime);
                            }
                        }
                        else
                        {
                            //day not in list add it
                            serveDay = new ServingDay {Day = e.DateOnly};
                            var serveTime = new ServingTime {Time = e.TimeOnly};
                            serveTime.ServingTeams.Add(NewServingTeam(team, opportunity, serveRole));

                            serveDay.ServeTimes.Add(serveTime);
                            serveDays.Add(serveDay);
                        }
                    }
                }
            }

            //sort everything for front end
            var preSortedServeDays = serveDays.OrderBy(s => s.Day).ToList();
            var sortedServeDays = new List<ServingDay>();
            foreach (var serveDay in preSortedServeDays)
            {
                var sortedServeDay = new ServingDay {Day = serveDay.Day};
                var sortedServeTimes = serveDay.ServeTimes.OrderBy(s => s.Time).ToList();
                sortedServeDay.ServeTimes = sortedServeTimes;
                sortedServeDays.Add(sortedServeDay);
            }

            return sortedServeDays;
        }

        private static List<ServeEvent> ParseServingEvents(IEnumerable<Event> events)
        {
            var today = DateTime.Now;
            return events.Select(e => new ServeEvent
            {
                Name = e.EventTitle,
                StarDateTime = e.EventStartDate,
                DateOnly = e.EventStartDate.Date.ToString("d"),
                TimeOnly = e.EventStartDate.TimeOfDay.ToString(),
                EventId = e.EventId
            })
                .Where(
                    e =>
                        e.StarDateTime.Month == today.Month && e.StarDateTime.Day >= today.Day &&
                        e.StarDateTime.Year == today.Year)
                .ToList();
        }
    }
}