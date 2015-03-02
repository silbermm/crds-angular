using crds_angular.Models.Crossroads;
using MinistryPlatform.Translation.Services;

namespace crds_angular.Services
{
    public class SkillService
    {

        public static int Add(Skill crSkill, int parentRecordId, string token)
        {
                var mpAttribute = crSkill.GetAttribute();
                return  GetMyRecords.CreateAttribute(mpAttribute, parentRecordId, token);
        }

        public static bool Delete(int recordId, string token)
        {
            return GetMyRecords.DeleteAttribute(recordId, token);
        }
    }
}