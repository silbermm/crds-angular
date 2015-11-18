
using System.Messaging;

namespace crds_angular.Models.Crossroads.Events
{
    public class EventProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            AutoMapper.Mapper.CreateMap<MinistryPlatform.Models.Event, Models.Crossroads.Events.Event>()
                .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.EventTitle))
                .ForMember(dest => dest.location, opts => opts.MapFrom(src => src.EventLocation))
                .ForMember(dest => dest.time, opts => opts.MapFrom(src => src.EventStartDate));

            AutoMapper.Mapper.CreateMap<MinistryPlatform.Models.Event, Models.Crossroads.Events.Event>().ReverseMap();
        }
    }
}