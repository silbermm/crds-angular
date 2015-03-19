using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using crds_angular.Security;
using crds_angular.Models.MP;
using MinistryPlatform.Translation.Exceptions;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.PlatformService;

namespace crds_angular.Controllers.API
{
    public class GroupController : MPAuth
    {
        readonly private log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IGroupService groupService;
        private IEventService eventService;
        private IAuthenticationService authenticationService;

        private readonly int GroupRoleDefaultId = Convert.ToInt32(ConfigurationManager.AppSettings["Group_Role_Default_ID"]);

        public GroupController(IGroupService groupService, IEventService eventService, IAuthenticationService authenticationService)
        {
            this.groupService = groupService;
            this.eventService = eventService;
            this.authenticationService = authenticationService;
        }

        /**
         * Enroll the currently logged-in user into a Community Group, and also register this user for all events for the CG.
         */
        [ResponseType(typeof(GroupDTO))]
        [Route("api/group/{groupId}/user")]
        public IHttpActionResult Post(String groupId)
        {
            return Authorized(token =>
            {
                try
                {
                    var participant = authenticationService.GetParticipantRecord(token);
                    int participantId = participant.ParticipantId;

                    // First sign this user up for the community group
                    int groupParticipantId = groupService.addParticipantToGroup(participantId, Convert.ToInt32(groupId), GroupRoleDefaultId, DateTime.Now);
                    logger.Debug("Added user - group/participant id = " + groupParticipantId);
                    var response = new Dictionary<string, object>{
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
        [ResponseType(typeof(GroupDTO))]
        [Route("api/group/{groupId}/users")]
        public IHttpActionResult Post(String groupId, [FromBody] List<ContactDTO> contact)
        {
            throw new NotImplementedException();
            return this.Ok();
        }

        // TODO: implement later
        [ResponseType(typeof(ContactDTO))]
        [Route("api/group/{groupId}/user/{userId}")]
        public IHttpActionResult Get(String groupId, String userId)
        {
            throw new NotImplementedException();
            return this.Ok();

        }

       
        [ResponseType(typeof(List<GroupDTO>))]
        [Route("api/group/{groupId}")]
        public IHttpActionResult Get(int groupId)
        {
            Group g = groupService.getGroupDetails(groupId);
            
            var group = new GroupDTO()
            {
                groupID = g.RecordId,
                groupFullInd = g.Full,
                waitListInd = g.WaitList,
                waitListGroupId = g.WaitListGroupId
            };
            
            return Ok(group);
        
        }

        // TODO: implement later
        [ResponseType(typeof(GroupDTO))]
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

    public class GroupDTO
    {
        public int groupID { get; set; }
        public bool groupFullInd { get; set; }
        public bool waitListInd { get; set; }
        public IList<int> waitListGroupId { get; set; }
     }
    

    
}
