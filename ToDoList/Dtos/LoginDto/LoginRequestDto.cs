using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Dtos.LoginDto
{
    public class LoginRequestDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}