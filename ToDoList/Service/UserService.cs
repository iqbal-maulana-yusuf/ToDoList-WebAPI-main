using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ToDoList.Dtos.UserDto;
using ToDoList.Models;
using ToDoList.Repository;

namespace ToDoList.Service
{
    public interface IUserService
    {
        public Task<List<UserResponsDto>> GetAll();
        public Task<UserResponsDto> GetById(string userId);
        public Task<UserResponsDto> Create(UserCreateRequestDto createDto);
        public Task<UserResponsDto> Update(UserUpdateRequestDto updateDto, string userId);
        public Task Delete(string userId);


    }
    public class UserService : IUserService
    {
        private IUserRepository _userRepo;
        private IMapper _mapper;

        public UserService(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }
        public async Task<UserResponsDto> Create(UserCreateRequestDto createDto)
        {
            var user = _mapper.Map<AppUser>(createDto);
            var userModel = await _userRepo.CreateAsync(user);
            var userModelDto = _mapper.Map<UserResponsDto>(userModel);
            return userModelDto;
        }

        public async Task Delete(string userId)
        {
            await _userRepo.DeleteAsync(userId);
        }

        public async Task<List<UserResponsDto>> GetAll()
        {
            var users = await _userRepo.GetAllAsync();
            var userDtos = _mapper.Map<List<UserResponsDto>>(users);
            return userDtos;
        }

        public async Task<UserResponsDto> GetById(string userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            var userDto = _mapper.Map<UserResponsDto>(user);
            return userDto;
        }

        public async Task<UserResponsDto> Update(UserUpdateRequestDto updateDto, string userId)
        {
            var user = await _userRepo.UpdateAsync(updateDto, userId);
            var userDto = _mapper.Map<UserResponsDto>(user);
            return userDto; 
        }
    }
}