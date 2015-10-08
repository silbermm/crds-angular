using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Security;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class AttributeTypeController : MPAuth
    {
        private readonly IAttributeService _attributeService;        

        public AttributeTypeController(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        [ResponseType(typeof (List<AttributeType>))]
        [Route("api/AttributeType/{attributeTypeId:int?}")]
        public IHttpActionResult Get(int? attributeTypeId = null)
        {
            return Authorized(token =>
            {                
                var attributeTypes = _attributeService.GetAttributeTypes(attributeTypeId);

                return this.Ok(attributeTypes);
            });
        }
    }
}