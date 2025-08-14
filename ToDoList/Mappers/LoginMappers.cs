using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ToDoList.Dtos.LoginDto;
using ToDoList.Models;

namespace ToDoList.Mappers
{

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // DTO -> Entity
            CreateMap<LoginRequestDto, AppUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            // Entity -> Response DTO
            CreateMap<AppUser, LoginResponsDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}
