using AutoMapper;
using ToDoList.Models;
using ToDoList.Dtos.RegisterDto;

namespace ToDoList.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // DTO -> Entity
            CreateMap<RegisterRequestDto, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            // Entity -> Response DTO
            CreateMap<AppUser, RegisterReponsDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}
