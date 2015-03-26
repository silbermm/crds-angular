using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.PlatformService;
using Attribute = MinistryPlatform.Models.Attribute;

namespace MinistryPlatform.Translation.Services
{
    public class GetMyRecords : BaseService
    {
        //public static IEnumerable<Contact_Relationship> GetMyFamily(int contactId, string token)
        //{
        //    var viewId = Convert.ToInt32(ConfigurationManager.AppSettings["MyContactFamilyRelationshipViewId"]);
        //    var viewRecords = MinistryPlatformService.GetSubpageViewRecords(viewId, contactId, token);

        //    return viewRecords.Select(viewRecord => new Contact_Relationship
        //    {
        //        Contact_Id = (int) viewRecord["Contact_ID"],
        //        Email_Address = (string) viewRecord["Email_Address"],
        //        Last_Name = (string) viewRecord["Last Name"],
        //        Preferred_Name = (string)viewRecord["Preferred Name"]
        //    }).ToList();
        //}

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

        //public static List<Group> GetMyServingTeams(int contactId, string token)
        //{
        //    var pageViewId = Convert.ToInt32(ConfigurationManager.AppSettings["MyServingTeams"]);
        //    var searchString = ",,,," + contactId;
        //    var teams = MinistryPlatformService.GetPageViewRecords(pageViewId, token, searchString);
        //    var groups = new List<Group>();
        //    foreach (var team in teams)
        //    {
        //        var group = new Group
        //        {
        //            GroupId = (int) team["Group_ID"],
        //            Name = (string) team["Group_Name"],
        //            GroupRole = (string) team["Role_Title"]
        //        };
        //        groups.Add(group);
        //    }
        //    return groups;
        //} 

        public static int CreateAttribute(Attribute attribute, int parentRecordId, string token)
        {
            try
            {
                var subPageId = Convert.ToInt32(ConfigurationManager.AppSettings["MySkills"]);
                var platformServiceClient = new PlatformServiceClient();
                SelectQueryResult result;

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
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool DeleteAttribute(int recordId, string token)
        {
            try
            {
                var platformServiceClient = new PlatformServiceClient();
                SelectQueryResult result;

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
            catch (Exception e)
            {
                throw e;
            }
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