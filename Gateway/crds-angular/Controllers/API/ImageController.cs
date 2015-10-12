using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Models.Json;
using crds_angular.Security;
using MinistryPlatform.Models;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class ImageController : MPAuth
    {
        private readonly MPInterfaces.IMinistryPlatformService _mpService;
        private readonly MPInterfaces.IAuthenticationService _authenticationService;

        public ImageController(MPInterfaces.IMinistryPlatformService mpService, MPInterfaces.IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _mpService = mpService;
        }

        /// <summary>
        /// Retrieves an image given a file ID.
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns>A byte stream?</returns>
        [Route("api/image/{fileId:int}")]
        [HttpGet]
        public IHttpActionResult GetImage(int fileId)
        {
            return (Authorized(token =>
            {
                var image = _mpService.GetFile(fileId, token);
                if (image == null)
                {
                    return (RestHttpActionResult<ApiErrorDto>.WithStatus(HttpStatusCode.NotFound, new ApiErrorDto("No matching image found")));
                }
                using (var memoryStream = new MemoryStream())
                {
                    image.CopyTo(memoryStream);
                    return (Ok(memoryStream.ToArray()));
                }
            }));
        }
    }
}

