using System.Web.Http;

namespace Crossroads.AsyncJobs.Application
{
    public class JobProcessorController : ApiController
    {
        private readonly JobProcessor _jobProcessor;

        public JobProcessorController(JobProcessor jobProcessor)
        {
            _jobProcessor = jobProcessor;
        }

        public string Get()
        {
            _jobProcessor.Start();
            return "value";
        }
    }
}
