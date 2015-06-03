using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IFormSubmissionService
    {
        int SubmitFormResponse(FormResponse form);
    }
}
