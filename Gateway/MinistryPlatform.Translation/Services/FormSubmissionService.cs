using System;
using System.Collections.Generic;
using System.Linq;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class FormSubmissionService : BaseService, IFormSubmissionService
    {
        private readonly int _formResponsePageId = AppSettings("FormResponsePageId");
        private readonly int _formAnswerPageId = AppSettings("FormAnswerPageId");
        private readonly int _formFieldCustomPage = AppSettings("AllFormFieldsView");

        private readonly IMinistryPlatformService _ministryPlatformService;

        public FormSubmissionService(IMinistryPlatformService ministryPlatformService)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public int GetFormFieldId(int crossroadsId)
        {
            var searchString = string.Format(",{0}", crossroadsId);
            var formFields = _ministryPlatformService.GetPageViewRecords(_formFieldCustomPage, apiLogin(), searchString);

            var field = formFields.Single();
            var formFieldId = field.ToInt("Form_Field_ID");
            return formFieldId;
        }

        public List<FormField> GetFieldsForForm(int formId)
        {
            var searchString = string.Format(",,,,{0}", formId);
            var formFields = _ministryPlatformService.GetPageViewRecords(_formFieldCustomPage, apiLogin(), searchString);

            return formFields.Select(formField => new FormField
            {
                CrossroadsId = formField.ToInt("CrossroadsId"),
                FieldLabel = formField.ToString("Field_Label"),
                FieldOrder = formField.ToInt("Field_Order"),
                FieldType = formField.ToString("Field_Type"),
                FormFieldId = formField.ToInt("Form_Field_ID"),
                FormId = formField.ToInt("Form_ID"),
                FormTitle = formField.ToString("Form_Title"),
                Required = formField.ToBool("Required")
            }).ToList();
        }

        public int SubmitFormResponse(FormResponse form)
        {
            var token = apiLogin();
            var responseId = CreateFormResponse(form.FormId, form.ContactId, form.OpportunityId,
                form.OpportunityResponseId, token);
            foreach (var answer in form.FormAnswers)
            {
                answer.FormResponseId = responseId;
                CreateFormAnswer(answer, token);
            }
            return responseId;
        }

        private int CreateFormResponse(int formId, int contactId, int opportunityId, int opportunityResponseId, string token)
        {
            var formResponse = new Dictionary<string, object>
            {
                {"Form_ID", formId},
                {"Response_Date", DateTime.Today},
                {"Contact_ID", contactId},
                {"Opportunity_ID", opportunityId},
                {"Opportunity_Response", opportunityResponseId}
            };

            var responseId = _ministryPlatformService.CreateRecord(_formResponsePageId, formResponse, token, true);
            return responseId;
        }

        private void CreateFormAnswer(FormAnswer answer, string token)
        {
            var formAnswer = new Dictionary<string, object>
            {
                {"Form_Response_ID", answer.FormResponseId},
                {"Form_Field_ID", answer.FieldId},
                {"Response", answer.Response},
                {"Opportunity_Response", answer.OpportunityResponseId}
            };

            _ministryPlatformService.CreateRecord(_formAnswerPageId, formAnswer, token, true);
        }
    }
}