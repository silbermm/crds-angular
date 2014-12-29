using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Services
{
    public class SkillService
    {

        public static Models.Crossroads.Skill Add(Models.Crossroads.Skill crSkill, int parentRecordId, string token)
        {
            var mpAttribute = crSkill.GetAttribute();
            var x = MinistryPlatform.Translation.Services.GetMyRecords.CreateAttribute(mpAttribute, parentRecordId, token);
            if (x)
            {
                //need to update the recordId, service should return that
                return crSkill;
            }
            throw new Exception("skill save failed");
        }
    }
}