using Microsoft.EntityFrameworkCore;
using PetAdoptApp.Models;

namespace PetAdoptApp.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; } = null!;
        public DbSet<TrainingEntity> TrainingEntities { get; set; } = null!;
        public DbSet<ExerciseEntity> ExerciseEntities { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory + "TrainingDb.db");
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrainingEntity>()
                .HasMany(t => t.Exercises)
                .WithMany(e => e.Trainings)
                .UsingEntity(j => j.ToTable("TrainingExercises"));

            modelBuilder.Entity<TrainingEntity>()
                .HasOne(t => t.User)
                .WithMany(u => u.Trainings)
                .HasForeignKey(t => t.Id_User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserEntity>(userConf =>
            {
                userConf.HasKey(p => p.Id_User);
            });

            modelBuilder.Entity<TrainingEntity>(trainingConf =>
            {
                trainingConf.HasKey(p => p.Id_Training);
            });

            modelBuilder.Entity<ExerciseEntity>(trexConf =>
            {
                trexConf.HasKey(p => p.Id_Exercise);
            });
        }

        public static void Initialize(AppDbContext dbContext)
        {
            
            if (!dbContext.ExerciseEntities.Any())
            {
                dbContext.ExerciseEntities.AddRange(
                    new ExerciseEntity { Id_Exercise = 1, Title = "Приседания", Description = "Упражнение для ног" },
                    new ExerciseEntity { Id_Exercise = 2, Title = "Отжимания", Description = "Упражнение для рук" },
                    new ExerciseEntity { Id_Exercise = 3, Title = "Планка", Description = "Упражнение для корпуса" },
                    new ExerciseEntity { Id_Exercise = 4, Title = "Бег на месте", Description = "Кардио упражнение" },
                    new ExerciseEntity { Id_Exercise = 5, Title = "Подтягивания", Description = "Упражнение для спины" }
                );
                dbContext.SaveChanges();
            }

           
            var users = new List<UserEntity>
            {
                new UserEntity { Id_User = 1, Surname = "Иванов", Name = "Иван", Patronymic = "Иванович", Login = "qq", Password = "qq" },
                new UserEntity { Id_User = 2, Surname = "Петров", Name = "Петр", Patronymic = "Петрович", Login = "ww", Password = "ww" },
                new UserEntity { Id_User = 3, Surname = "Алексеев", Name = "Алексей", Patronymic = "Алексеевич", Login = "ee", Password = "ee" },
                new UserEntity { Id_User = 4, Surname = "Комаров", Name = "Влад", Patronymic = "Андреевич", Login = "rr", Password = "rr" },
                new UserEntity { Id_User = 5, Surname = "Булатова", Name = "Милена", Patronymic = "Илдаровна", Login = "tt", Password = "tt" }
            };
            if (!dbContext.Users.Any())
            {

                dbContext.Users.AddRange(users);
                dbContext.SaveChanges();

            }
            if (!dbContext.TrainingEntities.Any())
            {
                var user1 = dbContext.Users.FirstOrDefault(u => u.Id_User == 1);
                var user2 = dbContext.Users.FirstOrDefault(u => u.Id_User == 2);
                var user3 = dbContext.Users.FirstOrDefault(u => u.Id_User == 3);
                var user4 = dbContext.Users.FirstOrDefault(u => u.Id_User == 4);
                var user5 = dbContext.Users.FirstOrDefault(u => u.Id_User == 5);

                if (user1 == null || user2 == null || user3 == null || user4 == null || user5 == null)
                {
                    throw new Exception("Один или несколько пользователей не найдены в базе данных.");
                }


                dbContext.TrainingEntities.AddRange(
                    new TrainingEntity { Id_Training = 1, Title = "Тренировка 1", Description = "ляля", DataCreate = new DateTime(2025, 01, 01), User= users[0] },
                    new TrainingEntity { Id_Training = 2, Title = "Тренировка 2", Description = "тополя", DataCreate = new DateTime(2025, 01, 01), User = users[1] },
                    new TrainingEntity { Id_Training = 3, Title = "Тренировка 3", Description = "ляля", DataCreate = new DateTime(2025, 01, 01), User = users[2] },
                    new TrainingEntity { Id_Training = 4, Title = "Тренировка 4", Description = "караганда", DataCreate = new DateTime(2025, 01, 01), User = users[3] },
                    new TrainingEntity { Id_Training = 5, Title = "Тренировка 5", Description = "ляля", DataCreate = new DateTime(2025, 01, 01), User = users[4] }
                );

                dbContext.SaveChanges();
            }
        }
    }
}
