using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Dtos.ToDoDto
{
    public class ToDoCreateDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}