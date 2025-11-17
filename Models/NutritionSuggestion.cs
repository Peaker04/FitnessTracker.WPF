using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.WPF.Models
{
    [Table("NutritionSuggestions")]
    public class NutritionSuggestion
    {
        [Key]
        public int NutritionSuggestionId { get; set; }

        public int UserId { get; set; }

        public int? WorkoutSessionId { get; set; }

        public DateTime SuggestionDate { get; set; }

        public int? RecommendedCalories { get; set; }

        public int? RecommendedProteinGrams { get; set; }

        public int? RecommendedCarbsGrams { get; set; }

        public int? RecommendedFatsGrams { get; set; }

        public string PreWorkoutMeal { get; set; }

        public string PostWorkoutMeal { get; set; }

        public string DailyMealPlan { get; set; }

        public bool IsAIGenerated { get; set; } = true;

        public string AIPrompt { get; set; }

        public string AIResponse { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("WorkoutSessionId")]
        public virtual WorkoutSession WorkoutSession { get; set; }
    }
}