using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Models.Crossroads.Attribute;
using crds_angular.Security;
using crds_angular.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class AttributeTypeController : MPAuth
    {
        private readonly IAttributeService _attributeService;        

        public AttributeTypeController(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        [ResponseType(typeof (List<AttributeTypeDTO>))]
        [Route("api/attributetype")]
        public IHttpActionResult Get()
        {
            return Authorized(token =>
            {                
                var attributeTypes = _attributeService.GetAttributeTypes(null);

                return this.Ok(attributeTypes);
            });
        }

        [ResponseType(typeof(AttributeTypeDTO))]
        [Route("api/attributetype/{attributeTypeId}")]
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