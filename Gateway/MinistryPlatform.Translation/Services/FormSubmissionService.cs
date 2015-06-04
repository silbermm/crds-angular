using System;
using System.Collections.Generic;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class FormSubmissionService : BaseService, IFormSubmissionService
    {
        private readonly int formResponsePageId = AppSettings("FormResponsePageId");
        private readonly int formAnswerPageId = AppSettings("FormAnswerPageId");
        private IMinistryPlatformService _ministryPlatformService;

        public FormSubmissionService(IMinistryPlatformService ministryPlatformService)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public int SubmitFormResponse(FormResponse form )
        {
            var responseId = CreateFormResponse(form.FormId, form.ContactId, form.OpportunityId, form.OpportunityResponseId);
            foreach (var answer in form.FormAnswers)
            {
                answer.FormResponseId = responseId;
                CreateFormAnswer(answer);
            }
            return responseId;
        }

        private int CreateFormResponse(int formId, int contactId, int opportunityId, int opportunityResponseId)
        {
            var formResponse = new Dictionary<string, object>
            {
                {"Form_ID", formId},
                {"Response_Date", DateTime.Today},
                {"Contact_ID", contactId},
                {"Opportunity_ID", opportunityId},
                {"Opportunity_Response", opportunityResponseId}
            };

            var responseId = _ministryPlatformService.CreateRecord(formResponsePageId, formResponse, apiLogin(), true);
            return responseId;
        }

        private void CreateFormAnswer(FormAnswer answer)
        {

            var formAnswer = new Dictionary<string, object>
            {
                {"Form_Response_ID", answer.FormResponseId},
                {"Form_Field_ID", answer.Field},
                {"Response", answer.Response},
                {"Opportunity_Response", answer.OpportunityResponse}
            };

            _ministryPlatformService.CreateRecord(formAnswerPageId, formAnswer, apiLogin(), true);
        }
    }
}
