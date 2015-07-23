using System;
using System.Collections.Generic;
using System.Web.Hosting;

namespace Crossroads.AsyncJobs.Application
{
    public class JobProcessor : IRegisteredObject
    {
        private readonly IQueueProcessor[] _queueProcessors;
        private bool _started;
        private readonly object _lockObject = new object();

        public JobProcessor(IQueueProcessor[] processors)
        {
            _queueProcessors = processors;
        }

        public void Start()
        {
            lock (_lockObject)
            {
                Console.WriteLine("Starting job processor");
                if (_started)
                {
                    return;
                }
                _started = true;

                HostingEnvironment.RegisterObject(this);

                foreach (var p in _queueProcessors)
                {
                    p.Start();
                }
            }
        }

        public void Stop()
        {
            lock (_lockObject)
            {
                if (!_started)
                {
                    return;
                }
                _started = false;

                HostingEnvironment.UnregisterObject(this);

                foreach (var p in _queueProcessors)
                {
                    p.Pause();
                }

            }
        }

        public void Stop(bool immediate)
        {
            Stop();
        }
    }
}