using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IFormSubmissionService
    {
        int GetFormFieldId(int crossroadsId);
        int SubmitFormResponse(FormResponse form);
        List<FormField> GetFieldsForForm(int formId);
    }
}
