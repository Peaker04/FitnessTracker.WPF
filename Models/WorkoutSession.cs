using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.WPF.Models
{
    [Table("WorkoutSessions")]
    public class WorkoutSession
    {
        [Key]
        public int WorkoutSessionId { get; set; }

        public int UserId { get; set; }

        public int? WorkoutPlanId { get; set; } // Có thể null nếu tập tự do

        [MaxLength(100)]
        public string SessionName { get; set; }

        public DateTime SessionDate { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? TotalDurationMinutes { get; set; }

        public int? TotalCaloriesBurned { get; set; }

        public int? AverageHeartRate { get; set; }

        [Range(1, 5)]
        public int? DifficultyRating { get; set; }

        [Range(1, 5)]
        public int? EnergyLevel { get; set; }

        public string Notes { get; set; }

        public WorkoutSessionStatus Status { get; set; } = WorkoutSessionStatus.Planned;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("WorkoutPlanId")]
        public virtual WorkoutPlan WorkoutPlan { get; set; }

        public virtual ICollection<WorkoutSessionExercise> Exercises { get; set; }
        public virtual ICollection<NutritionSuggestion> NutritionSuggestions { get; set; }
    }
}
