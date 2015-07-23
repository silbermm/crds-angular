using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crossroads.AsyncJobs.Models
{
    public class JobDetails<T>
    {
        public DateTime EnqueuedDateTime { get; set; }
        public DateTime RetrievedDateTime { get; set; }
        public T Data { get; set; }
    }
}