using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.WPF.Models
{
    [Table("WorkoutPlanExercises")]
    public class WorkoutPlanExercise
    {
        [Key]
        public int WorkoutPlanExerciseId { get; set; }

        public int WorkoutPlanId { get; set; }

        public int ExerciseId { get; set; }

        public int DayNumber { get; set; } // 1-7

        public int OrderIndex { get; set; }

        public int? PlannedSets { get; set; } = 3;

        public int? PlannedReps { get; set; } = 10;

        [Column(TypeName = "decimal(6,2)")]
        public decimal? PlannedWeight { get; set; }

        public int? PlannedRestSeconds { get; set; } = 60;

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("WorkoutPlanId")]
        public virtual WorkoutPlan WorkoutPlan { get; set; }

        [ForeignKey("ExerciseId")]
        public virtual Exercise Exercise { get; set; }
    }
}
