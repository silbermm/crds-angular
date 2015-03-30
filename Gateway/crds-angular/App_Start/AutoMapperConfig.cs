using System.Collections.Generic;
using AutoMapper;
using crds_angular.Models.Crossroads;
using MinistryPlatform.Models;

namespace crds_angular.App_Start
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.CreateMap<Dictionary<string, object>, AccountInfo>()
                .ForMember(dest => dest.EmailNotifications,
                    opts => opts.MapFrom(src => src["Bulk_Email_Opt_Out"]));

            Mapper.CreateMap<Attribute, Skill>()
                .ForMember(dest => dest.SkillId, opts => opts.MapFrom(src => src.dp_RecordID))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Attribute_Name));

            Mapper.CreateMap<Skill, Attribute>()
                .ForMember(dest => dest.Attribute_ID, opts => opts.MapFrom(src => src.SkillId));

            Mapper.CreateMap<Contact_Relationship, FamilyMember>()
                .ForMember(dest => dest.ContactId, opts => opts.MapFrom(src => src.Contact_Id))
                .ForMember(dest => dest.PreferredName, opts => opts.MapFrom(src => src.Preferred_Name))
                .ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.Last_Name))
                .ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Email_Address))
                .ForMember(dest => dest.ParticipantId, opts => opts.MapFrom(src => src.Participant_Id));
        }
    }
}