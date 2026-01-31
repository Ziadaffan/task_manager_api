using Domaine.Classes;
using Domaine.Enum;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DbInitializer
    {
        public static async System.Threading.Tasks.Task SeedAsync(TaskManagerContext context)
        {
            if (!context.Users.Any(u => u.Username == "admin"))
            {
                var adminUser = new User(
                    "admin",
                    "admin@gmail.com",
                    HashPassword("admin"),
                    null,
                    Domaine.Enum.Role.Admin
                );
                context.Users.Add(adminUser);
                await context.SaveChangesAsync();
            }
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
