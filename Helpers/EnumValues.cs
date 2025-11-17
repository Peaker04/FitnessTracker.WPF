using FitnessTracker.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.WPF.Helpers
{
    public static class EnumValues
    {
        // Chuyển đổi Array thành IEnumerable để Binding dễ dàng
        public static IEnumerable<FitnessGoal> FitnessGoals =>
            Enum.GetValues(typeof(FitnessGoal)).Cast<FitnessGoal>();

        public static IEnumerable<PreferredEnvironment> Environments =>
            Enum.GetValues(typeof(PreferredEnvironment)).Cast<PreferredEnvironment>();

        public static IEnumerable<ExerciseCategory> ExerciseCategories =>
            Enum.GetValues(typeof(ExerciseCategory)).Cast<ExerciseCategory>();

        public static IEnumerable<DifficultyLevel> DifficultyLevels =>
            Enum.GetValues(typeof(DifficultyLevel)).Cast<DifficultyLevel>();

        public static IEnumerable<Gender> Genders =>
            Enum.GetValues(typeof(Gender)).Cast<Gender>();

        // --- Bổ sung các Enum còn thiếu ---

        public static IEnumerable<UnitSystem> UnitSystems =>
            Enum.GetValues(typeof(UnitSystem)).Cast<UnitSystem>();

        public static IEnumerable<ThemeMode> ThemeModes =>
            Enum.GetValues(typeof(ThemeMode)).Cast<ThemeMode>();

        public static IEnumerable<WorkoutSessionStatus> WorkoutSessionStatuses =>
            Enum.GetValues(typeof(WorkoutSessionStatus)).Cast<WorkoutSessionStatus>();

        public static IEnumerable<AIRequestType> AIRequestTypes =>
            Enum.GetValues(typeof(AIRequestType)).Cast<AIRequestType>();
    }
}
