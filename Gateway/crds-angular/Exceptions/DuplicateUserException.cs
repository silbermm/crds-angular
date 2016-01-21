using System;

namespace crds_angular.Exceptions
{
    public class DuplicateUserException : Exception
    {
        public DuplicateUserException(string userName) : base(string.Format("User {0} already exists", userName)) { }
    }
}