using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Authorize]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public TaskController(UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequestDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var toDoListIdClaim = User.FindFirst("ToDoListId")?.Value;
            if (toDoListIdClaim == null)
                return BadRequest("toDoListId cannot be null");

            if (!int.TryParse(toDoListIdClaim, out int toDoListId))
                return BadRequest("Invalid ToDoListId in claims.");

            var user = await _unitOfWork.Users.FindByLamdaAsync(X=>X.Id == userId);
            if (user == null)
                return NotFound(new { message = "User not found" });

            // Find the user ToDoList 
            var toDoList = await _unitOfWork.ToDolist.Entities()
                          .SingleOrDefaultAsync(td => td.ApplicationUserId == userId && td.Id == toDoListId);
            if (toDoList == null)
                return NotFound(new { message = "ToDoList not found" });

            var task = new task()
            {
                Description = model.Description,
                Name = model.Name, 
                CreatedOn = DateTime.UtcNow,
                ToDolistId = toDoListId
            };
            await _unitOfWork.Tasks.CreateEntityAsync(task);
            await _unitOfWork.CompleteAsync();
            var response = new CreateTaskResponseDto()
            {
                Id = task.Id,
                Name = model.Name,
                Description = model.Description,
                CreatedOn = task.CreatedOn,
                toDoListId = toDoListId
            };
            return Ok(response);
        }

        [HttpGet("todolist/{toDoListId}")]
        public async Task<IActionResult> GetAllTasks(int toDoListId, [FromQuery] int currentPage = 1, 
            [FromQuery] int pageSize = 10, [FromQuery] string sortOrder = "asc")
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound(new { message = "User not found" });

            var toDoList = await _unitOfWork.ToDolist.Entities()
                .SingleOrDefaultAsync(td => td.Id == toDoListId && td.ApplicationUserId == user.Id);

            if (toDoList == null)
                return StatusCode(403, new { message = "Access denied: This ToDoList does not belong to you" });

            // Ensure valid pagination values
            currentPage = currentPage < 1 ? 1 : currentPage;
            pageSize = pageSize > 100 ? 100 : (pageSize < 1 ? 10 : pageSize); // Maximum 10 items per page

            // Count the total number of tasks for this ToDoList
            var totalTasks = await _unitOfWork.Tasks.Entities().CountAsync(t => t.ToDolistId == toDoListId);
            if (totalTasks == 0)
                return Ok("No Tasks In Your ToDoList");

            // Calculate total pages (pageCount)
            var pageCount = (int)Math.Ceiling(totalTasks / (double)pageSize);

            // Ensure the current page is not out of bounds
            if (currentPage > pageCount)
                currentPage = pageCount;

            // Apply sorting and pagination
            var tasksQuery = _unitOfWork.Tasks.Entities().Where(t => t.ToDolistId == toDoListId);

            if (sortOrder.ToLower() == "desc")
                tasksQuery = tasksQuery.OrderByDescending(t => t.CreatedOn);
            else
                tasksQuery = tasksQuery.OrderBy(t => t.CreatedOn);

            var tasks = await tasksQuery
                .Skip((currentPage - 1) * pageSize) // Skip records based on the current page
                .Take(pageSize)                     // Limit the number of records returned
                .ToListAsync();

            var response = tasks.Select(task => new CreateTaskResponseDto
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                CreatedOn = task.CreatedOn,
                toDoListId = task.ToDolistId
            }).ToList();

            return Ok(new
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                PageCount = pageCount,
                TotalTasks = totalTasks,
                Tasks = response
            });
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTask(int taskId, [FromBody] UpdateTaskRequestDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var toDoListIdClaim = User.FindFirst("ToDoListId")?.Value;

            if (toDoListIdClaim == null)
                return BadRequest("toDoListId cannot be null");

            if (!int.TryParse(toDoListIdClaim, out int toDoListId))
                return BadRequest("Invalid ToDoListId in claims.");

            var toDoList = await _unitOfWork.ToDolist.Entities()
                .SingleOrDefaultAsync(td => td.Id == toDoListId && td.ApplicationUserId == userId);

            if (toDoList == null)
                return StatusCode(403, new { message = "Access denied: This ToDoList does not belong to you" });

            var task = await _unitOfWork.Tasks.Entities().SingleOrDefaultAsync(t => t.Id == taskId && t.ToDolistId == toDoListId);
            if (task == null)
                return NotFound("Task not found or does not belong to the current user's ToDoList.");

            task.Name = model.Name;
            task.Description = model.Description;
            task.CreatedOn = model.CreatedOn;

            await _unitOfWork.Tasks.UpdateEntityAsync(task);
            await _unitOfWork.CompleteAsync();

            var response = new UpdateTaskResponseDto
            {
                TaskId = task.Id,
                Name = task.Name,
                Description = task.Description,
                CreatedOn = task.CreatedOn
            };
            return Ok(response);
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var toDoListIdClaim = User.FindFirst("ToDoListId")?.Value;

            if (toDoListIdClaim == null)
                return BadRequest("toDoListId cannot be null");

            if (!int.TryParse(toDoListIdClaim, out int toDoListId))
                return BadRequest("Invalid ToDoListId in claims.");

            var toDoList = await _unitOfWork.ToDolist.Entities()
                .SingleOrDefaultAsync(td => td.Id == toDoListId && td.ApplicationUserId == userId);

            if (toDoList == null)
                return StatusCode(403, new { message = "Access denied: This ToDoList does not belong to you" });

            var task = await _unitOfWork.Tasks.Entities().SingleOrDefaultAsync(t => t.Id == taskId && t.ToDolistId == toDoListId);
            if (task == null)
                return NotFound("Task not found or does not belong to the current user's ToDoList.");
            await _unitOfWork.Tasks.DeleteEntityAsync(task);
            await _unitOfWork.CompleteAsync();
            return Ok(new { massge = "Task is deleted" });
        }
    }
}