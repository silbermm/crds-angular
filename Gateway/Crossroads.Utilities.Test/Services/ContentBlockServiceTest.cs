using System.Collections.Generic;
using Crossroads.Utilities.Models;
using Crossroads.Utilities.Services;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace Crossroads.Utilities.Test.Services
{
    public class ContentBlockServiceTest
    {
        // ReSharper disable once CollectionNeverUpdated.Local
        private ContentBlockService _fixture;
        private Mock<IRestClient> _cmsRestClient;
        private ContentBlocks _contentBlocks;

        [SetUp]
        public void SetUp()
        {
            _contentBlocks = new ContentBlocks
            {
                contentBlocks = new List<ContentBlock>
                {
                    new ContentBlock
                    {
                        Title = "error1",
                        Id = 1
                    },
                    new ContentBlock
                    {
                        Title = "error2",
                        Id = 2
                    },
                    new ContentBlock
                    {
                        Title = "error3",
                        Id = 3
                    }
                }
            };
            var restResponse = new Mock<IRestResponse<ContentBlocks>>();
            restResponse.Setup(mocked => mocked.Data).Returns(_contentBlocks);

            _cmsRestClient = new Mock<IRestClient>();
            _cmsRestClient.Setup(mocked => mocked.Execute<ContentBlocks>(It.IsAny<IRestRequest>())).Returns(restResponse.Object);

            _fixture = new ContentBlockService(_cmsRestClient.Object);
        }

        [Test]
        public void TestContentBlocks()
        {
            Assert.AreSame(_contentBlocks.contentBlocks[0], _fixture["error1"]);
            Assert.AreSame(_contentBlocks.contentBlocks[1], _fixture["error2"]);
            Assert.AreSame(_contentBlocks.contentBlocks[2], _fixture["error3"]);
        }
    }
}
