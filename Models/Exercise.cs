using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.WPF.Models
{
    [Table("Exercises")]
    public class Exercise
    {
        [Key]
        public int ExerciseId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public ExerciseCategory Category { get; set; }

        [MaxLength(100)]
        public string MuscleGroup { get; set; }

        public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.Beginner;

        [MaxLength(255)]
        public string EquipmentNeeded { get; set; }

        public PreferredEnvironment Environment { get; set; } = PreferredEnvironment.Both;

        public string Instructions { get; set; }

        [MaxLength(500)]
        public string VideoUrl { get; set; }

        [MaxLength(500)]
        public string ImageUrl { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? EstimatedCaloriesPerRep { get; set; }

        public int DefaultSets { get; set; } = 3;
        public int DefaultReps { get; set; } = 10;
        public int DefaultRestSeconds { get; set; } = 60;

        public int? CreatedBy { get; set; }
        public bool IsPublic { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("CreatedBy")]
        public virtual User Creator { get; set; }
    }
}
