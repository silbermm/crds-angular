using System;
using System.Messaging;
using System.Threading;
using Crossroads.AsyncJobs.Models;
using Crossroads.Utilities.Messaging.Interfaces;
using log4net;

namespace Crossroads.AsyncJobs.Application
{
    public class QueueProcessor<T> : IQueueProcessor
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(QueueProcessor<T>));
        private readonly QueueProcessorConfig<T> _config;
        private readonly MessageQueue _queue;
        private bool _paused;

        public QueueProcessor(QueueProcessorConfig<T> queueProcessorConfig, IMessageQueueFactory messageQueueFactory)
        {
            _config = queueProcessorConfig;

            _queue = messageQueueFactory.CreateQueue(_config.QueueName, QueueAccessMode.Receive, _config.MessageFormatter);
        }

        public void Start()
        {
            _paused = false;

            ThreadPool.QueueUserWorkItem(delegate
            {
                while (true)
                {
                    if (_paused)
                    {
                        return;
                    }
                    var message = _queue.Receive(MessageQueueTransactionType.Automatic);
                    if (message == null)
                    {
                        _logger.Error("Received message was null");
                        continue;
                    }
                    else if (message.Body == null)
                    {
                        _logger.Error("Received message did not have a body, Id: " + message.Id);
                        continue;
                    }
                    else if (message.Body.GetType() != GetType().GetGenericArguments()[0])
                    {
                        _logger.Error(string.Format("Received message body type ({0}) did not match expected {1}", message.Body.GetType(), GetType().GetGenericArguments()[0].BaseType));
                        continue;
                    }

                    var details = new JobDetails<T>
                    {
                        Data = (T)message.Body,
                        EnqueuedDateTime = message.ArrivedTime,
                        RetrievedDateTime = DateTime.Now
                    };
                    try
                    {
                        _config.JobExecutor.Execute(details);
                    }
                    catch (Exception e)
                    {
                        _logger.Error("Error in worker", e);
                    }
                }
            });
        }

        public void Pause()
        {
            _paused = true;
        }
    }
}