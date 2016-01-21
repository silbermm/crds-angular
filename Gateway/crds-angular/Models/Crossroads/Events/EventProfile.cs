namespace crds_angular.Models.Crossroads.Events
{
    public class EventProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            AutoMapper.Mapper.CreateMap<MinistryPlatform.Models.Event, Models.Crossroads.Events.Event>()
                .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.EventTitle))
                .ForMember(dest => dest.location, opts => opts.MapFrom(src => src.Congregation))
                .ForMember(dest => dest.StartDate, opts => opts.MapFrom(src => src.EventStartDate))
                .ForMember(dest => dest.EndDate, opts => opts.MapFrom(src => src.EventEndDate))
                .ForMember(dest => dest.EventType, opts => opts.MapFrom(src => src.EventType))
                .ForMember(dest => dest.PrimaryContactId, opts => opts.MapFrom(src => src.PrimaryContact.ContactId))
                .ForMember(dest => dest.PrimaryContactEmailAddress, opts => opts.MapFrom(src => src.PrimaryContact.EmailAddress));
        }
    }
}