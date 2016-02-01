using System.Messaging;

namespace Crossroads.Utilities.Messaging.Interfaces
{
    public interface IMessageQueueFactory
    {
        MessageQueue CreateQueue(string queueName, QueueAccessMode accessMode, IMessageFormatter formatter = null);
    }
}