using Crossroads.AsyncJobs.Models;

namespace Crossroads.AsyncJobs.Interfaces
{
    public interface IJobExecutor<T>
    {
        void Execute(JobDetails<T> details);
    }
}
