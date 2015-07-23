using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crossroads.AsyncJobs.Models;

namespace Crossroads.AsyncJobs.Interfaces
{
    public interface IJobExecutor<T>
    {
        void Execute(JobDetails<T> details);
    }
}
