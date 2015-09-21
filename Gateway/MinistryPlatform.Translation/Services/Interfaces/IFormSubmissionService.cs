using System;
using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IFormSubmissionService
    {
        List<FormField> GetFieldsForForm(int formId);
        int GetFormFieldId(int crossroadsId);
        List<TripFormResponse> GetTripFormResponsesByRecordId(int recordId);
        List<TripFormResponse> GetTripFormResponsesBySelectionId(int selectionId);
        int SubmitFormResponse(FormResponse form);

        DateTime? GetTripFormResponseByContactId(int p, int pledgeId);
    }
}