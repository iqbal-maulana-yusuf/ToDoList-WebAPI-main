using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Dtos.ToDoDto;
using ToDoList.Models;

namespace ToDoList.Repository
{
    public interface IToDoRepository
    {
        Task<List<ToDoItem>> GetAllAsync(string UserId);
        Task<ToDoItem?> GetByIdAsync(int id, string userId);
        Task<ToDoItem> CreateAsync(ToDoItem createModel);
        Task<ToDoUpdateDto> UpdateAsync(ToDoUpdateDto updateDto, string userId, int id);
        Task DeleteAsync(string userId, int id);
    }
    public class ToDoRepository : IToDoRepository
    {
        private ApplicationDbContext _context;
        public ToDoRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ToDoItem> CreateAsync(ToDoItem createModel)
        {
            var todoRepoModel = await _context!.ToDoItems.AddAsync(createModel);
            await _context.SaveChangesAsync();
            return todoRepoModel.Entity;  
        }

        public async Task DeleteAsync(string userId, int id)
        {
            var existingTodo = await _context.ToDoItems.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            _context.ToDoItems.Remove(existingTodo!);
            await _context.SaveChangesAsync();
            
        }

        public async Task<List<ToDoItem>> GetAllAsync(string userId)
        {
            var todoRepoModels = await _context.ToDoItems.Where(x => x.UserId == userId).ToListAsync();
            return todoRepoModels;
        }

        public async Task<ToDoItem?> GetByIdAsync(int id, string userId)
        {
            var todoRepoModel = await _context.ToDoItems.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            return todoRepoModel;
        }

        public async Task<ToDoUpdateDto> UpdateAsync(ToDoUpdateDto updateDto, string userId, int id)
        {
            var existingTodo = await _context.ToDoItems.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (existingTodo == null)
            {
                return null!;
            }

            existingTodo.Title = updateDto.Title!;
            existingTodo.Description = updateDto.Description;
            existingTodo.IsCompleted = updateDto.IsCompleted ?? false;

            await _context.SaveChangesAsync();

            return updateDto;
           
        }
    }
}