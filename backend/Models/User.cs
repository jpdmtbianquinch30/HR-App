using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        [JsonIgnore]
        public string PasswordHash { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Position { get; set; } = null!;

        [Required]
        public DateTime HireDate { get; set; }

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }

        [JsonIgnore]
        public virtual Department? Department { get; set; }

        [JsonIgnore]
        public virtual ICollection<WorkTask> WorkTasks { get; set; } = new List<WorkTask>();
    }

    public enum UserRole
    {
        ADMIN,
        MANAGER,
        EMPLOYE
    }
}
