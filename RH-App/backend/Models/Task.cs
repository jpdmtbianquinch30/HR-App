using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public TaskStatus Status { get; set; } = TaskStatus.EN_COURS;

        [ForeignKey("AssignedTo")]
        public int AssignedToId { get; set; }
        
        [JsonIgnore]
        public virtual User AssignedTo { get; set; }

        [ForeignKey("CreatedBy")]
        public int? CreatedById { get; set; }
        
        [JsonIgnore]
        public virtual User CreatedBy { get; set; }
    }

    public enum TaskStatus
    {
        EN_COURS,
        TERMINE
    }
}