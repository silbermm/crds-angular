using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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



        [Route("api/image/profile/")]
        [HttpPost]
        public IHttpActionResult Post()
        {
            return (Authorized(token =>
            {
                var imageBytes = new byte[0];
                var fileName = "";
                var recordId = "";
                if (Request.Content.IsMimeMultipartContent())
                {
                    Request.Content.ReadAsMultipartAsync<MultipartMemoryStreamProvider>(new MultipartMemoryStreamProvider()).ContinueWith((task) =>
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
                                case "\"recordId\"":
                                    recordId = content.ReadAsStringAsync().Result;
                                    break;
                                default:
                                    //Should we throw an exception?
                                    break;

                            }
                        }
                        return _mpService.CreateFile(
                            "Contacts",
                            Int32.Parse(recordId),
                            fileName,
                            "Profile Image",
                            true,
                            -1,
                            imageBytes,
                            token
                            );

                    });
                }
                else
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
                }
                return null;
            }));
        }
    }
}

