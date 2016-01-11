using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface ICongregationService
    {
        Congregation GetCongregationById(int id);
    }
}