using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Security;
using crds_angular.Models.MP;
using MinistryPlatform.Translation.Services;

namespace crds_angular.Controllers.API
{
    public class GroupController : MPAuth
    {
        readonly private log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IGroupService _groupService;
        private readonly string GroupRoleDefaultId = ConfigurationManager.AppSettings["Group_Role_Default_ID"];

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [ResponseType(typeof(GroupDTO))]
        [Route("api/group/{groupId}/user")]
        public IHttpActionResult Post(String groupId)
        {
            return Authorized(token =>
            {
                try
                {
                    int groupParticipantId = _groupService.addUserToGroup(token, groupId, GroupRoleDefaultId, DateTime.Now);
                    logger.Debug("Added user - group/participant id = " + groupParticipantId);
                    var response = new Dictionary<string, object>{
                        {"groupParticipantId", groupParticipantId}
                    };
                    return Ok(response);
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
        [Route("api/group/{groupId}/user")]
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

        // TODO: implement later
        [ResponseType(typeof(List<ContactDTO>))]
        [Route("api/group/{groupId}")]
        public IHttpActionResult Get(String groupId)
        {
            throw new NotImplementedException();
            return this.Ok();

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
    }
}
