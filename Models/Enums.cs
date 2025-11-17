using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.WPF.Models
{
    public enum Gender
    {
        Male,
        Female,
        Other
    }

    public enum FitnessGoal
    {
        MuscleBuilding,
        WeightLoss,
        Endurance
    }

    public enum PreferredEnvironment
    {
        Home,
        Gym,
        Both
    }

    public enum ExerciseCategory
    {
        Chest,
        Back,
        Legs,
        Shoulders,
        Arms,
        Core,
        Cardio,
        FullBody
    }

    public enum DifficultyLevel
    {
        Beginner,
        Intermediate,
        Advanced
    }

    public enum WorkoutSessionStatus
    {
        Planned,
        InProgress,
        Completed,
        Skipped
    }

    public enum ThemeMode
    {
        Light,
        Dark
    }

    public enum UnitSystem
    {
        Metric,
        Imperial
    }

    public enum AIRequestType
    {
        WorkoutPlan,
        NutritionAdvice,
        ExerciseSuggestion
    }
}
