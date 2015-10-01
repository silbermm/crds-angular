using System.Collections.Generic;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Services.Interfaces;
using Crossroads.AsyncJobs.Interfaces;
using Crossroads.AsyncJobs.Models;
using Crossroads.Utilities.Extensions;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;

namespace Crossroads.AsyncJobs.Processors
{
    public class SignupToServeProcessor : BaseService, IJobExecutor<SaveRsvpDto>
    {
        private readonly IServeService _serveService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IAuthenticationService _authenticationService;

        public SignupToServeProcessor(IServeService serveService, IConfigurationWrapper configurationWrapper, IAuthenticationService authenticationService)
            : base(authenticationService, configurationWrapper)
        {
            this._serveService = serveService;
            this._configurationWrapper = configurationWrapper;
            this._authenticationService = authenticationService;
        }

        public void Execute(JobDetails<SaveRsvpDto> details)
        {
            var result = WithApiLogin<List<int>>(token => _serveService.SaveServeRsvp(token, details.Data));
            if (result.Count > 0)
            {
                //success!
            }
            if (result.Count < 1)
            {
                //failed...
            }
        }
    }
}