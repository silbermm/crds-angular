using System.Collections.Generic;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class FormSubmissionServiceTest
    {
        private FormSubmissionService _fixture;
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private FormResponse _mockForm;
        private FormAnswer _mockAnswer1, _mockAnswer2, _mockAnswer3;
        private const int formResponsePageId = 424;
        private const int formAnswerPageId = 425;
        private const int responseId = 2;
        //private const int GetMyFamilyViewId = 75;

        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _fixture = new FormSubmissionService(_ministryPlatformService.Object);
            
            _mockAnswer1 = new FormAnswer
            {
                Field = 10,
                FormResponseId = responseId,
                OpportunityResponse = 39,
                Response = "Question Answer 1"
            };

            _mockAnswer2 = new FormAnswer
            {
                Field = 20,
                FormResponseId = responseId,
                OpportunityResponse = 39,
                Response = "Question Answer 2"
            };

            _mockAnswer3 = new FormAnswer
            {
                Field = 30,
                FormResponseId = responseId,
                OpportunityResponse = 39,
                Response = "Question Answer 3"
            };

            _mockForm = new FormResponse
            {
                FormId = 1,
                ContactId = 42,
                OpportunityId = 8,
                OpportunityResponseId = 39,
                FormAnswers = new List<FormAnswer>
                {
                    _mockAnswer1,
                    _mockAnswer2,
                    _mockAnswer3
                }
            };
        }

        [Test]
        public void SubmitFormResponse()
        {
            var expectedResponseDict = new Dictionary<string, object>
            {
                {"Form", _mockForm.FormId},
                {"Contact", _mockForm.ContactId},
                {"Opportunity", _mockForm.OpportunityId},
                {"Opportunity_Response", _mockForm.OpportunityResponseId}
            };

            var expectedAnswerDict1 = new Dictionary<string, object>
            {
                {"FormResponse", _mockAnswer1.FormResponseId},
                {"Field", _mockAnswer1.Field},
                {"Response", _mockAnswer1.Response},
                {"Opportunity_Response", _mockAnswer1.OpportunityResponse}
            };

            var expectedAnswerDict2 = new Dictionary<string, object>
            {
                {"FormResponse", _mockAnswer2.FormResponseId},
                {"Field", _mockAnswer2.Field},
                {"Response", _mockAnswer2.Response},
                {"Opportunity_Response", _mockAnswer2.OpportunityResponse}
            };

            var expectedAnswerDict3 = new Dictionary<string, object>
            {
                {"FormResponse", _mockAnswer3.FormResponseId},
                {"Field", _mockAnswer3.Field},
                {"Response", _mockAnswer3.Response},
                {"Opportunity_Response", _mockAnswer3.OpportunityResponse}
            };

            _ministryPlatformService.Setup(m => m.CreateRecord(formResponsePageId, expectedResponseDict, It.IsAny<string>(), true)).Returns(responseId);
            _ministryPlatformService.Setup(m => m.CreateRecord(formAnswerPageId, expectedAnswerDict1, It.IsAny<string>(), true));
            _ministryPlatformService.Setup(m => m.CreateRecord(formAnswerPageId, expectedAnswerDict2, It.IsAny<string>(), true));
            _ministryPlatformService.Setup(m => m.CreateRecord(formAnswerPageId, expectedAnswerDict3, It.IsAny<string>(), true));

            var result = _fixture.SubmitFormResponse(_mockForm);
            
            Assert.AreEqual(responseId, result);
            _ministryPlatformService.VerifyAll();
        }
    }
}
