using System.Messaging;
using Crossroads.Utilities.Messaging.Interfaces;

namespace Crossroads.Utilities.Messaging
{
    public class MessageQueueFactory : IMessageQueueFactory
    {
        public MessageQueue CreateQueue(string queueName, QueueAccessMode accessMode, IMessageFormatter formatter)
        {
            var queue = new MessageQueue(queueName, accessMode)
            {
                Formatter = formatter ?? new JsonMessageFormatter(),
                MessageReadPropertyFilter = new MessagePropertyFilter
                {
                    ArrivedTime = true,
                    Body = true
                }
            };
            return (queue);
        }
    }
}