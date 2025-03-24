using Microsoft.EntityFrameworkCore;
using PetAdoptApp.Models;

namespace PetAdoptApp.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory + "PetAdoptDb.db");
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(userConf =>
            {
                userConf.HasKey(p => p.Id_User);
            });
        }

        public static void Initialize(AppDbContext dbContext)
        {
            
           
           
            var users = new List<UserEntity>
            {
                new UserEntity { Id_User = 1, Surname = "Иванов", Name = "Иван", Patronymic = "Иванович", Email = "qq", Password = "qq" },
                new UserEntity { Id_User = 2, Surname = "Петров", Name = "Петр", Patronymic = "Петрович", Email = "ww", Password = "ww" },
                new UserEntity { Id_User = 3, Surname = "Алексеев", Name = "Алексей", Patronymic = "Алексеевич", Email = "ee", Password = "ee" },
                new UserEntity { Id_User = 4, Surname = "Комаров", Name = "Влад", Patronymic = "Андреевич", Email = "rr", Password = "rr" },
                new UserEntity { Id_User = 5, Surname = "Булатова", Name = "Милена", Patronymic = "Илдаровна", Email = "tt", Password = "tt" }
            };
            if (!dbContext.Users.Any())
            {

                dbContext.Users.AddRange(users);
                dbContext.SaveChanges();

            }
           
        }
    }
}
