using System;
using System.Messaging;
using crds_angular.Models.Crossroads.Stewardship;
using Crossroads.Utilities.Messaging;
using NUnit.Framework;

namespace crds_angular.test.IntegrationTests
{
    /// <summary>
    /// This test is here simply to prove out MSMQ setup.
    /// </summary>
    [TestFixture]
    [Category("IntegrationTests")]
    public class SendAndReceiveMessageTest
    {
        private const string QueueName = @".\Private$\TestMessages";
        private MessageQueue _sender;
        private MessageQueue _receiver;
        [SetUp]
        public void SetUp()
        {
            _sender = new MessageQueue(QueueName, QueueAccessMode.Send);
            _receiver = new MessageQueue(QueueName, QueueAccessMode.Receive)
            {
                Formatter = new JsonMessageFormatter()
            };
        }

        [Test]
        public void ShouldSendAndReceiveMessage()
        {
            var dto = new TestDto
            {
                Property1 = "hello",
                Property2 = "world"
            };
            var message = new Message
            {
                Body = dto,
                Formatter = new JsonMessageFormatter()
            };

            _sender.Send(message, MessageQueueTransactionType.Single);

            var received = _receiver.Receive(MessageQueueTransactionType.Single);
            if (received == null)
            {
                Assert.Fail("Could not receive message from queue " + QueueName);
            }
            var receivedDto = (TestDto)received.Body;

            Assert.AreEqual(dto.Property1, receivedDto.Property1);
            Assert.AreEqual(dto.Property2, receivedDto.Property2);
        }

        [Test]
        public void SendToWcfService()
        {
            var evt = new StripeEvent
            {
                Type = "transfer.paid",
                Data = new StripeEventData
                {
                    Object = new StripeTransfer
                    {
                        Amount = 12345,
                        Id = "102030"
                    }
                }
            };
            //var mq = new MessageQueue(@"DIRECT=OS:ing029-hp\private$\Crossroads.StripeEventsService/StripeEvents.svc", QueueAccessMode.Send);
            var message = new Message
            {
                Body = evt,
                Formatter = new JsonMessageFormatter()
            };
            _sender.Send(message);

        }
    }

    public class TestDto
    {
        public string Property1 { get; set; }
        public string Property2 { get; set; }
    }
}
