using FitnessTracker.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.WPF.Services.Interfaces
{
    public interface IAIService
    {
        bool IsEnabled { get; set; }
        Task<string> GenerateWorkoutPlanAsync(FitnessGoal goal, PreferredEnvironment environment, int daysPerWeek);
        Task<string> GenerateNutritionAdviceAsync(WorkoutSession session, User user);
        Task<IEnumerable<Exercise>> SuggestExercisesAsync(FitnessGoal goal, PreferredEnvironment environment);
    }
}
