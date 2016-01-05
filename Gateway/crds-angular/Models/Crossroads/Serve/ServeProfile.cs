using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Models.Crossroads.Serve
{
    public class ServeProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            AutoMapper.Mapper.CreateMap<Dictionary<string, object>, ServeReminder>()
                .ForMember(dest => dest.OpportunityTitle, opts => opts.MapFrom(src => src["Opportunity Title"]))
                .ForMember(dest => dest.OpportunityContactId, opts => opts.MapFrom(src => src["Opportunity_Contact_ID"]))
                .ForMember(dest => dest.OpportunityEmailAddress, opts => opts.MapFrom(src => src["Opportunity_Contact_Email_Address"]))
                .ForMember(dest => dest.EventEndDate, opts => opts.MapFrom(src => src["Event_End_Date"]))
                .ForMember(dest => dest.EventStartDate, opts => opts.MapFrom(src => src["Event_Start_Date"]))
                .ForMember(dest => dest.EventTitle, opts => opts.MapFrom(src => src["Event_Title"]))
                .ForMember(dest => dest.SignedupContactId, opts => opts.MapFrom(src => src["Contact_ID"]))
                .ForMember(dest => dest.SignedupEmailAddress, opts => opts.MapFrom(src => src["Email_Address"]))
                .ForMember(dest => dest.TemplateId, opts => opts.MapFrom(src => src["Communication_ID"]))
                .ForMember(dest => dest.ShiftStart, opts => opts.MapFrom(src => src["Shift_Start"]))
                .ForMember(dest => dest.ShiftEnd, opts => opts.MapFrom(src => src["Shift_End"]))
                ;

        }
    }
}
