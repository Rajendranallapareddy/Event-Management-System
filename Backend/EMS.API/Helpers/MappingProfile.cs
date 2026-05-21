using AutoMapper;
using EMS.Common.DTOs.Event;
using EMS.Common.DTOs.Session;
using EMS.Common.DTOs.Speaker;
using EMS.Common.DTOs.User;
using EMS.DAL.Models;

namespace EMS.API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "Participant"));
            CreateMap<User, UserResponseDto>();
            CreateMap<EventCreateDto, Event>();
            CreateMap<Event, EventResponseDto>()
                .ForMember(dest => dest.SessionCount, opt => opt.MapFrom(src => src.Sessions.Count));
            CreateMap<SpeakerCreateDto, Speaker>();
            CreateMap<Speaker, SpeakerResponseDto>();
            CreateMap<SessionCreateDto, Session>();
            CreateMap<Session, SessionResponseDto>()
                .ForMember(dest => dest.EventTitle, opt => opt.MapFrom(src => src.Event != null ? src.Event.Title : string.Empty))
                .ForMember(dest => dest.SpeakerName, opt => opt.MapFrom(src => src.Speaker != null ? src.Speaker.Name : null));
        }
    }
}