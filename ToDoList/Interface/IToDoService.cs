using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Dtos.ToDoDto;
using ToDoList.Models;

namespace ToDoList.Interface
{
    public interface IToDoService
    {
        Task<List<ToDoItem>> GetAllUserToDosAsync(string userId);
        Task<ToDoItem?> GetToDoByIdAsync(int id, string userId);
        Task<ToDoItem> CreateToDoAsync(ToDoCreateDto dto, string userId);
        Task<ToDoItem?> UpdateToDoAsync(int id, ToDOUpdateDto dto, string userId);
        Task<bool> DeleteToDoAsync(int id, string userId);
    }
}