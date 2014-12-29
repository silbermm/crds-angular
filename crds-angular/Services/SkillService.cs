using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Services
{
    public class SkillService
    {

        public static int Add(Models.Crossroads.Skill crSkill, int parentRecordId, string token)
        {
            //try
            //{
                var mpAttribute = crSkill.GetAttribute();
                return  MinistryPlatform.Translation.Services.GetMyRecords.CreateAttribute(mpAttribute, parentRecordId, token);
                //crSkill.RecordId = recordId;
                //return crSkill;
            //}
            //catch
            
            //throw new Exception("skill save failed");
        }

        public static bool Delete(int recordId, string token)
        {
            return MinistryPlatform.Translation.Services.GetMyRecords.DeleteAttribute(recordId, token);
        }
    }
}