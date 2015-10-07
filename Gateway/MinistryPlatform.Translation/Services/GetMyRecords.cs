using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Services.Interfaces;
using Attribute = MinistryPlatform.Models.Attribute;
using RoleDTO = MinistryPlatform.Models.DTO.RoleDto;

namespace MinistryPlatform.Translation.Services
{
    public class GetMyRecords : BaseService
    {
        public GetMyRecords(IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
        {
            
        }

        public static List<RoleDTO> GetMyRoles(string token)
        {
            var pageId = Convert.ToInt32(ConfigurationManager.AppSettings["MyRoles"]);
            var pageRecords = MinistryPlatformService.GetRecordsDict(pageId, token);

            return pageRecords.Select(record => new RoleDTO
            {
                Id = (int) record["Role_ID"], Name = (string) record["Role_Name"]
            }).ToList();
        }
        public static List<Attribute> GetMyAttributes(int recordId, string token)
        {
            var subPageId = Convert.ToInt32(ConfigurationManager.AppSettings["MySkills"]);
            var subPageRecords = MinistryPlatformService.GetSubPageRecords(subPageId, recordId, token);
            var attributes = new List<Attribute>();

            foreach (var record in subPageRecords)
            {
                var attribute = new Attribute
                {
                    Attribute_Name = (string) record["Attribute_Name"],
                    Attribute_Type = (string) record["Attribute_Type"],
                    dp_FileID = (int?) record["dp_FileID"],
                    dp_RecordID = (int) record["dp_RecordID"],
                    dp_RecordName = (string) record["dp_RecordName"],
                    dp_RecordStatus = (int) record["dp_RecordStatus"],
                    dp_Selected = (int) record["dp_Selected"]
                };
                attributes.Add(attribute);
            }
            return attributes;
        }

        public static int CreateAttribute(Attribute attribute, int parentRecordId, string token)
        {
            var subPageId = Convert.ToInt32(ConfigurationManager.AppSettings["MySkills"]);
            var platformServiceClient = new PlatformServiceClient();

            using (
                new OperationContextScope(
                    (IClientChannel) platformServiceClient.InnerChannel))
            {
                WebOperationContext.Current.OutgoingRequest.Headers.Add("Authorization",
                                                                        "Bearer " + token);
                attribute.Start_Date = DateTime.Now;
                var dictionary = getDictionary(attribute);
                return platformServiceClient.CreateSubpageRecord(subPageId, parentRecordId, dictionary, false);
            }
        }

        public static bool DeleteAttribute(int recordId, string token)
        {
            var platformServiceClient = new PlatformServiceClient();

            using (
                new OperationContextScope(
                    (IClientChannel) platformServiceClient.InnerChannel))
            {
                WebOperationContext.Current.OutgoingRequest.Headers.Add("Authorization",
                                                                        "Bearer " + token);
                platformServiceClient.DeleteSubpageRecord(AppSettings("MySkills"), recordId, null);
            }
            return true;
        }

        private static Dictionary<string, object> getDictionary(Object input)
        {
            var dictionary = input.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(input, null));
            return dictionary;
        }
    }
}