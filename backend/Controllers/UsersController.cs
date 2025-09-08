using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;
using System.Security.Claims;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Autorisation globale, certaines actions restrictives via Roles
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // ===================== USERS =====================

        // GET: api/users
        [HttpGet]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
                .Include(u => u.Department)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    FullName = u.FullName,
                    Position = u.Position,
                    HireDate = u.HireDate,
                    Salary = u.Salary,
                    Role = u.Role,
                    DepartmentId = u.DepartmentId,
                    DepartmentName = u.Department != null ? u.Department.Name : null
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound("User not found.");

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Position = user.Position,
                HireDate = user.HireDate,
                Salary = user.Salary,
                Role = user.Role,
                DepartmentId = user.DepartmentId,
                DepartmentName = user.Department != null ? user.Department.Name : null
            };

            return Ok(userDto);
        }

        // POST: api/users
        [HttpPost]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (user == null) return BadRequest("User data is required.");

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound("User not found.");

            user.FullName = updatedUser.FullName;
            user.Position = updatedUser.Position;
            user.Salary = updatedUser.Salary;
            user.Role = updatedUser.Role;
            user.DepartmentId = updatedUser.DepartmentId;

            await _context.SaveChangesAsync();
            return Ok(user);
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound("User not found.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ===================== TASKS =====================

        // GET: api/users/tasks
        [HttpGet("tasks")]
        public async Task<IActionResult> GetTasks()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var userRole = User.FindFirstValue(ClaimTypes.Role)!;

            IQueryable<WorkTask> tasksQuery = _context.WorkTasks
                .Include(t => t.AssignedTo)
                .Include(t => t.CreatedBy);

            if (userRole == "EMPLOYE")
                tasksQuery = tasksQuery.Where(t => t.AssignedToId == userId);
            else if (userRole == "MANAGER")
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                    tasksQuery = tasksQuery.Where(t => t.AssignedTo.DepartmentId == user.DepartmentId);
            }

            var tasks = await tasksQuery.ToListAsync();
            return Ok(tasks);
        }

        // POST: api/users/tasks
        [HttpPost("tasks")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public async Task<IActionResult> CreateTask([FromBody] WorkTask task)
        {
            if (task == null) return BadRequest("Task data is required.");

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            task.CreatedById = userId;
            task.Status = WorkTaskStatus.EN_COURS;

            _context.WorkTasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        // GET: api/users/tasks/{id}
        [HttpGet("tasks/{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _context.WorkTasks
                .Include(t => t.AssignedTo)
                .Include(t => t.CreatedBy)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null) return NotFound("Task not found.");
            return Ok(task);
        }

        // PATCH: api/users/tasks/{id}/status
        [HttpPatch("tasks/{id}/status")]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] WorkTaskStatus status)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var task = await _context.WorkTasks.FindAsync(id);

            if (task == null) return NotFound("Task not found.");
            if (task.AssignedToId != userId && !User.IsInRole("ADMIN") && !User.IsInRole("MANAGER"))
                return Forbid();

            task.Status = status;
            await _context.SaveChangesAsync();

            return Ok(task);
        }

        // PUT: api/users/tasks/{id}
        [HttpPut("tasks/{id}")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] WorkTask updatedTask)
        {
            var task = await _context.WorkTasks.FindAsync(id);
            if (task == null) return NotFound("Task not found.");

            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.DueDate = updatedTask.DueDate;
            task.AssignedToId = updatedTask.AssignedToId;
            task.Status = updatedTask.Status;

            await _context.SaveChangesAsync();
            return Ok(task);
        }

        // DELETE: api/users/tasks/{id}
        [HttpDelete("tasks/{id}")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.WorkTasks.FindAsync(id);
            if (task == null) return NotFound("Task not found.");

            _context.WorkTasks.Remove(task);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
