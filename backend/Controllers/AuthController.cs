using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services;
using System.Security.Claims;

namespace backend.Controllers
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

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Username and password are required.");

            var loginResponse = await _authService.Login(request.Username, request.Password);
            if (loginResponse == null)
                return Unauthorized("Invalid username or password.");

            return Ok(loginResponse);
        }
    }

    // Controllers/TasksController.cs
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Tous les endpoints nécessitent authentification
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var roleClaim = User.FindFirstValue(ClaimTypes.Role);

            if (!int.TryParse(userIdClaim, out int userId)) return Unauthorized();

            IQueryable<WorkTask> tasksQuery = _context.WorkTasks
                .Include(t => t.AssignedTo)
                .Include(t => t.CreatedBy);

            if (roleClaim == "EMPLOYE")
            {
                tasksQuery = tasksQuery.Where(t => t.AssignedToId == userId);
            }
            else if (roleClaim == "MANAGER")
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                    tasksQuery = tasksQuery.Where(t => t.AssignedTo.DepartmentId == user.DepartmentId);
            }

            var tasks = await tasksQuery.ToListAsync();
            return Ok(tasks);
        }

        // GET: api/tasks/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _context.WorkTasks
                .Include(t => t.AssignedTo)
                .Include(t => t.CreatedBy)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null) return NotFound("Task not found.");
            return Ok(task);
        }

        // POST: api/tasks
        [HttpPost]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public async Task<IActionResult> CreateTask([FromBody] WorkTask task)
        {
            if (task == null) return BadRequest("Task data is required.");

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out int userId)) return Unauthorized();

            task.CreatedById = userId;

            _context.WorkTasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        // PATCH: api/tasks/{id}/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] WorkTaskStatus status)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out int userId)) return Unauthorized();

            var task = await _context.WorkTasks.FindAsync(id);
            if (task == null) return NotFound("Task not found.");

            // Seul le propriétaire de la tâche ou admin/manager peut changer le status
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (task.AssignedToId != userId && role != "ADMIN" && role != "MANAGER")
                return Forbid();

            task.Status = status;
            await _context.SaveChangesAsync();

            return Ok(task);
        }
    }
}
