using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Repositories
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly ApplicationDbContext _context;

        public ToDoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ToDoItem>> GetAllAsync(string userId)
        {
            return await _context.ToDoItems.Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task<ToDoItem?> GetByIdAsync(int id, string userId)
        {
            return await _context.ToDoItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        public async Task<ToDoItem> AddAsync(ToDoItem todo)
        {
            _context.ToDoItems.Add(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task<ToDoItem> UpdateAsync(ToDoItem todo)
        {
            _context.Entry(todo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task DeleteAsync(ToDoItem todo)
        {
            _context.ToDoItems.Remove(todo);
            await _context.SaveChangesAsync();
        }
    }
}
