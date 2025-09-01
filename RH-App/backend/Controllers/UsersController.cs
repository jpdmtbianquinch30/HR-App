using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.DTOs;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        public UsersController(AppDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userRole = User.FindFirst(ClaimTypes.Role).Value;
            
            IQueryable<User> usersQuery = _context.Users.Include(u => u.Department);
            
            if (userRole == "MANAGER")
            {
                var currentUser = await _context.Users.FindAsync(userId);
                usersQuery = usersQuery.Where(u => u.DepartmentId == currentUser.DepartmentId);
            }
            else if (userRole == "EMPLOYE")
            {
                return Forbid(); // Les employés ne peuvent pas voir tous les utilisateurs
            }
            
            var users = await usersQuery.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                FullName = u.FullName,
                Position = u.Position,
                HireDate = u.HireDate,
                Salary = u.Salary,
                Role = u.Role,
                DepartmentId = u.DepartmentId,
                DepartmentName = u.Department.Name
            }).ToListAsync();
            
            return Ok(users);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userRole = User.FindFirst(ClaimTypes.Role).Value;
            
            var user = await _context.Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == id);
                
            if (user == null) return NotFound();
            
            // Vérifier les permissions
            if (userRole == "MANAGER")
            {
                var currentUser = await _context.Users.FindAsync(userId);
                if (user.DepartmentId != currentUser.DepartmentId) return Forbid();
            }
            else if (userRole == "EMPLOYE" && user.Id != userId)
            {
                return Forbid();
            }
            
            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Position = u.Position,
                HireDate = u.HireDate,
                Salary = u.Salary,
                Role = u.Role,
                DepartmentId = u.DepartmentId,
                DepartmentName = u.Department.Name
            };
            
            return Ok(userDto);
        }
    }
}