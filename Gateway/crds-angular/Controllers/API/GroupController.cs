using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Security;
using log4net;
using MinistryPlatform.Translation.Exceptions;
using MinistryPlatform.Translation.Services.Interfaces;
using crds_angular.Models.Crossroads.Events;

namespace crds_angular.Controllers.API
{
    public class GroupController : MPAuth
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private crds_angular.Services.Interfaces.IGroupService groupService;
        private IAuthenticationService authenticationService;
        private IParticipantService participantService;

        private readonly int GroupRoleDefaultId =
            Convert.ToInt32(ConfigurationManager.AppSettings["Group_Role_Default_ID"]);

        public GroupController(crds_angular.Services.Interfaces.IGroupService groupService,
                               IAuthenticationService authenticationService,
                               IParticipantService participantService)
        {
            this.groupService = groupService;
            this.authenticationService = authenticationService;
            this.participantService = participantService;
        }

        /**
         * Enroll the currently logged-in user into a Community Group, and also register this user for all events for the CG.
         */

        [ResponseType(typeof (GroupDTO))]
        [Route("api/group/{groupId}/participants")]
        public IHttpActionResult Post(int groupId, [FromBody] List<ParticipantSignup> partId)
        {
            return Authorized(token =>
            {
                try
                {
                    groupService.addParticipantsToGroup(groupId, partId);
                    _logger.Debug(String.Format("Successfully added participants {0} to group {1}", partId, groupId));
                    return (Ok());
                }
                catch (GroupFullException e)
                {
                    var responseMessage = new ApiErrorDto("Group Is Full", e).HttpResponseMessage;

                    // Using HTTP Status code 422/Unprocessable Entity to indicate Group Is Full
                    // http://tools.ietf.org/html/rfc4918#section-11.2
                    responseMessage.StatusCode = (HttpStatusCode) 422;
                    throw new HttpResponseException(responseMessage);
                }
                catch (Exception e)
                {
                    _logger.Error("Could not add user to group", e);
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
        }

        [ResponseType(typeof (GroupDTO))]
        [Route("api/group/{groupId}")]
        public IHttpActionResult Get(int groupId)
        {
            return Authorized(token =>
            {
                try
                {
                    var participant = participantService.GetParticipantRecord(token);
                    var contactId = authenticationService.GetContactId(token);

                    var detail = groupService.getGroupDetails(groupId, contactId, participant, token);

                    return Ok(detail);
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("Get Group", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
                
            }
                );
        }

        [ResponseType(typeof(List<Event>))]
        [Route("api/group/{groupId}/events")]
        public IHttpActionResult GetEvents(int groupId)
        {
            return Authorized(token =>
                {
                    try
                    {
                        var eventList = groupService.GetGroupEvents(groupId);
                        return Ok(eventList);
                    }
                    catch (Exception e)
                    {
                        var apiError = new ApiErrorDto("Error getting events ", e);
                        throw new HttpResponseException(apiError.HttpResponseMessage);
                    }
                }
            );
        }

        [ResponseType(typeof(List<GroupContactDTO>))]
        [Route("api/group/{groupId}/event/{eventId}")]
        public IHttpActionResult GetParticipants(int groupId, int eventId, string recipients)
        {
            return Authorized(token =>
                {
                    try
                    {
                        if (recipients != "current" || recipients != "potential")
                        {
                            throw new ApplicationException("Recipients should be 'current' or 'potential'");
                        }
                        var memberList = groupService.GetGroupMembersByEvent(groupId, eventId, recipients);
                        return Ok(memberList);
                    }
                    catch (Exception e)
                    {
                        var apiError = new ApiErrorDto("Error getting participating group members ", e);
                        throw new HttpResponseException(apiError.HttpResponseMessage);
                    }
                }
            );
        }

        // TODO: implement later
        [ResponseType(typeof (ContactDTO))]
        [Route("api/group/{groupId}/user/{userId}")]
        public IHttpActionResult Get(String groupId, String userId)
        {
            throw new NotImplementedException();
        }

        // TODO: implement later
        [ResponseType(typeof (GroupDTO))]
        [Route("api/group/{groupId}/user/{userId}")]
        public IHttpActionResult Delete(String groupId, String userId)
        {
            throw new NotImplementedException();
        }
    }

    public class ContactDTO
    {
    }
}