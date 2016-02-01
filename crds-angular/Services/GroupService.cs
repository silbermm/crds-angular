using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Models.Crossroads.Opportunity;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Extensions;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Exceptions;
using MinistryPlatform.Translation.Services.Interfaces;
using Event = crds_angular.Models.Crossroads.Events.Event;
using IEventService = MinistryPlatform.Translation.Services.Interfaces.IEventService;
using IGroupService = MinistryPlatform.Translation.Services.Interfaces.IGroupService;

namespace crds_angular.Services
{
    public class GroupService : crds_angular.Services.Interfaces.IGroupService
    {
        private readonly ILog logger = LogManager.GetLogger(typeof (GroupService));

        private readonly IGroupService _mpGroupService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IEventService _eventService;
        private readonly IContactRelationshipService _contactRelationshipService;
        private readonly IServeService _serveService;
        

        /// <summary>
        /// This is retrieved in the constructor from AppSettings
        /// </summary>
        private readonly int GroupRoleDefaultId;

        public GroupService(IGroupService mpGroupService,
                            IConfigurationWrapper configurationWrapper,
                            IEventService eventService,
                            IContactRelationshipService contactRelationshipService,
                            IServeService serveService)
        {
            _mpGroupService = mpGroupService;
            _configurationWrapper = configurationWrapper;
            _eventService = eventService;
            _contactRelationshipService = contactRelationshipService;
            _serveService = serveService;
            GroupRoleDefaultId = Convert.ToInt32(_configurationWrapper.GetConfigIntValue("Group_Role_Default_ID"));
        }

        public void addParticipantsToGroup(int groupId, List<ParticipantSignup> participants)
        {
            Group g;

            try
            {
                g = _mpGroupService.getGroupDetails(groupId);
            }
            catch (Exception e)
            {
                var message = String.Format("Could not retrieve group details for group {0}: {1}", groupId, e.Message);
                logger.Error(message, e);
                throw (new ApplicationException(message, e));
            }

            var numParticipantsToAdd = participants.Count;
            var spaceRemaining = g.TargetSize - g.Participants.Count;
            if (((g.TargetSize > 0) && (numParticipantsToAdd > spaceRemaining)) || (g.Full))
            {
                throw (new GroupFullException(g));
            }

            try
            {
                foreach (var p in participants)
                {
                    // First sign this user up for the community group
                    int groupParticipantId = _mpGroupService.addParticipantToGroup(p.particpantId,
                                                                                   Convert.ToInt32(groupId),
                                                                                   GroupRoleDefaultId,
                                                                                   p.childCareNeeded,
                                                                                   DateTime.Now);
                    logger.Debug("Added user - group/participant id = " + groupParticipantId);

                    // Now see what future events are scheduled for this group, and register the user for those
                    var events = _mpGroupService.getAllEventsForGroup(Convert.ToInt32(groupId));
                    logger.Debug("Scheduled events for this group: " + events);
                    if (events != null && events.Count > 0)
                    {
                        foreach (var e in events)
                        {
                            _eventService.RegisterParticipantForEvent(p.particpantId, e.EventId, groupId, groupParticipantId);
                            logger.Debug("Added participant " + p + " to group event " + e.EventId);
                        }
                    }
                    var waitlist = g.GroupType == _configurationWrapper.GetConfigIntValue("GroupType_Waitlist");
                    _mpGroupService.SendCommunityGroupConfirmationEmail(p.particpantId, groupId, waitlist, p.childCareNeeded);
                }

                return;
            }
            catch (Exception e)
            {
                logger.Error("Could not add user to group", e);
                throw;
            }
        }

        public List<Event> GetGroupEvents(int groupId, string token)
        {
            var eventTypes = _mpGroupService.GetEventTypesForGroup(groupId, token);
            var events = new List<MinistryPlatform.Models.Event>();
            foreach (var eventType in eventTypes.Where(eventType => !string.IsNullOrEmpty(eventType)))
            {
                events.AddRange(_eventService.GetEvents(eventType, token));
            }
            var futureEvents = events.Where(e => e.EventStartDate >= DateTime.Now).OrderBy(e => e.EventStartDate);
            var eventList = Mapper.Map<List<Event>>(futureEvents.GroupBy(x => x.EventId).Select(y => y.First()));
            return eventList;
        }

