using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.WPF.Models
{
    [Table("WorkoutSessionExercises")]
    public class WorkoutSessionExercise
    {
        [Key]
        public int WorkoutSessionExerciseId { get; set; }

        public int WorkoutSessionId { get; set; }

        public int ExerciseId { get; set; }

        public int OrderIndex { get; set; }

        // Kế hoạch ban đầu cho bài này
        public int? PlannedSets { get; set; }
        public int? PlannedReps { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public decimal? PlannedWeight { get; set; }

        // Thực tế
        public int CompletedSets { get; set; } = 0;

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("WorkoutSessionId")]
        public virtual WorkoutSession WorkoutSession { get; set; }

        [ForeignKey("ExerciseId")]
        public virtual Exercise Exercise { get; set; }

        public virtual ICollection<Set> Sets { get; set; }
    }
}
