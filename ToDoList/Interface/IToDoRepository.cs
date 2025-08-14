using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Dtos.ToDoDto;
using ToDoList.Models;

namespace ToDoList.Repositories
    {

    public interface IToDoRepository
    {
        Task<List<ToDoItem>> GetAllAsync(string userId);
        Task<ToDoItem?> GetByIdAsync(int id, string userId);
        Task<ToDoItem> AddAsync(ToDoItem todo);
        Task<ToDoItem> UpdateAsync(ToDoItem todo);
        Task DeleteAsync(ToDoItem todo);
    }

    }
