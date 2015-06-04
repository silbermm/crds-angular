using System;
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

        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _fixture = new FormSubmissionService(_ministryPlatformService.Object);

            _mockAnswer1 = new FormAnswer
            {
                Field = 375,
                FormResponseId = responseId,
                OpportunityResponse = 7329,
                Response = "Test Last Name"
            };

            _mockAnswer2 = new FormAnswer
            {
                Field = 376,
                FormResponseId = responseId,
                OpportunityResponse = 7329,
                Response = "Test First Name"
            };

            _mockAnswer3 = new FormAnswer
            {
                Field = 377,
                FormResponseId = responseId,
                OpportunityResponse = 7329,
                Response = "Test Middle Initial"
            };

            _mockForm = new FormResponse
            {
                FormId = 17,
                ContactId = 2389887,
                OpportunityId = 313,
                OpportunityResponseId = 7329,
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
                {"Form_ID", _mockForm.FormId},
                {"Response_Date", DateTime.Today},
                {"Contact_ID", _mockForm.ContactId},
                {"Opportunity_ID", _mockForm.OpportunityId},
                {"Opportunity_Response", _mockForm.OpportunityResponseId}
            };

            var expectedAnswerDict1 = new Dictionary<string, object>
            {
                {"Form_Response_ID", _mockAnswer1.FormResponseId},
                {"Form_Field_ID", _mockAnswer1.Field},
                {"Response", _mockAnswer1.Response},
                {"Opportunity_Response", _mockAnswer1.OpportunityResponse}
            };

            var expectedAnswerDict2 = new Dictionary<string, object>
            {
                {"Form_Response_ID", _mockAnswer2.FormResponseId},
                {"Form_Field_ID", _mockAnswer2.Field},
                {"Response", _mockAnswer2.Response},
                {"Opportunity_Response", _mockAnswer2.OpportunityResponse}
            };

            var expectedAnswerDict3 = new Dictionary<string, object>
            {
                {"Form_Response_ID", _mockAnswer3.FormResponseId},
                {"Form_Field_ID", _mockAnswer3.Field},
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
