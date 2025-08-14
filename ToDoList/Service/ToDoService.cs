using ToDoList.Dtos.ToDoDto;
using ToDoList.Interface;
using ToDoList.Models;
using ToDoList.Repositories;

namespace ToDoList.Services
{
    public class ToDoService : IToDoService
    {
        private readonly IToDoRepository _toDoRepository;

        public ToDoService(IToDoRepository toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }

        public async Task<List<ToDoItem>> GetAllUserToDosAsync(string userId)
        {
            return await _toDoRepository.GetAllAsync(userId);
        }

        public async Task<ToDoItem?> GetToDoByIdAsync(int id, string userId)
        {
            return await _toDoRepository.GetByIdAsync(id, userId);
        }

        public async Task<ToDoItem> CreateToDoAsync(ToDoCreateDto dto, string userId)
        {
            var todo = new ToDoItem
            {
                Title = dto.Title,
                Description = dto.Description,
                UserId = userId
            };
            return await _toDoRepository.AddAsync(todo);
        }

        public async Task<ToDoItem?> UpdateToDoAsync(int id, ToDOUpdateDto dto, string userId)
        {
            var todo = await _toDoRepository.GetByIdAsync(id, userId);
            if (todo == null) return null;

            if (!string.IsNullOrEmpty(dto.Title)) todo.Title = dto.Title;
            if (dto.Description != null) todo.Description = dto.Description;
            if (dto.IsCompleted.HasValue) todo.IsCompleted = dto.IsCompleted.Value;

            return await _toDoRepository.UpdateAsync(todo);
        }

        public async Task<bool> DeleteToDoAsync(int id, string userId)
        {
            var todo = await _toDoRepository.GetByIdAsync(id, userId);
            if (todo == null) return false;

            await _toDoRepository.DeleteAsync(todo);
            return true;
        }
    }
}
