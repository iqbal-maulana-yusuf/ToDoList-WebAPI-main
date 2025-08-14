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
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signinManager;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AuthController(UserManager<AppUser> userManager, IMapper mapper, SignInManager<AppUser> signinManager, ITokenService tokenService, IUserService userService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signinManager = signinManager;
            _tokenService = tokenService;
            _userService = userService;
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
            try
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
                    Secure = false,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddDays(2),
                    Domain = "localhost"
                });
                return Ok(new { message = "Login Berhasil", user = responsDto, token = token });
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            try
            {
                Response.Cookies.Delete("jwt");
                return Ok(new { message = "Logout Berhasil" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("profile")]
        [Authorize]

        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);

                return Ok(new
                {
                    UserId = userId,
                    Email = user!.Email
                });
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
                var users = await _userService.GetAll();
                return Ok(users);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

    }
}
