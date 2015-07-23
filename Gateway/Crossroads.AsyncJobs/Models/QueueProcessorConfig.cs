using System.Messaging;
using Crossroads.AsyncJobs.Interfaces;

namespace Crossroads.AsyncJobs.Models
{
    public class QueueProcessorConfig<T>
    {
        public string QueueName { get; set; }
        public IMessageFormatter MessageFormatter;
        public IJobExecutor<T> JobExecutor { get; set; }
    }
}