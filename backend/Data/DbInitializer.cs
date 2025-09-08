using backend.Models;
using System.Security.Cryptography;
using System.Text;

namespace backend.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Departments.Any()) return;

            // Créer des départements
            var departments = new Department[]
            {
                new Department { Name = "Ressources Humaines" },
                new Department { Name = "Informatique" },
                new Department { Name = "Finance" },
                new Department { Name = "Marketing" },
                new Department { Name = "Production" }
            };
            context.Departments.AddRange(departments);
            context.SaveChanges();

            // Administrateur par défaut
            var adminUser = new User
            {
                Username = "admin",
                PasswordHash = HashPassword("admin123"),
                FullName = "Administrateur Système",
                Position = "Administrateur",
                HireDate = DateTime.Now.AddYears(-1),
                Salary = 100000,
                Role = UserRole.ADMIN,
                DepartmentId = departments.First(d => d.Name == "Ressources Humaines").Id
            };
            context.Users.Add(adminUser);
            context.SaveChanges();

            // Définir l'admin comme manager du département RH
            var hrDepartment = context.Departments.First(d => d.Name == "Ressources Humaines");
            hrDepartment.ManagerId = adminUser.Id;
            context.SaveChanges();

            // Créer des managers pour chaque département
            var managers = new User[]
            {
                new User
                {
                    Username = "manager.it",
                    PasswordHash = HashPassword("manager123"),
                    FullName = "Sophie Martin",
                    Position = "IT Manager",
                    HireDate = DateTime.Now.AddMonths(-18),
                    Salary = 80000,
                    Role = UserRole.MANAGER,
                    DepartmentId = departments.First(d => d.Name == "Informatique").Id
                },
                new User
                {
                    Username = "manager.finance",
                    PasswordHash = HashPassword("manager123"),
                    FullName = "Pierre Dubois",
                    Position = "Finance Manager",
                    HireDate = DateTime.Now.AddMonths(-24),
                    Salary = 85000,
                    Role = UserRole.MANAGER,
                    DepartmentId = departments.First(d => d.Name == "Finance").Id
                },
                new User
                {
                    Username = "manager.marketing",
                    PasswordHash = HashPassword("manager123"),
                    FullName = "Marie Leroy",
                    Position = "Marketing Manager",
                    HireDate = DateTime.Now.AddMonths(-12),
                    Salary = 78000,
                    Role = UserRole.MANAGER,
                    DepartmentId = departments.First(d => d.Name == "Marketing").Id
                }
            };
            context.Users.AddRange(managers);
            context.SaveChanges();

            // Mettre à jour les départements avec leurs managers
            departments.First(d => d.Name == "Informatique").ManagerId = managers.First(m => m.Position == "IT Manager").Id;
            departments.First(d => d.Name == "Finance").ManagerId = managers.First(m => m.Position == "Finance Manager").Id;
            departments.First(d => d.Name == "Marketing").ManagerId = managers.First(m => m.Position == "Marketing Manager").Id;
            context.SaveChanges();

            // Créer des employés exemple
            var employees = new User[]
            {
                new User
                {
                    Username = "employee1",
                    PasswordHash = HashPassword("employee123"),
                    FullName = "Jean Dupont",
                    Position = "Développeur Frontend",
                    HireDate = DateTime.Now.AddMonths(-6),
                    Salary = 45000,
                    Role = UserRole.EMPLOYE,
                    DepartmentId = departments.First(d => d.Name == "Informatique").Id
                },
                new User
                {
                    Username = "employee2",
                    PasswordHash = HashPassword("employee123"),
                    FullName = "Julie Bernard",
                    Position = "Développeur Backend",
                    HireDate = DateTime.Now.AddMonths(-3),
                    Salary = 48000,
                    Role = UserRole.EMPLOYE,
                    DepartmentId = departments.First(d => d.Name == "Informatique").Id
                },
                new User
                {
                    Username = "employee3",
                    PasswordHash = HashPassword("employee123"),
                    FullName = "Marc Petit",
                    Position = "Analyste Financier",
                    HireDate = DateTime.Now.AddMonths(-9),
                    Salary = 42000,
                    Role = UserRole.EMPLOYE,
                    DepartmentId = departments.First(d => d.Name == "Finance").Id
                },
                new User
                {
                    Username = "employee4",
                    PasswordHash = HashPassword("employee123"),
                    FullName = "Lucie Moreau",
                    Position = "Chargée de Marketing",
                    HireDate = DateTime.Now.AddMonths(-4),
                    Salary = 38000,
                    Role = UserRole.EMPLOYE,
                    DepartmentId = departments.First(d => d.Name == "Marketing").Id
                }
            };
            context.Users.AddRange(employees);
            context.SaveChanges();

            // Créer des tâches exemple
            var tasks = new WorkTask[]
            {
                new WorkTask
                {
                    Title = "Révision des politiques RH",
                    Description = "Réviser et mettre à jour les politiques des ressources humaines pour l'année à venir",
                    DueDate = DateTime.Now.AddDays(30),
                    Status = WorkTaskStatus.EN_COURS,
                    AssignedToId = adminUser.Id,
                    CreatedById = adminUser.Id
                },
                new WorkTask
                {
                    Title = "Développement nouvelle fonctionnalité",
                    Description = "Développer la nouvelle fonctionnalité de rapport pour le module financier",
                    DueDate = DateTime.Now.AddDays(14),
                    Status = WorkTaskStatus.EN_COURS,
                    AssignedToId = employees.First(e => e.FullName == "Jean Dupont").Id,
                    CreatedById = managers.First(m => m.Position == "IT Manager").Id
                },
                new WorkTask
                {
                    Title = "Audit financier trimestriel",
                    Description = "Réaliser l'audit financier du premier trimestre et préparer le rapport",
                    DueDate = DateTime.Now.AddDays(21),
                    Status = WorkTaskStatus.EN_COURS,
                    AssignedToId = employees.First(e => e.FullName == "Marc Petit").Id,
                    CreatedById = managers.First(m => m.Position == "Finance Manager").Id
                },
                new WorkTask
                {
                    Title = "Campagne publicitaire printemps",
                    Description = "Préparer et lancer la campagne publicitaire pour la saison printanière",
                    DueDate = DateTime.Now.AddDays(45),
                    Status = WorkTaskStatus.EN_COURS,
                    AssignedToId = employees.First(e => e.FullName == "Lucie Moreau").Id,
                    CreatedById = managers.First(m => m.Position == "Marketing Manager").Id
                },
                new WorkTask
                {
                    Title = "Migration base de données",
                    Description = "Migrer la base de données vers la nouvelle version de PostgreSQL",
                    DueDate = DateTime.Now.AddDays(7),
                    Status = WorkTaskStatus.TERMINE,
                    AssignedToId = employees.First(e => e.FullName == "Julie Bernard").Id,
                    CreatedById = managers.First(m => m.Position == "IT Manager").Id
                }
            };
            context.WorkTasks.AddRange(tasks);
            context.SaveChanges();
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
