using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using AutoMapper;
using crds_angular.Models;
using crds_angular.Models.Crossroads;
using Microsoft.Ajax.Utilities;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services;
using Newtonsoft.Json;
using Attribute = MinistryPlatform.Models.Attribute;
using Event = MinistryPlatform.Models.Event;
using Response = crds_angular.Models.Crossroads.Response;

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

        public List<FamilyMember> GetMyFamily(int recordId, string token)
        {
            var contactRelationships = GetMyRecords.GetMyFamily(recordId, token).ToList();
            var familyMembers = Mapper.Map<List<Contact_Relationship>, List<FamilyMember>>(contactRelationships);

            var personService = new PersonService();
            //probaby a better way, this is just a test
            var loggedInUserProfile = personService.getLoggedInUserProfile(token);
            var me = new FamilyMember();
            me.ContactId = loggedInUserProfile.Contact_Id;
            me.Email = loggedInUserProfile.Email_Address;
            me.LastName = loggedInUserProfile.Last_Name;
            me.PreferredName = "";
            familyMembers.Add(me);

            return familyMembers;
        }

        public List<tmServingTeam> GetMeMyFamilysServingStuff(string token)
        {
            var personService = new PersonService();
            //probaby a better way, this is just a test
            var loggedInUserProfile = personService.getLoggedInUserProfile(token);

            var servingTeams = new List<tmServingTeam>();
            var myTeams = GetMyRecords.GetMyServingTeams(loggedInUserProfile.Contact_Id, token);

            foreach (var team in myTeams)
            {
                //if team already in servingTeams, just add role
                if (servingTeams.Any(s => s.GroupId == team.GroupId))
                {
                    var tmp = team;
                    //why this?
                    var s = servingTeams.Single(t => t.GroupId == tmp.GroupId);
                    var roleName = string.Format("{0} {1}", tmp.GroupName, tmp.GroupRole);
                    roleName = tmp.GroupRole;
                    s.Members[0].Roles.Add(new tmRole { Name = roleName });
                }
                else
                {
                    var servingTeam = new tmServingTeam();
                    servingTeam.Name = team.GroupName;
                    servingTeam.GroupId = team.GroupId;


                    //if person already in servingTeam, just add role


                    var groupMembers = new List<TmTeamMember>();
                    var groupMember = new TmTeamMember();
                    groupMember.ContactId = loggedInUserProfile.Contact_Id;
                    groupMember.Name = loggedInUserProfile.First_Name;


                    var role = new tmRole();
                    role.Name = string.Format("{0} {1}", team.GroupName, team.GroupRole);
                    role.Name = team.GroupRole;
                    groupMember.Roles = new List<tmRole> {role};

                    groupMembers.Add(groupMember);
                    servingTeam.Members = groupMembers;
                    servingTeams.Add(servingTeam);
                }

                //get all catch all roles for team
            }


            //now go get family
            var familyMembers = personService.GetMyFamily(loggedInUserProfile.Contact_Id, token);
            foreach (var familyMember in familyMembers)
            {
                var groups = GetMyRecords.GetMyServingTeams(familyMember.ContactId, token);
                foreach (var group in groups)
                {
                    if (servingTeams.Any(s => s.GroupId == group.GroupId))
                    {
                        var tmp = group;
                        //why this?
                        var s = servingTeams.Single(t => t.GroupId == tmp.GroupId);

                        //is this person already on team?
                        if (s.Members.Any(x => x.ContactId == familyMember.ContactId))
                        {
                            //found a match
                            var member = s.Members.Single(q => q.ContactId == familyMember.ContactId);
                            var roleName = string.Format("{0} {1}", tmp.GroupName, tmp.GroupRole);
                            roleName = tmp.GroupRole;
                            var role = new tmRole {Name = roleName};
                            member.Roles.Add(role);
                        }
                        else
                        {
                            var groupMember = new TmTeamMember();
                            groupMember.ContactId = familyMember.ContactId;
                            groupMember.Name = familyMember.PreferredName;

                            var role = new tmRole();
                            role.Name = string.Format("{0} {1}", tmp.GroupName, tmp.GroupRole);
                            role.Name = tmp.GroupRole;
                            groupMember.Roles = new List<tmRole> {role};

                            s.Members.Add(groupMember);
                        }
                    }
                    else
                    {
                        var servingTeam = new tmServingTeam();
                        servingTeam.Name = group.GroupName;
                        servingTeam.GroupId = group.GroupId;

                        var groupMembers = new List<TmTeamMember>();
                        var groupMember = new TmTeamMember();
                        groupMember.ContactId = familyMember.ContactId;
                        groupMember.Name = familyMember.PreferredName;
                        groupMembers.Add(groupMember);
                        servingTeam.Members = groupMembers;
                        servingTeams.Add(servingTeam);
                    }
                }
            }
            return servingTeams;
        }

        public List<tmServeDay> GetEventsStuff(List<tmServingTeam> teams, string token)
        {
            //var viewId = 623;
            //var tmp1 = MinistryPlatform.Translation.Services.MinistryPlatformService.GetPageViewRecords(viewId, token);

            var newTeams = new List<tmServingTeam2>();

            //var returnServeEvents = new List<tmServeEvent>();

            var serveDays = new List<tmServeDay>();

            foreach (var team in teams)
            {
                var newTeam = new tmServingTeam2();
                newTeam.Name = team.Name;

                var opportunities = OpportunityService.GetOpportunitiesForGroup(team.GroupId, token);
                //var x = opportunities[0].EventType;

                //how do i know if team member has opportunity?  
                //assumption one opportunity per role, except for catch all

                foreach (var opportunity in opportunities)
                {
                    if (opportunity.EventType != null)
                    {
                        //is this where we need to be adding roles???
                        //roles
                        var tmpRole = new tmRole();
                        tmpRole.Name = opportunity.OpportunityName + " " + opportunity.RoleTitle;
                        

                        
                        //end roles


                        //team events
                        var events = ParseTmEvents(opportunity.Events);


                        var newEventsList = new List<tmServeEvent>();

                        foreach (var e in events)
                        {
                            if (serveDays.Any(r => r.ServeDay == e.DateOnly))
                            {
                                //day already in list

                                //check if time is in list
                                var serveDay = serveDays.Single(r => r.ServeDay == e.DateOnly);
                                if ((serveDay.ServeTimes != null) &&  (serveDay.ServeTimes.Any(s => s.ServeTime == e.TimeOnly)))
                                {
                                    //time in list
                                    var serveTime = serveDay.ServeTimes.Single(s => s.ServeTime == e.TimeOnly);
                                    if (serveTime.ServingTeams == null)
                                    {
                                        serveTime.ServingTeams = new List<tmServingTeam>();
                                    }
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
                                                if (role.Name == opportunity.RoleTitle)
                                                {

                                                    var newTeamMember2 = new TmTeamMember();
                                                    newTeamMember2.Name = teamMember.Name;
                                                    newTeamMember2.ContactId = teamMember.ContactId;
                                                    if (newTeamMember2.Roles == null)
                                                    {
                                                        newTeamMember2.Roles = new List<tmRole>();
                                                    }
                                                    newTeamMember2.Roles.Add(tmpRole);

                                                    //need to add this to existing member
                                                    if ((existingTeam.Members != null) &&
                                                        (existingTeam.Members.Any(
                                                            m => m.ContactId == newTeamMember2.ContactId)))
                                                    {
                                                        var member =
                                                            existingTeam.Members.Single(
                                                                m => m.ContactId == newTeamMember2.ContactId);
                                                        member.Roles.Add(tmpRole);
                                                    }
                                                    else
                                                    {
                                                        if (existingTeam.Members == null)
                                                        {
                                                            existingTeam.Members = new List<TmTeamMember>();
                                                        }
                                                        existingTeam.Members.Add(newTeamMember2);
                                                    }

                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        //team not in list
                                        //need to add members and roles here!
                                        if (serveTime.ServingTeams == null)
                                        {
                                            serveTime.ServingTeams = new List<tmServingTeam>();
                                        }
                                        var team2 = new tmServingTeam();
                                        team2.Name = team.Name;
                                        team2.GroupId = team.GroupId;
                                        team2.Members = new List<TmTeamMember>();

                                        foreach (var teamMember in team.Members)
                                        {
                                            foreach (var role in teamMember.Roles)
                                            {
                                                if (role.Name == opportunity.RoleTitle)
                                                {

                                                    var newTeamMember2 = new TmTeamMember();
                                                    newTeamMember2.Name = teamMember.Name;
                                                    newTeamMember2.ContactId = teamMember.ContactId;
                                                    if (newTeamMember2.Roles == null)
                                                    {
                                                        newTeamMember2.Roles = new List<tmRole>();
                                                    }
                                                    newTeamMember2.Roles.Add(tmpRole);
                                                    team2.Members.Add(newTeamMember2);
                                                }
                                            }
                                        }
                                        serveTime.ServingTeams.Add(team2);

                                    }
                                }
                                else
                                {
                                    //time not in list
                                    var serveTime = new tmServeTime();
                                    serveTime.ServeTime = e.TimeOnly;

                                    //need to add members and roles here!
                                    if (serveTime.ServingTeams == null)
                                    {
                                        serveTime.ServingTeams = new List<tmServingTeam>();
                                    }
                                    var team2 = new tmServingTeam();
                                    team2.Name = team.Name;
                                    team2.GroupId = team.GroupId;
                                    team2.Members = new List<TmTeamMember>();

                                    foreach (var teamMember in team.Members)
                                    {
                                        foreach (var role in teamMember.Roles)
                                        {
                                            if (role.Name == opportunity.RoleTitle)
                                            {

                                                var newTeamMember2 = new TmTeamMember();
                                                newTeamMember2.Name = teamMember.Name;
                                                newTeamMember2.ContactId = teamMember.ContactId;
                                                if (newTeamMember2.Roles == null)
                                                {
                                                    newTeamMember2.Roles = new List<tmRole>();
                                                }
                                                newTeamMember2.Roles.Add(tmpRole);
                                                team2.Members.Add(newTeamMember2);
                                            }
                                        }
                                    }
                                    serveTime.ServingTeams.Add(team2);

                                    if (serveDay.ServeTimes == null)
                                    {
                                        serveDay.ServeTimes = new List<tmServeTime>();
                                    }
                                    serveDay.ServeTimes.Add(serveTime);
                                }
                            }
                            else
                            {
                                //day not in list add it
                                var serveDay = new tmServeDay();
                                serveDay.ServeDay = e.DateOnly;
                                var serveTime = new tmServeTime();
                                serveTime.ServeTime = e.TimeOnly;

                                if (serveTime.ServingTeams == null)
                                {
                                    serveTime.ServingTeams = new List<tmServingTeam>();
                                }

                                //does the team already exists in new list?
                                //if (serveDays.Any(r => r.ServeDay == e.DateOnly))


                                var team2 = new tmServingTeam();
                                team2.Name = team.Name;
                                team2.GroupId = team.GroupId;
                                team2.Members = new List<TmTeamMember>();

                                foreach (var teamMember in team.Members)
                                {
                                    foreach (var role in teamMember.Roles)
                                    {
                                        if (role.Name == opportunity.RoleTitle)
                                        {

                                            var newTeamMember2 = new TmTeamMember();
                                            newTeamMember2.Name = teamMember.Name;
                                            newTeamMember2.ContactId = teamMember.ContactId;
                                            if (newTeamMember2.Roles == null)
                                            {
                                                newTeamMember2.Roles=new List<tmRole>();
                                            }
                                            newTeamMember2.Roles.Add(tmpRole);
                                            team2.Members.Add(newTeamMember2);
                                        }
                                    }
                                }
                                serveTime.ServingTeams.Add(team2);

                                if (serveDay.ServeTimes == null)
                                {
                                    serveDay.ServeTimes = new List<tmServeTime>();
                                }
                                serveDay.ServeTimes.Add(serveTime);
                                serveDays.Add(serveDay);


                                //now what?
                                //do i need to find every team from my list that has event on this day?
                                //feels like i'm reading database again??
                            }


                            //if ((newTeam.Events != null) && (newTeam.Events.Any(w => w.EventId == e.EventId)))
                            //{
                                
                            //}
                            //else
                            //{

                            //    newEventsList.Add(e);
                            //}
                        }

                        //if (newTeam.Events == null)
                        //{
                        //    newTeam.Events = new List<tmServeEvent>();
                        //}

                        //newTeam.Events.AddRange(newEventsList);
                        //teamEvents end
                        
                    }
                }
                newTeams.Add(newTeam);
            }


            //Console.Write(newTeams);

            //might be a better place or way to sort this above instead here

            var preSortedServeDays = serveDays.OrderBy(s => s.ServeDay).ToList();
            var sortedServeDays = new List<tmServeDay>();
            foreach (var serveDay in preSortedServeDays)
            {
                var sortedServeDay = new tmServeDay();
                sortedServeDay.ServeDay = serveDay.ServeDay;

                var sortedServeTimes = serveDay.ServeTimes.OrderBy(s => s.ServeTime).ToList();

                sortedServeDay.ServeTimes = sortedServeTimes;

                sortedServeDays.Add(sortedServeDay);
            }


            return sortedServeDays;
            //return newTeams;
        }

        public List<ServingTeam> GetServingOpportunities(int contactId, string token)
        {
            var groups = GetMyRecords.GetMyServingTeams(contactId, token);
            var teams = new List<ServingTeam>();
            foreach (var group in groups)
            {
                var team = new ServingTeam();
                team.TeamName = group.GroupName;
                var opportunities = OpportunityService.GetOpportunitiesForGroup(group.GroupId, token);
                foreach (var opp in opportunities)
                {
                    var opportunity = new ServingOpportunity();
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

        private static List<tmServeEvent> ParseTmEvents(IEnumerable<Event> events)
        {
            return events.Select(e => new tmServeEvent
            {
                Name = e.EventTitle,
                StarDateTime = e.EventStartDate,
                DateOnly = e.EventStartDate.Date.ToString("d"),
                TimeOnly = e.EventStartDate.TimeOfDay.ToString(), EventId = e.EventID
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

public class tmServingTeam
{
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "groupId")]
    public int GroupId { get; set; }

    [JsonProperty(PropertyName = "members")]
    public List<TmTeamMember> Members { get; set; }

    //[JsonProperty(PropertyName = "eventTypes")]
    //public List<tmEventType> EventTypes { get; set; }

    [JsonProperty(PropertyName = "events")]
    public List<tmServeEvent> Events { get; set; }
}

public class tmServingTeam2
{
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "groupId")]
    public int GroupId { get; set; }

    [JsonProperty(PropertyName = "members")]
    public List<TmTeamMember> Members { get; set; }

    //[JsonProperty(PropertyName = "eventTypes")]
    //public List<tmEventType> EventTypes { get; set; }

    [JsonProperty(PropertyName = "events")]
    public List<tmServeEvent> Events { get; set; }


    [JsonProperty(PropertyName = "roles")]
    public List<tmRole> Roles { get; set; }
}

public class TmTeamMember
{
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "contactId")]
    public int ContactId { get; set; }

    [JsonProperty(PropertyName = "roles")]
    public List<tmRole> Roles { get; set; }
}

public class tmRole
{
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }
}

public class tmEventType
{
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "eventId")]
    public int EventId { get; set; }
}

public class tmServeEvent
{
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "startDateTime")]
    public DateTime StarDateTime { get; set; }

    public string DateOnly { get; set; }
    public string TimeOnly { get; set; }

    public int EventId { get; set; }
}

public class tmServeDay
{
    public string ServeDay { get; set; }
    public List<tmServeTime> ServeTimes { get; set; } 
}

public class tmServeTime
{
    public string ServeTime { get; set; }
    public List<tmServingTeam> ServingTeams {get; set; }
}