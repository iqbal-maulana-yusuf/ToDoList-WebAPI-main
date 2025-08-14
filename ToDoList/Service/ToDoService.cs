using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ToDoList.Dtos.ToDoDto;
using ToDoList.Models;
using ToDoList.Repository;

namespace ToDoList.Service
{
    public interface IToDoService
    {
        public Task<List<ToDoResponsDto>> GetAll(string userId);
        public Task<ToDoResponsDto> GetById(string userId, int id);
        public Task<ToDoResponsDto> Create(ToDoCreateRequestDto createDto, string userId);
        public Task<ToDoResponsDto> Update(ToDoUpdateDto updateDto, string userId, int id);
        public Task Delete(string userId, int id);

        
        
    }
    public class ToDoService : IToDoService
    {

        private IToDoRepository _todoRepo;
        private IMapper _mapper;

        public ToDoService(IToDoRepository toDoRepo, IMapper mapper)
        {
            _todoRepo = toDoRepo;
            _mapper = mapper;
        }

        public async Task<ToDoResponsDto> Create(ToDoCreateRequestDto createDto, string userId)
        {
            var todoService = _mapper.Map<ToDoItem>(createDto);
            todoService.UserId = userId;
            var todoServiceModel = await _todoRepo.CreateAsync(todoService);
            var todoServiceModelDto = _mapper.Map<ToDoResponsDto>(todoServiceModel);
            return todoServiceModelDto;

        }

        public async Task Delete(string userId, int id)
        {
            await _todoRepo.DeleteAsync(userId, id);
        }

        public async Task<List<ToDoResponsDto>> GetAll(string userId)
        {
            var todoServiceModels = await _todoRepo!.GetAllAsync(userId);
            var todoServicesModelDtos = _mapper.Map<List<ToDoResponsDto>>(todoServiceModels);
            return todoServicesModelDtos;
        }

        public async Task<ToDoResponsDto> GetById(string userId, int id)
        {
            var todoServiceModel = await _todoRepo.GetByIdAsync(id, userId);
            if (todoServiceModel == null)
            {
                return null!;
            }
            var todoServiceModelDto = _mapper.Map<ToDoResponsDto>(todoServiceModel);
            return todoServiceModelDto;
        }

        public async Task<ToDoResponsDto> Update(ToDoUpdateDto updateDto, string userId, int id)
        {
            var todoService = await _todoRepo.UpdateAsync(updateDto, userId, id);
            var todoServiceDto = _mapper.Map<ToDoResponsDto>(todoService);
            return todoServiceDto;
            
        }
    }
}