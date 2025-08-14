using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ToDoList.Dtos.ToDoDto;
using ToDoList.Models;

namespace ToDoList.Mappers
{
    public class ToDoMappers : Profile
    {
        public ToDoMappers()
        {
            CreateMap<ToDoItem, ToDoResponsDto>();
            CreateMap<ToDoCreateRequestDto, ToDoItem>();
            CreateMap<ToDoUpdateDto, ToDoResponsDto>();
        }
    }
}