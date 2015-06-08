using System.Collections.Generic;
using AutoMapper;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Opportunity;
using MinistryPlatform.Models;
using Response = MinistryPlatform.Models.Response;

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

            Mapper.CreateMap<Group, OpportunityGroup>()
                .ForMember(dest => dest.GroupId, opts => opts.MapFrom(src => src.GroupId))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.EventTypeId, opts => opts.MapFrom(src => src.EventTypeId))
                .ForMember(dest => dest.Participants, opts => opts.MapFrom(src => src.Participants));

            Mapper.CreateMap<GroupParticipant, OpportunityGroupParticipant>()
                .ForMember(dest => dest.ContactId, opts => opts.MapFrom(src => src.ContactId))
                .ForMember(dest => dest.GroupRoleId, opts => opts.MapFrom(src => src.GroupRoleId))
                .ForMember(dest => dest.GroupRoleTitle, opts => opts.MapFrom(src => src.GroupRoleTitle))
                .ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
                .ForMember(dest => dest.NickName, opts => opts.MapFrom(src => src.NickName))
                .ForMember(dest => dest.ParticipantId, opts => opts.MapFrom(src => src.ParticipantId));

            Mapper.CreateMap<Response, OpportunityResponseDto>()
                .ForMember(dest => dest.Closed, opts => opts.MapFrom(src => src.Closed))
                .ForMember(dest => dest.Comments, opts => opts.MapFrom(src => src.Comments))
                .ForMember(dest => dest.EventId, opts => opts.MapFrom(src => src.Event_ID))
                .ForMember(dest => dest.OpportunityId, opts => opts.MapFrom(src => src.Opportunity_ID))
                .ForMember(dest => dest.ParticipantId, opts => opts.MapFrom(src => src.Participant_ID))
                .ForMember(dest => dest.ResponseDate, opts => opts.MapFrom(src => src.Response_Date))
                .ForMember(dest => dest.ResponseId, opts => opts.MapFrom(src => src.Response_ID))
                .ForMember(dest => dest.ResponseResultId, opts => opts.MapFrom(src => src.Response_Result_ID));


        }
    }
}