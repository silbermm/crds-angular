using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
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



        [Route("api/image/")]
        [HttpPost]
        public HttpResponseMessage Post()
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var imageBytes = new byte[10000];
            if (Request.Content.IsMimeMultipartContent())
            {
                Request.Content.ReadAsMultipartAsync<MultipartMemoryStreamProvider>(new MultipartMemoryStreamProvider()).ContinueWith((task) =>
                {
                    MultipartMemoryStreamProvider provider = task.Result;
                    foreach (HttpContent content in provider.Contents)
                    {
                        Stream imageStream = content.ReadAsStreamAsync().Result;
                        using (var memoryStream = new MemoryStream())
                        {
                            imageStream.CopyTo(memoryStream);
                            imageBytes = memoryStream.ToArray();
                        }
                    }
                });
                return result;
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
            }
        }

        public async Task<HttpResponseMessage> PostFormData()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var bytes = new byte[10000];
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                // Show all the key-value pairs.
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        Trace.WriteLine(string.Format("{0}: {1}", key, val));
                    }
                }
                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                    Trace.WriteLine("Server file path: " + file.LocalFileName);
                    /*
                    await file.ReadAsByteArrayAsync().ContinueWith(b =>
                    {
                        bytes = b;
                    });
                     */
                }
                /*
                (Authorized(token =>
                {
                    return _mpService.CreateFile(
                        provider.FormData.Get("pageName"),
                        provider.FormData.Get("recordId"),
                        provider.FileData[0].
                    );
                }));
                */
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}

