using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToDoList.Api.Dtos;
using ToDoList.Core.Models;
using ToDoList.Core.Services;
using ToDoList.Core.UnitOfWork;

namespace ToDoList.Api.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IUnitOfWork unitOfWork, 
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterDto model)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.UserName,
            };
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                // If email already exists, return a BadRequest response
                return BadRequest(new { message = "Email is already registered" });
            }

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            await _userManager.AddToRoleAsync(user, "User");

            ToDolist todolist = new ToDolist { ApplicationUserId = user.Id };
            var Result = await _unitOfWork.ToDolist.CreateEntityAsync(todolist);
            if (Result is null)
                return BadRequest("todolist can not be null");
            await _unitOfWork.CompleteAsync();

            var token = await _tokenService.GenerateToken(user, todolist.Id);
            var respoonse = new RegisterResponseDto
            {
                Id = user.Id,
                Email = model.Email,
                UserName = model.UserName,
                Token = token,
                UserToDoListId = todolist.Id
            };
            return Ok(respoonse);
        }
        [HttpPost()]
        public async Task<IActionResult> Login([FromForm] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
                return Unauthorized("Invalid credentials");

            var toDoList = await _unitOfWork.ToDolist.FindByLamdaAsync(t => t.ApplicationUserId == user.Id);
            if (toDoList == null)
                return NotFound("No associated ToDoList found.");

            var token = await _tokenService.GenerateToken(user,toDoList.Id);
            var response = new LoginResponseDto
            {
                Id=user.Id,
                UserName = user.UserName!,
                Token = token,
                UserToDoListId = toDoList.Id
            };
            return Ok(response);
        }
    }
}