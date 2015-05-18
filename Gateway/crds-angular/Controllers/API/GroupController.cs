using System;
using System.Collections;
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
using Newtonsoft.Json;

namespace crds_angular.Controllers.API
{
    public class GroupController : MPAuth
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private crds_angular.Services.Interfaces.IGroupService groupService;
        private IAuthenticationService authenticationService;
       
        private readonly int GroupRoleDefaultId =
            Convert.ToInt32(ConfigurationManager.AppSettings["Group_Role_Default_ID"]);

        public GroupController(crds_angular.Services.Interfaces.IGroupService groupService,
            IAuthenticationService authenticationService)
        {
            this.groupService = groupService;
            this.authenticationService = authenticationService;
        }

        /**
         * Enroll the currently logged-in user into a Community Group, and also register this user for all events for the CG.
         */

        [ResponseType(typeof (GroupDTO))]
        [Route("api/group/{groupId}/participants")]
        public IHttpActionResult Post(int groupId, [FromBody] PartID partId )
        {
            return Authorized(token =>
            {
                try
                {
                    var response = groupService.addParticipantsToGroup(groupId, partId.partId);
                    logger.Debug(String.Format("Response for adding participants {0} to group {1}: {2}", partId.partId, groupId, response));
                    return (Ok(response));
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
                var contactId = authenticationService.GetContactId(token);

                var detail = groupService.getGroupDetails(groupId, contactId, participant, token);
               
                return Ok(detail);
            }
          );
        }

        // TODO: implement later
        [ResponseType(typeof(ContactDTO))]
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
