using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace crds_angular.Models.Json
{
    /// <summary>
    /// An IHttpActionResult implementation, specific for RESTful JSON web services.  This wraps a JSON object, represented by the T parameter.
    /// </summary>
    /// <typeparam name="T">The type of object this will wrap, and serialize to JSON</typeparam>
    public class RestHttpActionResult<T> : IHttpActionResult
    {
        private readonly HttpStatusCode _statusCode;
        private readonly T _content;

        /// <summary>
        /// Get the Content object associated to this Result.
        /// </summary>
        public T Content
        {
            get { return _content; }
        }

        /// <summary>
        /// Get the HTTP StatusCode associated to this Result.
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get { return _statusCode; }
        }

        /// <summary>
        /// Construct a new RestHttpActionResult with the given content (to be serialized to JSON on output) and HTTP status code.
        /// The constructor is intentionally marked private, as static convenience methods are provided for constructing.
        /// </summary>
        /// <param name="statusCode">The HttpStatusCode for this result</param>
        /// <param name="content">The content object to wrap</param>
        private RestHttpActionResult(HttpStatusCode statusCode, T content)
        {
            _statusCode = statusCode;
            _content = content;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(JsonConvert.SerializeObject(_content))
            };

            response.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            return (Task.FromResult(response));
        }

        /// <summary>
        /// Get a new RestHttpActionResult for the given content, with HttpStatusCode.OK (200) indicating success.
        /// </summary>
        /// <param name="content">The content object to wrap</param>
        /// <returns>the resulting RestHttpActionResult</returns>
        public static RestHttpActionResult<T> Ok(T content)
        {
            return (new RestHttpActionResult<T>(HttpStatusCode.OK, content));
        }

        /// <summary>
        /// Get a new RestHttpActionResult for the given content, with HttpStatusCode.BadRequest (400) indicating a client error.
        /// </summary>
        /// <param name="content">The content object to wrap</param>
        /// <returns>the resulting RestHttpActionResult</returns>
        public static RestHttpActionResult<T> ClientError(T content)
        {
            return (new RestHttpActionResult<T>(HttpStatusCode.BadRequest, content));
        }

        /// <summary>
        /// Get a new RestHttpActionResult for the given content, with HttpStatusCode.InternalServerError (500) indicating a server error.
        /// </summary>
        /// <param name="content">The content object to wrap</param>
        /// <returns>the resulting RestHttpActionResult</returns>
        public static RestHttpActionResult<T> ServerError(T content)
        {
            return (new RestHttpActionResult<T>(HttpStatusCode.InternalServerError, content));
        }

        /// <summary>
        /// Get a new RestHttpActionResult for the given status code and content.
        /// </summary>
        /// <param name="statusCode">The HttpStatusCode</param>
        /// <param name="content">The content object to wrap</param>
        /// <returns>the resulting RestHttpActionResult</returns>
        public static RestHttpActionResult<T> WithStatus(HttpStatusCode statusCode, T content)
        {
            return (new RestHttpActionResult<T>(statusCode, content));
        }
    }
}