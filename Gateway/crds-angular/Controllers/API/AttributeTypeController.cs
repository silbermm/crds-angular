using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using crds_angular.Security;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class AttributeTypeController : MPAuth
    {
        private readonly IAttributeService _attributeService;        

        public AttributeTypeController(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        [ResponseType(typeof (List<AttributeType>))]
        [Route("api/AttributeType")]
        public IHttpActionResult Get()
        {
            return Authorized(token =>
            {                
                var attributeTypes = _attributeService.GetAttributeTypes(null);

                return this.Ok(attributeTypes);
            });
        }

        [ResponseType(typeof(AttributeType))]
        [Route("api/AttributeType/{attributeTypeId}")]
        public IHttpActionResult Get(int attributeTypeId)
        {
            return Authorized(token =>
            {
                var attributeTypes = _attributeService.GetAttributeTypes(attributeTypeId);
                return this.Ok(attributeTypes[0]);
            });
        }
    }
}