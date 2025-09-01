using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH_App.Data;
using RH_App.Models;
using RH_App.Services;
using System.Security.Claims;

namespace RH_App.Controllers
{
    // Controllers/AuthController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authService.Login(request.Username, request.Password);
            if (token == null) return Unauthorized();

            return Ok(new { Token = token });
        }
    }

    // Controllers/TasksController.cs
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userRole = User.FindFirst(ClaimTypes.Role).Value;

            IQueryable<Task> tasksQuery = _context.Tasks.Include(t => t.AssignedTo);

            if (userRole == "EMPLOYE")
            {
                tasksQuery = tasksQuery.Where(t => t.AssignedToId == userId);
            }
            else if (userRole == "MANAGER")
            {
                var user = await _context.Users.FindAsync(userId);
                tasksQuery = tasksQuery.Where(t => t.AssignedTo.DepartmentId == user.DepartmentId);
            }

            var tasks = await tasksQuery.ToListAsync();
            return Ok(tasks);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public async Task<IActionResult> CreateTask([FromBody] Task task)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            task.CreatedById = userId;

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] TaskStatus status)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var task = await _context.Tasks.FindAsync(id);

            if (task == null) return NotFound();
            if (task.AssignedToId != userId) return Forbid();

            task.Status = status;
            await _context.SaveChangesAsync();

            return Ok(task);
        }
    }
}
