using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using System.Web.Razor.Generator;
using System.Web.UI.WebControls.WebParts;
using crds_angular.Models.Crossroads;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using log4net;
using log4net.DateFormatter;
using log4net.Util;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Exceptions;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class GroupController : MPAuth
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IGroupService groupService;
        private IEventService eventService;
        private IAuthenticationService authenticationService;
        private IContactRelationshipService contactRelationshipService;
        private IPersonService personService;

        private readonly int GroupRoleDefaultId =
            Convert.ToInt32(ConfigurationManager.AppSettings["Group_Role_Default_ID"]);

        public GroupController(IGroupService groupService, IEventService eventService,
            IAuthenticationService authenticationService, IContactRelationshipService contactRelationshipService, IPersonService personService)
        {
            this.groupService = groupService;
            this.eventService = eventService;
            this.authenticationService = authenticationService;
            this.contactRelationshipService = contactRelationshipService;
            this.personService = personService;
        }

        /**
         * Enroll the currently logged-in user into a Community Group, and also register this user for all events for the CG.
         */

        [ResponseType(typeof (GroupDTO))]
        [Route("api/group/{groupId}/user")]
        public IHttpActionResult Post(String groupId)
        {
            return Authorized(token =>
            {
                try
                {
                    //remove the two fields below.  particpant IDs will be passed in 
                    var participant = authenticationService.GetParticipantRecord(token);
                    int participantId = participant.ParticipantId;

                    //always return the loggedin user as the first relationship
                    // First sign this user up for the community group
                    int groupParticipantId = groupService.addParticipantToGroup(participantId, Convert.ToInt32(groupId),
                        GroupRoleDefaultId, DateTime.Now);
                    logger.Debug("Added user - group/participant id = " + groupParticipantId);
                    var response = new Dictionary<string, object>
                    {
                        {"success", true},
                        {"participantId", participantId},
                        {"groupParticipantId", groupParticipantId}
                    };
                    var enrolledEvents = new List<string>();
                    response.Add("enrolledEvents", enrolledEvents);

                    // Now see what future events are scheduled for this group, and register the user for those
                    var events = groupService.getAllEventsForGroup(Convert.ToInt32(groupId));
                    logger.Debug("Scheduled events for this group: " + events);
                    if (events != null && events.Count > 0)
                    {
                        foreach (var e in events)
                        {
                            int eventParticipantId = eventService.registerParticipantForEvent(participantId, e.EventId);
                            logger.Debug("Added participant " + participantId + " to group event " + e.EventId);
                            enrolledEvents.Add(Convert.ToString(e.EventId));
                        }
                    }

                    return Ok(response);
                }
                catch (GroupFullException e)
                {
                    var response = new ModelStateDictionary();
                    response.AddModelError("error", "GroupIsFull");
                    return (BadRequest(response));
                }
                catch (Exception e)
                {
                    logger.Error("Could not add user to group", e);
                    return BadRequest();
                }
            });
        }

        // TODO: implement later
        [ResponseType(typeof (GroupDTO))]
        [Route("api/group/{groupId}/users")]
        public IHttpActionResult Post(String groupId, [FromBody] List<ContactDTO> contact)
        {
            throw new NotImplementedException();
            return this.Ok();
        }
        
        [ResponseType(typeof(GroupDTO))]
        [Route("api/group/{groupId}")]
        public IHttpActionResult Get(int groupId)
        {
            return Authorized(token =>
            {
                var participant = authenticationService.GetParticipantRecord(token);
                int participantId = participant.ParticipantId;
                var contactId = authenticationService.GetContactId(token);
               
                Group g = groupService.getGroupDetails(groupId);

                var signupRelations = groupService.GetGroupSignupRelations(g.GroupType, token);
            
                var currRelationships = contactRelationshipService.GetMyCurrentRelationships(contactId, token);

                Contact_Relationship[] familyToReturn =  null;
              
                if (currRelationships != null)
                {
                  familyToReturn =  currRelationships.Where(
                        c => signupRelations.Select(s => s.RelationshipId).Contains(c.Relationship_Id)).ToArray();
                }
                
                var detail = new GroupDTO();
                {
                    detail.GroupId = g.GroupId;
                    detail.GroupFullInd = g.Full;
                    detail.WaitListInd = g.WaitList;
                    detail.WaitListGroupId = g.WaitListGroupId;

                    //the first instance of family must always be the logged in user
                    var fam = new SignUpFamilyMembers
                    {
                        EmailAddress = participant.EmailAddress,
                        FirstName = participant.PreferredName,
                        UserInGroup = groupService.checkIfUserInGroup(participantId, g.Participants),
                        ParticpantId = participantId,
                    };
                    detail.SignUpFamilyMembers = new List<SignUpFamilyMembers> {fam};

                    if (familyToReturn != null)
                    {
                        foreach (var f in familyToReturn)
                        {
                            var fm = new SignUpFamilyMembers
                            {
                                EmailAddress = f.Email_Address,
                                FirstName = f.Preferred_Name,
                                UserInGroup = groupService.checkIfUserInGroup(f.Participant_Id, g.Participants),
                                ParticpantId = f.Participant_Id,
                            };
                            detail.SignUpFamilyMembers.Add(fm); 
                        }
                     }
                }

                return Ok(detail);
            }
          );
        }

        // TODO: implement later
        [ResponseType(typeof (ContactDTO))]
        [Route("api/group/{groupId}/user/{userId}")]
        public IHttpActionResult Get(String groupId, String userId)
        {
            throw new NotImplementedException();
            return this.Ok();
        }

        // TODO: implement later
        [ResponseType(typeof (GroupDTO))]
        [Route("api/group/{groupId}/user/{userId}")]
        public IHttpActionResult Delete(String groupId, String userId)
        {
            throw new NotImplementedException();
            return this.Ok();
        }
    }

    public class ContactDTO
    {
    }
    

    
}
