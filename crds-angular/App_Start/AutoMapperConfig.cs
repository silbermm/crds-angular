using crds_angular.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Models.MP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.App_Start
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {

           /* AutoMapper.Mapper.CreateMap<Dictionary<string, object>, Contact>()
                //.ForMember(d => d.ContactId, o => o.MapFrom(s => s["Contact_ID"]))
                //.ForMember(d => d.CompanyName,o => o.MapFrom(s => s["Company_Name"]))
                //.ForMember(d => d.FirstName, o => o.MapFrom(s => s["First_Name"]))
                //.ForMember(d => d.MiddleName,o => o.MapFrom(s => s["Middle_Name"]))
                //.ForMember(d => d.LastName, o => o.MapFrom(s => s["Last_Name"]))
                //.ForMember(d => d.NickName, o => o.MapFrom(s => s["Nick_Name"]))
                //.ForMember(d => d.DisplayName, o => o.MapFrom(s => s["Display_Name"]))
                //.ForMember(d => d.MaidenName, o => o.MapFrom(s => s["Maiden_Name"]))
                //.ForMember(d => d.PrefixId, o => o.MapFrom(s => s["Prefix_ID"]))
                .ForMember(d => d.Prefix, o => o.MapFrom(s => s["Prefix_ID_Text"]))
                //.ForMember(d => d.SuffixId, o => o.MapFrom(s => s["Suffix_ID"]))
                .ForMember(d => d.Suffix, o => o.MapFrom(s => s["Suffix_ID_Text"]))
                //.ForMember(d => d.GenderId, o => o.MapFrom(s => s["Gender_ID"]))
                .ForMember(d => d.Gender, o => o.MapFrom(s => s["Gender_ID_Text"]))
                //.ForMember(d => d.MaritalStatusId, o => o.MapFrom(s => s["Marital_Status_ID"]))
                .ForMember(d => d.MaritalStatus, o => o.MapFrom(s => s["Marital_Status_ID_Text"]))
                //.ForMember(d => d.ContactStatusId, o => o.MapFrom(s => s["Contact_Status_ID"]))
                .ForMember(d => d.ContactStatus, o => o.MapFrom(s => s["Contact_Status_ID_Text"]))
                //.ForMember(d => d.EmailAddress, o => o.MapFrom(s => s["Email_Address"]))
                //.ForMember(d => d.EmailUnlisted, o => o.MapFrom(s => s["Email_Unlisted"]))
                //.ForMember(d => d.BulkEmailOptOut, o => o.MapFrom(s => s["Bulk_EM"]))
                .ForMember(d => d.MobileCarrier, o => o.MapFrom(s => s["Mobile_Carrier_Text"]))
                .ForMember(d => d.MobileCarrierId, o => o.MapFrom(s => s["Mobile_Carrier"]))
                .ForMember(d => d.UserAccount, o => o.MapFrom(s => s["User_Account_Text"]))
                .ForMember(d => d.UserAccount, o => o.MapFrom(s => s["User_Account"]))
                .ForMember(d => d.Industry, o => o.MapFrom(s => s["Industry_ID_Text"]))
                .ForMember(d => d.IndustryId, o => o.MapFrom(s => s["Industry"]))
                .ForMember(d => d.ParticipantRecordId, o => o.MapFrom(s => s["Participant_Record"]))
                .ForMember(d => d.ParticipantRecord, o => o.MapFrom(s => s["Participant_Record_Text"]))
                .ForMember(d => d.SSN, o => o.MapFrom(s => s["SSN/EID"]))
                .ForMember(d => d.Occupation, o => o.MapFrom(s => s["Occupation"]))
                ;*/

            //AutoMapper.Mapper.CreateMap<Dictionary<string,object>, Person>();
            AutoMapper.Mapper.CreateMap<Dictionary<string, object>, AccountInfo>()
                .ForMember(dest => dest.EmailNotifications,
                          opts => opts.MapFrom(src => src["Bulk_Email_Opt_Out"]))
                ;
        }
    }

}
