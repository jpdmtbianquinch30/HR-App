using backend.Models;

namespace backend.DTOs
{
    // DTO pour la requête de connexion
    public class LoginRequest
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    // DTO pour la réponse après connexion
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public UserDto User { get; set; } = null!;
    }

    // DTO pour représenter un utilisateur
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Position { get; set; } = null!;
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
        public UserRole Role { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
    }

    // DTO pour représenter un département
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public int EmployeeCount { get; set; }
    }

    // DTO pour représenter une tâche
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; }
        public int AssignedToId { get; set; }
        public string AssignedToName { get; set; } = null!;
        public int? CreatedById { get; set; }
        public string? CreatedByName { get; set; }
    }
}
