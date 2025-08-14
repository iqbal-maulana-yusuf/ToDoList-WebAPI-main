using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Dtos.UserDto;
using ToDoList.Models;

namespace ToDoList.Repository
{
    public interface IUserRepository
    {
        public Task<List<AppUser>> GetAllAsync();
        public Task<AppUser> GetByIdAsync(string userId);
        public Task<AppUser> CreateAsync(AppUser userCreate);
        public Task<AppUser> UpdateAsync(UserUpdateRequestDto userUpdateDto, string userId);
        public Task DeleteAsync(string userId);
    }
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<AppUser>> GetAllAsync()
        {
            var userModels = await _context.Users.ToListAsync();
            return userModels;
        }

        public async Task<AppUser> GetByIdAsync(string userId)
        {
            var userModel = await _context.Users.FirstOrDefaultAsync(s => s.Id == userId);
            if (userModel == null)
            {
                return null!;
            }
            return userModel;


        }
        public async Task<AppUser> CreateAsync(AppUser userCreate)
        {
            var userModel = await _context!.Users.AddAsync(userCreate);
            await _context.SaveChangesAsync();
            return userModel.Entity;
        }

        public async Task DeleteAsync(string userId)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            _context.Users.Remove(existingUser!);
            await _context.SaveChangesAsync();
        }



        public async Task<AppUser> UpdateAsync(UserUpdateRequestDto userUpdateDto, string userId)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            existingUser!.UserName = userUpdateDto.Username;
            existingUser.Email = userUpdateDto.Email;
            existingUser.PasswordHash = userUpdateDto.Password;
            await _context.SaveChangesAsync();
            return existingUser;

        }
        

    }
}