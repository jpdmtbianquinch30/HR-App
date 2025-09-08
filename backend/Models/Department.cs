using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; // Valeur par défaut pour éviter l'avertissement

        [ForeignKey("Manager")]
        public int? ManagerId { get; set; }

        [JsonIgnore]
        public virtual User Manager { get; set; } = null!; // Valeur par défaut "null-forcé" pour supprimer l'avertissement

        [JsonIgnore]
        public virtual ICollection<User> Employees { get; set; } = new List<User>();
    }
}
