using FitnessTracker.WPF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.WPF.DataAccess
{
    public class FitnessDbContext : DbContext
    {
        public FitnessDbContext(DbContextOptions<FitnessDbContext> options)
            : base(options)
        {
        }

        // Khai báo tất cả các bảng trong database
        public DbSet<User> Users { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
        public DbSet<WorkoutPlanExercise> WorkoutPlanExercises { get; set; }
        public DbSet<WorkoutSession> WorkoutSessions { get; set; }
        public DbSet<WorkoutSessionExercise> WorkoutSessionExercises { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<ProgressRecord> ProgressRecords { get; set; }
        public DbSet<NutritionSuggestion> NutritionSuggestions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình lưu Enum dưới dạng String thay vì Int để dễ đọc trong Database
            modelBuilder.Entity<User>()
                .Property(u => u.Gender)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(u => u.FitnessGoal)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(u => u.PreferredEnvironment)
                .HasConversion<string>();

            modelBuilder.Entity<Exercise>()
                .Property(e => e.Category)
                .HasConversion<string>();

            modelBuilder.Entity<Exercise>()
                .Property(e => e.DifficultyLevel)
                .HasConversion<string>();

            modelBuilder.Entity<WorkoutSession>()
                .Property(ws => ws.Status)
                .HasConversion<string>();

            // Cấu hình Unique Index (Ràng buộc duy nhất)
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Cấu hình Relationships (Quan hệ)
            // Khi xóa User -> Xóa luôn WorkoutPlans của User đó (Cascade Delete)
            modelBuilder.Entity<WorkoutPlan>()
                .HasOne(wp => wp.User)
                .WithMany(u => u.WorkoutPlans)
                .HasForeignKey(wp => wp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Định nghĩa khóa composite hoặc các cấu hình đặc biệt khác nếu cần
        }
    }
}