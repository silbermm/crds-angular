using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Groups;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Exceptions;
using MinistryPlatform.Translation.Services.Interfaces;
using Event = crds_angular.Models.Crossroads.Events.Event;

namespace crds_angular.Services
{
    public class GroupService : crds_angular.Services.Interfaces.IGroupService
    {
        private readonly ILog logger = LogManager.GetLogger(typeof (GroupService));

        private IGroupService _mpGroupService;
        private IConfigurationWrapper _configurationWrapper;
        private IAuthenticationService _authenticationService;
        private IEventService _eventService;
        private IContactRelationshipService _contactRelationshipService;

        /// <summary>
        /// This is retrieved in the constructor from AppSettings
        /// </summary>
        private readonly int GroupRoleDefaultId;

        public GroupService(IGroupService mpGroupService,
                            IConfigurationWrapper configurationWrapper,
                            IAuthenticationService authenticationService,
                            IEventService eventService,
                            IContactRelationshipService contactRelationshipService)
        {
            this._mpGroupService = mpGroupService;
            this._configurationWrapper = configurationWrapper;
            this._authenticationService = authenticationService;
            this._eventService = eventService;
            this._contactRelationshipService = contactRelationshipService;

            this.GroupRoleDefaultId = Convert.ToInt32(_configurationWrapper.GetConfigIntValue("Group_Role_Default_ID"));
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
                            int eventParticipantId = _eventService.RegisterParticipantForEvent(p.particpantId, e.EventId, groupId, groupParticipantId);
                            logger.Debug("Added participant " + p + " to group event " + e.EventId);
                        }
                    }
                    _mpGroupService.SendCommunityGroupConfirmationEmail(p.particpantId, groupId, p.childCareNeeded);
                }

                return;
            }
            catch (Exception e)
            {
                logger.Error("Could not add user to group", e);
                throw (e);
            }
        }

        public List<Event> GetGroupEvents(int groupId)
        {
            var events = _mpGroupService.getAllEventsForGroup(groupId);
            var eventList = AutoMapper.Mapper.Map<List<Event>>(events);
            return eventList;
        }

        public List<GroupContactDTO> GetGroupMembersByEvent(int groupId, int eventId)
        {
            var participants = _mpGroupService.getEventParticipantsForGroup(groupId, eventId);
            var members = participants.Select(part => new GroupContactDTO
            {
                ContactId = part.ContactId,
                DisplayName = part.LastName + ", " + part.NickName
            }).ToList();
            return members;
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
                detail.Events = events.Select(Mapper.Map<MinistryPlatform.Models.Event, crds_angular.Models.Crossroads.Events.Event>).ToList();
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