using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using AutoMapper;
using crds_angular.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Serve;
using Microsoft.Ajax.Utilities;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services;
using Attribute = MinistryPlatform.Models.Attribute;
using Event = MinistryPlatform.Models.Event;
using Response = crds_angular.Models.Crossroads.Response;
using ServingTeam = crds_angular.Models.Crossroads.Serve.ServingTeam;

namespace crds_angular.Services
{
    public class PersonService : MinistryPlatformBaseService
    {
        public void setProfile(String token, Person person)
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

        public List<Skill> getLoggedInUserSkills(int contactId, string token)
        {
            return GetSkills(contactId, token);
        }

        public Person getLoggedInUserProfile(String token)
        {
            var contact = MinistryPlatformService.GetRecordsArr(AppSetting("MyProfile"), token);
            if (contact.Count == 0)
            {
                throw new InvalidOperationException("getLoggedInUserProfile - no data returned.");
            }
            var contactJson = TranslationService.DecodeJson(contact.ToString());

            var person = new Person
            {
                Contact_Id = contactJson.Contact_Id,
                Email_Address = contactJson.Email_Address,
                NickName = contactJson.Nickname,
                First_Name = contactJson.First_Name,
                Middle_Name = contactJson.Middle_Name,
                Last_Name = contactJson.Last_Name,
                Maiden_Name = contactJson.Maiden_Name,
                Mobile_Phone = contactJson.Mobile_Phone,
                Mobile_Carrier = contactJson.Mobile_Carrier_ID,
                Date_of_Birth = contactJson.Date_of_Birth,
                Marital_Status_Id = contactJson.Marital_Status_ID,
                Gender_Id = contactJson.Gender_ID,
                Employer_Name = contactJson.Employer_Name,
                Address_Line_1 = contactJson.Address_Line_1,
                Address_Line_2 = contactJson.Address_Line_2,
                City = contactJson.City,
                State = contactJson.State,
                Postal_Code = contactJson.Postal_Code,
                Anniversary_Date = contactJson.Anniversary_Date,
                Foreign_Country = contactJson.Foreign_Country,
                County = contactJson.County,
                Home_Phone = contactJson.Home_Phone,
                Congregation_ID = contactJson.Congregation_ID,
                Household_ID = contactJson.Household_ID,
                Address_Id = contactJson.Address_ID
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

        public List<FamilyMember> GetMyFamily(int contactId, string token)
        {
            var contactRelationships = GetMyRecords.GetMyFamily(contactId, token).ToList();
            var familyMembers = Mapper.Map<List<Contact_Relationship>, List<FamilyMember>>(contactRelationships);

            //now get info for Contact
            var personService = new PersonService();
            var myProfile = personService.getLoggedInUserProfile(token);
            var me = new FamilyMember
            {
                ContactId = myProfile.Contact_Id,
                Email = myProfile.Email_Address,
                LastName = myProfile.Last_Name,
                PreferredName = myProfile.NickName ?? myProfile.First_Name
            };
            familyMembers.Add(me);

            return familyMembers;
        }

        public List<ServingTeam> GetMyFamiliesServingTeams(int contactId, string token)
        {
            var personService = new PersonService();
            var servingTeams = new List<ServingTeam>();

            //now go get family
            var familyMembers = personService.GetMyFamily(contactId, token);
            foreach (var familyMember in familyMembers)
            {
                var groups = GetMyRecords.GetMyServingTeams(familyMember.ContactId, token);
                foreach (var group in groups)
                {
                    if (servingTeams.Any(s => s.GroupId == group.GroupId))
                    {
                        var s = servingTeams.Single(t => t.GroupId == group.GroupId);

                        //is this person already on team?
                        if (s.Members.Any(x => x.ContactId == familyMember.ContactId))
                        {
                            //person found on team
                            var member = s.Members.Single(q => q.ContactId == familyMember.ContactId);
                            var roleName = group.GroupRole;
                            var role = new ServeRole {Name = roleName};
                            member.Roles.Add(role);
                        }
                        else
                        {
                            s.Members.Add(NewTeamMember(familyMember, group));
                        }
                    }
                    else
                    {
                        var servingTeam = new ServingTeam {Name = @group.GroupName, GroupId = @group.GroupId};
                        var groupMembers = new List<TeamMember> {NewTeamMember(familyMember, @group)};
                        servingTeam.Members = groupMembers;
                        servingTeams.Add(servingTeam);
                    }
                }
            }
            return servingTeams;
        }

        private static TeamMember NewTeamMember(FamilyMember familyMember, Group group)
        {
            var teamMember = new TeamMember {ContactId = familyMember.ContactId, Name = familyMember.PreferredName};

            var role = new ServeRole {Name = @group.GroupRole};
            teamMember.Roles = new List<ServeRole> {role};

            return teamMember;
        }

        private static TeamMember NewTeamMember(TeamMember teamMember, ServeRole role)
        {
            var newTeamMember2 = new TeamMember {Name = teamMember.Name, ContactId = teamMember.ContactId};
            newTeamMember2.Roles.Add(role);

            return newTeamMember2;
        }

        private static List<TeamMember> NewTeamMembersWithRoles(List<TeamMember> teamMembers, string opportunityRoleTitle, ServeRole teamRole)
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
            //var x = servingTeam;
            return servingTeam;
        }

        public List<ServingDay> GetMyFamiliesServingEvents(List<ServingTeam> teams, string token)
        {
            var serveDays = new List<ServingDay>();

            foreach (var team in teams)
            {
                var newTeam = new ServingTeam();
                newTeam.Name = team.Name;

                var opportunities = OpportunityService.GetOpportunitiesForGroup(team.GroupId, token);

                foreach (var opportunity in opportunities)
                {
                    if (opportunity.EventType == null) continue;

                    //hold role for later
                    var tmpRole = new ServeRole {Name = opportunity.OpportunityName + " " + opportunity.RoleTitle};

                    //team events
                    var events = ParseTmEvents(opportunity.Events);

                    foreach (var e in events)
                    {
                        if (serveDays.Any(r => r.Day == e.DateOnly))
                        {
                            //day already in list

                            //check if time is in list
                            var serveDay = serveDays.Single(r => r.Day == e.DateOnly);
                            if ((serveDay.ServeTimes.Any(s => s.Time == e.TimeOnly)))
                            {
                                //time in list
                                var serveTime = serveDay.ServeTimes.Single(s => s.Time == e.TimeOnly);
                                //is team already in list??
                                if ((serveTime.ServingTeams != null) &&
                                    serveTime.ServingTeams.Any(t => t.GroupId == team.GroupId))
                                {
                                    //team exists
                                    var existingTeam = serveTime.ServingTeams.Single(t => t.GroupId == team.GroupId);
                                    foreach (var teamMember in team.Members)
                                    {
                                        foreach (var role in teamMember.Roles)
                                        {
                                            if (role.Name != opportunity.RoleTitle) continue;

                                            //need to add this to existing member
                                            if ((existingTeam.Members != null) &&
                                                (existingTeam.Members.Any(
                                                    m => m.ContactId == teamMember.ContactId)))
                                            {
                                                var member =
                                                    existingTeam.Members.Single(
                                                        m => m.ContactId == teamMember.ContactId);
                                                member.Roles.Add(tmpRole);
                                            }
                                            else
                                            {
                                                existingTeam.Members.Add(NewTeamMember(teamMember, tmpRole));
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //team not in list
                                    //var team2 = new ServingTeam
                                    //{
                                    //    Name = team.Name,
                                    //    GroupId = team.GroupId,
                                    //    Members = NewTeamMembersWithRoles(team.Members,opportunity.RoleTitle,tmpRole)
                                    //};
                                    serveTime.ServingTeams.Add(NewServingTeam(team, opportunity, tmpRole));
                                }
                            }
                            else
                            {
                                //time not in list
                                var serveTime = new ServingTime {Time = e.TimeOnly};

                                //need to add members and roles here!
                                //var team2 = new ServingTeam
                                //{
                                //    Name = team.Name,
                                //    GroupId = team.GroupId,
                                //    Members = NewTeamMembersWithRoles(team.Members, opportunity.RoleTitle, tmpRole)
                                //};
                                serveTime.ServingTeams.Add(NewServingTeam(team, opportunity, tmpRole));
                                serveDay.ServeTimes.Add(serveTime);
                            }
                        }
                        else
                        {
                            //day not in list add it
                            var serveDay = new ServingDay {Day = e.DateOnly};
                            var serveTime = new ServingTime {Time = e.TimeOnly};
                            serveTime.ServingTeams.Add(NewServingTeam(team, opportunity, tmpRole));

                            //if (serveDay.ServeTimes == null)
                            //{
                            //    serveDay.ServeTimes = new List<ServingTime>();
                            //}
                            serveDay.ServeTimes.Add(serveTime);
                            serveDays.Add(serveDay);
                        }
                    }
                }
            }

            //might be a better place or way to sort this above instead here
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

        

        public List<Models.Crossroads.ServingTeam> GetServingOpportunities(int contactId, string token)
        {
            var groups = GetMyRecords.GetMyServingTeams(contactId, token);
            var teams = new List<Models.Crossroads.ServingTeam>();
            foreach (var group in groups)
            {
                var team = new Models.Crossroads.ServingTeam();
                team.TeamName = group.GroupName;
                var opportunities = OpportunityService.GetOpportunitiesForGroup(group.GroupId, token);
                foreach (var opp in opportunities)
                {
                    var opportunity = new GroupOpportunity();
                    opportunity.OpportunityName = opp.OpportunityName;
                    opportunity.ServeOccurances = ParseEvents(opp.Events);
                    //opportunity.OpportunityDateTime = opp.Opportunity_Date;
                    var response = OpportunityService.GetMyOpportunityResponses(contactId, opp.OpportunityId, token);
                    if (response != null)
                    {
                        opportunity.Rsvp = new ServingRSVP
                        {
                            //Occurrence = response.Opportunity_Date,
                            Response = ParseResponseResult(response)
                        };
                    }

                    team.Opportunities.Add(opportunity);
                }
                teams.Add(team);
            }
            return teams;
        }

        private static List<ServeEvent> ParseTmEvents(IEnumerable<Event> events)
        {
            return events.Select(e => new ServeEvent
            {
                Name = e.EventTitle,
                StarDateTime = e.EventStartDate,
                DateOnly = e.EventStartDate.Date.ToString("d"),
                TimeOnly = e.EventStartDate.TimeOfDay.ToString(),
                EventId = e.EventID
            }).ToList();
        }

        private static List<ServeOccurance> ParseEvents(IEnumerable<Event> events)
        {
            return
                events.Select(e => new ServeOccurance {Name = e.EventTitle, StarDateTime = e.EventStartDate}).ToList();
        }

        private static Response ParseResponseResult(MinistryPlatform.Models.Response response)
        {
            switch (response.Response_Result_ID)
            {
                case 1:
                    //Placed
                    return Response.Yes;
                case 2:
                    //Not Placed
                    return Response.No;
                case null:
                    //Maybe?
                    return Response.Maybe;
                default:
                    throw new ApplicationException("Invalid Opportunity Response Result.");
            }
        }
    }
}

//public class tmServingTeam2
//{
//    [JsonProperty(PropertyName = "name")]
//    public string Name { get; set; }

//    [JsonProperty(PropertyName = "groupId")]
//    public int GroupId { get; set; }

//    [JsonProperty(PropertyName = "members")]
//    public List<TmTeamMember> Members { get; set; }

//    //[JsonProperty(PropertyName = "eventTypes")]
//    //public List<tmEventType> EventTypes { get; set; }

//    [JsonProperty(PropertyName = "events")]
//    public List<tmServeEvent> Events { get; set; }


//    [JsonProperty(PropertyName = "roles")]
//    public List<tmRole> Roles { get; set; }
//}