        public List<GroupContactDTO> GetGroupMembersByEvent(int groupId, int eventId, string recipients)
        {
            return recipients == "current" ? GroupMembersThatAreServingForEvent(groupId, eventId) : GroupMembersThatHaveNotRepliedToEvent(groupId, eventId);
        }

        private List<GroupContactDTO> GroupMembersThatAreServingForEvent(int groupId, int eventId)
        {
            return _mpGroupService.getEventParticipantsForGroup(groupId, eventId)
                .Select(part => new GroupContactDTO
                {
                    ContactId = part.ContactId,
                    DisplayName = part.LastName + ", " + part.NickName
                }).ToList();
        }

        private List<GroupContactDTO> GroupMembersThatHaveNotRepliedToEvent(int groupId, int eventId)
        {
            try
            {
                var groupMembers = _mpGroupService.getGroupDetails(groupId).Participants.Select(p => 
                    new GroupParticipant {ContactId = p.ContactId, LastName = p.LastName, NickName = p.NickName, ParticipantId = p.ParticipantId}
                    ).ToList();
                var evt = Mapper.Map<crds_angular.Models.Crossroads.Events.Event>(_eventService.GetEvent(eventId));
                return _serveService.PotentialVolunteers(groupId, evt, groupMembers);                                
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
        }

        public GroupDTO getGroupDetails(int groupId, int contactId, Participant participant, string authUserToken)
        {
            int participantId = participant.ParticipantId;
            Group g = _mpGroupService.getGroupDetails(groupId);

            var signupRelations = _mpGroupService.GetGroupSignupRelations(g.GroupType);

            var currRelationships = _contactRelationshipService.GetMyCurrentRelationships(contactId, authUserToken);

            var events = _mpGroupService.getAllEventsForGroup(groupId);

            ContactRelationship[] familyToReturn = null;

            if (currRelationships != null)
            {
                familyToReturn = currRelationships.Where(
                    c => signupRelations.Select(s => s.RelationshipId).Contains(c.Relationship_Id)).ToArray();
            }

            var detail = new GroupDTO();
            {
                detail.GroupName = g.Name;
                detail.GroupId = g.GroupId;
                detail.GroupFullInd = g.Full;
                detail.WaitListInd = g.WaitList;
                detail.ChildCareAvailable = g.ChildCareAvailable;
                detail.WaitListGroupId = g.WaitListGroupId;
                detail.OnlineRsvpMinimumAge = g.MinimumAge;
                if (events != null)
                {
                    detail.Events = events.Select(Mapper.Map<MinistryPlatform.Models.Event, crds_angular.Models.Crossroads.Events.Event>).ToList();
                }
                //the first instance of family must always be the logged in user
                var fam = new SignUpFamilyMembers
                {
                    EmailAddress = participant.EmailAddress,
                    PreferredName = participant.PreferredName,
                    UserInGroup = _mpGroupService.checkIfUserInGroup(participantId, g.Participants),
                    ParticpantId = participantId,
                    ChildCareNeeded = false
                };
                detail.SignUpFamilyMembers = new List<SignUpFamilyMembers> {fam};

                if (familyToReturn != null)
                {
                    foreach (var f in familyToReturn)
                    {
                        var fm = new SignUpFamilyMembers
                        {
                            EmailAddress = f.Email_Address,
                            PreferredName = f.Preferred_Name,
                            UserInGroup = _mpGroupService.checkIfUserInGroup(f.Participant_Id, g.Participants),
                            ParticpantId = f.Participant_Id,
                        };
                        detail.SignUpFamilyMembers.Add(fm);
                    }
                }
            }

            return (detail);
        }
    }
}