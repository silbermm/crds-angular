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
            Int32? fileId = null;
            String fileName = null;
            foreach (var file in files.Where(file => file.IsDefaultImage))
            {
                fileId = file.FileId;
                fileName = file.FileName;
                break;
            }
            return fileId == null ? 
                (RestHttpActionResult<ApiErrorDto>.WithStatus(HttpStatusCode.NotFound, new ApiErrorDto("No matching image found"))) : 
                GetImage(fileId.Value, fileName, token);
        }

        [Route("api/image/profile/{fileId:int=-1}")]
        [HttpPost]
        public IHttpActionResult Post(Int32 fileId)
        {
            return (Authorized(token =>
            {
                var imageBytes = new byte[0];
                String fileName = null;
                // Getting contact ID from logged-in user token, rather than requiring it to be passed in
                //var recordId = "";
                var contactId = _authenticationService.GetContactId(token);
                if (Request.Content.IsMimeMultipartContent())
                {
                    Request.Content.LoadIntoBufferAsync().Wait();
                    Request.Content.ReadAsMultipartAsync<MultipartMemoryStreamProvider>(new MultipartMemoryStreamProvider()).ContinueWith(task =>
                    {
                        MultipartMemoryStreamProvider provider = task.Result;
                        foreach (HttpContent content in provider.Contents)
                        {
                            switch (content.Headers.ContentDisposition.Name)
                            {
                                case "\"file\"":
                                    imageBytes = content.ReadAsByteArrayAsync().Result;
                                    fileName = content.Headers.ContentDisposition.FileName;
                                    fileName = fileName.Replace("\"", "");
                                    break;
                                // Don't need recordId, can get it from authenticated user
                                //case "\"recordId\"":
                                //    recordId = content.ReadAsStringAsync().Result;
                                //    break;
                                default:
                                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
                            }
                        }

                        if (string.IsNullOrEmpty(fileName) || imageBytes.Length == 0)
                        {
                            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, "Request did not specify a \"file\" for the profile image."));
                        }
                        if (fileId > 0)
                        {
                            _mpService.UpdateFile(
                                fileId,
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
                                "Contacts",
                                //recordId,
                                contactId,
                                fileName,
                                "Profile Image",
                                true,
                                -1,
                                imageBytes,
                                token
                                );
                        }

                    });
                    return (Ok());
                }
                else
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
                }
            }));
        }
    }
}

