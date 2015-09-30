using System;
using System.Data.Entity.Core;
using crds_angular.Exceptions;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Services;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Services
{
    /// <summary>
    /// A service for impersonating another MinistryPlatform user.
    /// </summary>
    public class UserImpersonationService : IUserImpersonationService
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Construct a new UserImpersonationService with the given IUserService.
        /// </summary>
        /// <param name="userService">An IUserService for performing user lookups</param>
        public UserImpersonationService(IUserService userService)
        {
            _userService = userService;
        }

        public TOutput WithImpersonation<TOutput>(string authToken, string useridToImpersonate, Func<TOutput> action)
        {
            ImpersonatedUserGuid.Clear();

            var authUser = _userService.GetByAuthenticationToken(authToken);
            if (authUser == null || !authUser.CanImpersonate)
            {
                throw (new ImpersonationNotAllowedException());
            }

            var user = _userService.GetByUserId(useridToImpersonate);
            if (user == null)
            {
                throw (new ImpersonationUserNotFoundException(useridToImpersonate));
            }

            ImpersonatedUserGuid.Set(user.Guid);

            try
            {
                return (action());
            }
            finally
            {
                ImpersonatedUserGuid.Clear();
            }
        }
    }
}
