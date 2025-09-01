using Microsoft.EntityFrameworkCore;
using RH_App.Models;

namespace RH_App.Data
{
  
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration des relations et contraintes
            modelBuilder.Entity<User>()
                .HasOne(u => u.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(u => u.DepartmentId);

            modelBuilder.Entity<Department>()
                .HasOne(d => d.Manager)
                .WithMany()
                .HasForeignKey(d => d.ManagerId);

            modelBuilder.Entity<Task>()
                .HasOne(t => t.AssignedTo)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.AssignedToId);

            modelBuilder.Entity<Task>()
                .HasOne(t => t.CreatedBy)
                .WithMany()
                .HasForeignKey(t => t.CreatedById);
        }
    }
}
