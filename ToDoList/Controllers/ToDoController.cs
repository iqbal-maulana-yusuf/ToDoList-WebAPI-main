using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Dtos.ToDoDto;
using ToDoList.Service;

namespace ToDoList.Controllers
{

    [Route("api/todo")]
    
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private IToDoService _todoService;

        public ToDoController(IToDoService toDoService)
        {
            _todoService = toDoService;

        }

        [HttpGet]

        public async Task<IActionResult> GetAllToDo()
        {
            try
            {
                var userId = User.Claims.First(c => c.Type == "userId").Value;
                var todos = await _todoService.GetAll(userId);
                return Ok(todos);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        [Route("{id}")]
        [HttpGet]
        

        public async Task<IActionResult> GetToDoById([FromRoute] int id)
        {
            try
            {
                var userId = User.Claims.First(c => c.Type == "userId").Value;
                var todo = await _todoService.GetById(userId, id);
                if (todo == null)
                {
                    return NotFound();
                }
                return Ok(todo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateToDo([FromBody] ToDoCreateRequestDto createDto)
        {
            try
            {
                var userId = User.Claims.First(c => c.Type == "userId").Value;
                var todo = await _todoService.Create(createDto, userId);
                return Ok(todo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDo([FromBody] ToDoUpdateDto updateDto, int id)
        {
            try
            {
                var userId = User.Claims.First(c => c.Type == "userId").Value;
                var todo = await _todoService.Update(updateDto, userId, id);
                return Ok(todo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDo(int id)
        {
            try
            {
                var userId = User.Claims.First(c => c.Type == "userId").Value;
                await _todoService.Delete(userId, id);
                return Ok("Success Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}