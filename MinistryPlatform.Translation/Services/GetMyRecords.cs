using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MinistryPlatform.Translation.Services
{
    public class GetMyRecords
    {
        public static List<MinistryPlatform.Models.Attribute> GetMyAttributes(int recordId, String token)
        {
            var subPageId = Convert.ToInt32(ConfigurationManager.AppSettings["MySkills"]);
            var subPageRecords = Services.GetPageRecordService.GetSubPageRecords(subPageId, recordId, token);
            var attributes = new List<MinistryPlatform.Models.Attribute>();

            foreach (var record in subPageRecords) {
                var attribute = new MinistryPlatform.Models.Attribute
                {
                    Attribute_Name = (string)record["Attribute_Name"],
                    Attribute_Type = (string)record["Attribute_Type"],
                    dp_FileID = (int?)record["dp_FileID"],
                    dp_RecordID = (int)record["dp_RecordID"],
                    dp_RecordName = (string)record["dp_RecordName"],
                    dp_RecordStatus = (int)record["dp_RecordStatus"],
                    dp_Selected = (int)record["dp_Selected"]
                };
                attributes.Add(attribute);
            }
            return attributes;

        }
    }
}
