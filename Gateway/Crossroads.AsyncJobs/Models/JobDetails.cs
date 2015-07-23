using System;

namespace Crossroads.AsyncJobs.Models
{
    public class JobDetails<T>
    {
        public DateTime EnqueuedDateTime { get; set; }
        public DateTime RetrievedDateTime { get; set; }
        public T Data { get; set; }
    }
}