using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using System;

namespace crds_angular.Services
{
    public class GroupService : crds_angular.Services.Interfaces.IGroupService
    {
        private IGroupService _mpGroupService;
        private IConfigurationWrapper _configurationWrapper;
        private IAuthenticationService _authenticationService;

        public GroupService(IGroupService mpGroupService, IConfigurationWrapper configurationWrapper,
            IAuthenticationService authenticationService)
        {
            _mpGroupService = mpGroupService;
            _configurationWrapper = configurationWrapper;
            _authenticationService = authenticationService;
        }
    }
}