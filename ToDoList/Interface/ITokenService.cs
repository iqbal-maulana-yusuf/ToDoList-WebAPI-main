using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoList.Models;

namespace ToDoList.Service
{
    public interface ITokenService
    {
        string GenerateToken(AppUser user);
        ClaimsPrincipal? ValidateToken(string token);
    }
}