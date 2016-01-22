using MinistryPlatform.Models;

namespace crds_angular.Models.Crossroads.Participants
{
    public class ParticipantProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            AutoMapper.Mapper.CreateMap<Participant, MinistryPlatform.Translation.Models.People.MpParticipant>()
                .ForMember(dest => dest.Contact_ID, opts => opts.MapFrom(src => src.ContactId))
                .ForMember(dest => dest.Display_Name, opts => opts.MapFrom(src => src.DisplayName))
                .ForMember(dest => dest.Email_Address, opts => opts.MapFrom(src => src.EmailAddress))
                .ForMember(dest => dest.Participant_ID, opts => opts.MapFrom(src => src.ParticipantId))
                .ForMember(dest => dest.Attendance_Start_Date, opts => opts.MapFrom(src => src.AttendanceStart));
        }
    }
}