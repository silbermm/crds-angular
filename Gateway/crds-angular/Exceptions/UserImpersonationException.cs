using System;

namespace crds_angular.Exceptions
{
    public abstract class UserImpersonationException : Exception
    {
        protected UserImpersonationException(string message) : base(message)
        {
        }
    }

    public class ImpersonationNotAllowedException : UserImpersonationException
    {
        private const string DefaultMessage = "User is not authorized to impersonate other users.";
        public ImpersonationNotAllowedException()
            : base(DefaultMessage)
        {
        }

        public ImpersonationNotAllowedException(string auxMessage)
            : base(string.Format("{0}: {1}", DefaultMessage, auxMessage))
        {
        }
    }

    public class ImpersonationUserNotFoundException : UserImpersonationException
    {
        public ImpersonationUserNotFoundException()
            : base("Could not locate user to impersonate")
        {
        }

        public ImpersonationUserNotFoundException(string userId)
            : base(string.Format("Could not locate user '{0}' to impersonate", userId))
        {
        }
    }
}