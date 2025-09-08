using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models
{
    public class WorkTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public WorkTaskStatus Status { get; set; } = WorkTaskStatus.EN_COURS;

        [ForeignKey("AssignedTo")]
        public int AssignedToId { get; set; }

        [JsonIgnore]
        public virtual User AssignedTo { get; set; } = null!;

        [ForeignKey("CreatedBy")]
        public int? CreatedById { get; set; }

        [JsonIgnore]
        public virtual User? CreatedBy { get; set; }
    }

    public enum WorkTaskStatus
    {
        EN_COURS,
        TERMINE
    }
}
