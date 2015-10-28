using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Json;
using crds_angular.Security;
using crds_angular.Util;
using log4net;
using MinistryPlatform.Translation.PlatformService;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class ImageController : MPAuth
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof (ImageController));
        private readonly MPInterfaces.IMinistryPlatformService _mpService;
        private readonly MPInterfaces.IAuthenticationService _authenticationService;
        private readonly MPInterfaces.IApiUserService _apiUserService;

        public ImageController(MPInterfaces.IMinistryPlatformService mpService, MPInterfaces.IAuthenticationService authenticationService, MPInterfaces.IApiUserService apiUserService)
        {
            _authenticationService = authenticationService;
            _apiUserService = apiUserService;
            _mpService = mpService;
        }

        private IHttpActionResult GetImage(Int32 fileId, String fileName, String token)
        {
            var imageStream = _mpService.GetFile(fileId, token);
            if (imageStream == null)
            {
                return (RestHttpActionResult<ApiErrorDto>.WithStatus(HttpStatusCode.NotFound, new ApiErrorDto("No matching image found")));
            }

            HttpContext.Current.Response.Buffer = true;
            return (new FileResult(imageStream, fileName, null, false));
        }

        /// <summary>
        /// Retrieves an image given a file ID.
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns>A byte stream?</returns>
        [Route("api/image/{fileId:int}")]
        [HttpGet]
        public IHttpActionResult GetImage(Int32 fileId)
        {
            try
            {
                return (Authorized(token =>
                {
                    var imageDescription = _mpService.GetFileDescription(fileId, token);
                    return GetImage(fileId, imageDescription.FileName, token);
                }));
            }
            catch (Exception e)
            {
                _logger.Error("Error getting profile image", e);
                return (BadRequest());
            }
        }


        /// <summary>
        /// Retrieves a profile image given a contact ID.
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns>A byte stream?</returns>
        [Route("api/image/profile/{contactId:int}")]
        [HttpGet]
        public IHttpActionResult GetProfileImage(Int32 contactId)
        {
            var token = _apiUserService.GetToken();
            var files = _mpService.GetFileDescriptions("Contacts", contactId, token);
            var file = files.FirstOrDefault(f => f.IsDefaultImage);
            return file != null ? 
                GetImage(file.FileId, file.FileName, token) : 
                (RestHttpActionResult<ApiErrorDto>.WithStatus(HttpStatusCode.NotFound, new ApiErrorDto("No matching image found")));
        }

        /// <summary>
        /// Retrieves an image for a pledge campaign given a record ID.
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns>A byte stream?</returns>
        [Route("api/image/profile/{recordId:int}")]
        [HttpGet]
        public IHttpActionResult GetCampaignImage(Int32 recordId)
        {
            var token = _apiUserService.GetToken();
            var files = _mpService.GetFileDescriptions("Pledge_Campaigns", recordId, token);
            var file = files.FirstOrDefault(f => f.IsDefaultImage);
            return file != null ?
                GetImage(file.FileId, file.FileName, token) :
                (RestHttpActionResult<ApiErrorDto>.WithStatus(HttpStatusCode.NotFound, new ApiErrorDto("No campaign image found")));
        }

        [Route("api/image/profile/")]
        [HttpPost]
        public IHttpActionResult Post()
        {
            return (Authorized(token =>
            {
                const String fileName = "profile.png";

                var contactId = _authenticationService.GetContactId(token);
                var files = _mpService.GetFileDescriptions("MyContact", contactId, token);
                var file = files.FirstOrDefault(f => f.IsDefaultImage);
                var base64String = Request.Content.ReadAsStringAsync().Result;

                if (base64String.Length == 0)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, "Request did not specify a \"file\" for the profile image."));
                }

                var imageBytes = Convert.FromBase64String(base64String.Split(',')[1]);

                if (file!=null)
                {
                    _mpService.UpdateFile(
                        file.FileId,
                        fileName,
                        "Profile Image",
                        true,
                        -1,
                        imageBytes,
                        token
                        );
                }
                else
                {
                    _mpService.CreateFile(
                        "MyContact",
                        contactId,
                        fileName,
                        "Profile Image",
                        true,
                        -1,
                        imageBytes,
                        token
                        );
                }
                return (Ok());
            }));
        }
    }
}

