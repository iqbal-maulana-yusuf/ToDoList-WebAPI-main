using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ToDoList.Dtos.UserDto;
using ToDoList.Models;

namespace ToDoList.Mappers
{
    public class UserMappers : Profile
    {
        public UserMappers()
        {
            CreateMap<UserCreateRequestDto, AppUser>();
            CreateMap<AppUser, UserResponsDto>();

        }
        
    }
}