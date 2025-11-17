using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.WPF.Models
{
    [Table("Sets")]
    public class Set
    {
        [Key]
        public int SetId { get; set; }

        public int WorkoutSessionExerciseId { get; set; }

        public int SetNumber { get; set; }

        public int Reps { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal? Weight { get; set; } // kg

        public int? RestSeconds { get; set; }

        [Range(1, 5)]
        public int? FormRating { get; set; }

        [Range(1, 5)]
        public int? DifficultyRating { get; set; }

        public DateTime CompletedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("WorkoutSessionExerciseId")]
        public virtual WorkoutSessionExercise WorkoutSessionExercise { get; set; }
    }
}