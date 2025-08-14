using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Dtos.ToDoDto;
using ToDoList.Interface;
using ToDoList.Services;

namespace ToDoList.Controllers
{
    [Route("api/todo")]
    [ApiController]
    [Authorize]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _toDoService;

        public ToDoController(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }

        // GET: semua ToDo user
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.Claims.First(c => c.Type == "userId").Value;
            var todos = await _toDoService.GetAllUserToDosAsync(userId);
            return Ok(todos);
        }

        // GET: single ToDo
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userId = User.Claims.First(c => c.Type == "userId").Value;
            var todo = await _toDoService.GetToDoByIdAsync(id, userId);
            if (todo == null) return NotFound();
            return Ok(todo);
        }

        // POST: create ToDo
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ToDoCreateDto dto)
        {
            var userId = User.Claims.First(c => c.Type == "userId").Value;
            var todo = await _toDoService.CreateToDoAsync(dto, userId);
            return Ok(todo);
        }

        // PUT: update ToDo
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ToDOUpdateDto dto)
        {
            var userId = User.Claims.First(c => c.Type == "userId").Value;
            var updatedTodo = await _toDoService.UpdateToDoAsync(id, dto, userId);
            if (updatedTodo == null) return NotFound();
            return Ok(updatedTodo);
        }

        // DELETE: hapus ToDo
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.Claims.First(c => c.Type == "userId").Value;
            var success = await _toDoService.DeleteToDoAsync(id, userId);
            if (!success) return NotFound();
            return Ok(new { message = "ToDo deleted successfully" });
        }
    }
}
