using System;
using System.Net;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Json;

namespace crds_angular.Exceptions
{
    public abstract class UserImpersonationException : Exception
    {
        private readonly HttpStatusCode _httpStatusCode;

        protected UserImpersonationException(string message, HttpStatusCode httpStatusCode)
            : base(message)
        {
            _httpStatusCode = httpStatusCode;
        }

        public RestHttpActionResult<ApiErrorDto> GetRestHttpActionResult()
        {
            return (RestHttpActionResult<ApiErrorDto>.WithStatus(_httpStatusCode, new ApiErrorDto(Message)));
        }

    }

    public class ImpersonationNotAllowedException : UserImpersonationException
    {
        private const string DefaultMessage = "User is not authorized to impersonate other users.";
        public ImpersonationNotAllowedException()
            : base(DefaultMessage, HttpStatusCode.Forbidden)
        {
        }

        public ImpersonationNotAllowedException(string auxMessage)
            : base(string.Format("{0}: {1}", DefaultMessage, auxMessage), HttpStatusCode.Forbidden)
        {
        }
    }

    public class ImpersonationUserNotFoundException : UserImpersonationException
    {
        public ImpersonationUserNotFoundException()
            : base("Could not locate user to impersonate", HttpStatusCode.Conflict)
        {
        }

        public ImpersonationUserNotFoundException(string userId)
            : base(string.Format("Could not locate user '{0}' to impersonate", userId), HttpStatusCode.Conflict)
        {
        }
    }
}