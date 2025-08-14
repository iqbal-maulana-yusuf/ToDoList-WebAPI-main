using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Dtos.LoginDto;
using ToDoList.Dtos.RegisterDto;
using ToDoList.Models;
using ToDoList.Service;

namespace ToDoList.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signinManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthController(UserManager<AppUser> userManager, IMapper mapper, SignInManager<AppUser> signinManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signinManager = signinManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto requestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = _mapper.Map<AppUser>(requestDto);
            var result = await _userManager.CreateAsync(user, requestDto.Password!);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { message = "User berhasil dibuat", user });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto requestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == requestDto.Email);
            if (user == null) return Unauthorized("Invalid Email!");

            var result = await _signinManager.CheckPasswordSignInAsync(user, requestDto.Password!, false);
            if (!result.Succeeded) return Unauthorized("Incorrect Password!");

            var responsDto = _mapper.Map<LoginResponsDto>(user);
            var token = _tokenService.GenerateToken(user);


            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // Untuk development
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(2), // Sesuaikan dengan expiry token
                Domain = "localhost"
            });
            return Ok(new { message = "Login Berhasil", user = responsDto, token = token });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok(new { message = "Logout Berhasil" });
        }


        [HttpGet("profile")]

        [Authorize]  // Hanya bisa diakses jika user sudah login dengan JWT

        public async Task<IActionResult> GetProfile()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);

            return Ok(new
            {
                UserId = userId,
                Email = user!.Email
            });
        }

    }
}
