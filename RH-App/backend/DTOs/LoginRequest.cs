namespace backend.DTOs
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
        public UserRole Role { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }

    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ManagerId { get; set; }
        public string ManagerName { get; set; }
        public int EmployeeCount { get; set; }
    }

    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; }
        public int AssignedToId { get; set; }
        public string AssignedToName { get; set; }
        public int? CreatedById { get; set; }
        public string CreatedByName { get; set; }
    }
}