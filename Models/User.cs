using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.WPF.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required, MaxLength(255)]
        public string PasswordHash { get; set; }

        [Required, MaxLength(255)]
        public string PasswordSalt { get; set; }

        [MaxLength(100)]
        public string FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public Gender Gender { get; set; } = Gender.Other;

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Height { get; set; } // cm

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Weight { get; set; } // kg

        public FitnessGoal FitnessGoal { get; set; } = FitnessGoal.WeightLoss;

        public PreferredEnvironment PreferredEnvironment { get; set; } = PreferredEnvironment.Home;

        public bool IsAIEnabled { get; set; } = true;

        public UnitSystem PreferredUnit { get; set; } = UnitSystem.Metric;

        public ThemeMode ThemeMode { get; set; } = ThemeMode.Light;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public DateTime? LastLoginAt { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public virtual ICollection<WorkoutPlan> WorkoutPlans { get; set; }
        public virtual ICollection<WorkoutSession> WorkoutSessions { get; set; }
        public virtual ICollection<ProgressRecord> ProgressRecords { get; set; }
    }
}
