using crds_angular.Models.Crossroads;

namespace crds_angular.Services.Interfaces
{
    public interface ISkillService
    {
        int Add(Skill crSkill, int parentRecordId, string token);
        bool Delete(int recordId, string token);
    }
}