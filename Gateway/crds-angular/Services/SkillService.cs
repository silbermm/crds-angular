using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Services
{
    public class SkillService : ISkillService
    {

        private IGetMyRecords _getMyRecords;

        public SkillService(IGetMyRecords getMyRecords)
        {
            _getMyRecords = getMyRecords;
        }

        public  int Add(Skill crSkill, int parentRecordId, string token)
        {
                var mpAttribute = crSkill.GetAttribute();
                return  _getMyRecords.CreateAttribute(mpAttribute, parentRecordId, token);
        }

        public  bool Delete(int recordId, string token)
        {
            return _getMyRecords.DeleteAttribute(recordId, token);
        }
    }
}