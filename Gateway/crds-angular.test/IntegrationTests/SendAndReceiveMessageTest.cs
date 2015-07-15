using System;
using System.Messaging;
using Crossroads.Utilities.Messaging;
using NUnit.Framework;

namespace crds_angular.test.IntegrationTests
{
    /// <summary>
    /// This test is here simply to prove out MSMQ setup.
    /// </summary>
    [TestFixture]
    //[Category("IntegrationTests")]
    public class SendAndReceiveMessageTest
    {
        //private const string QueueName = @".\Private$\StripeEvents";
        private const string QueueName = @"FormatName:DIRECT=OS:mp-int-web.crossroads.net\Private$\StripeEvents";
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
    }

    public class TestDto
    {
        public string Property1 { get; set; }
        public string Property2 { get; set; }
    }
}
