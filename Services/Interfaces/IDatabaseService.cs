using FitnessTracker.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.WPF.Services.Interfaces
{
    public interface IDatabaseService
    {
        // User methods
        Task<User> AuthenticateUserAsync(string username, string password);
        Task<User> RegisterUserAsync(User user, string password);
        Task<User> GetUserByIdAsync(int userId);
        Task<bool> UpdateUserProfileAsync(User user);

        // Exercise methods
        Task<IEnumerable<Exercise>> GetAllExercisesAsync();
        Task<IEnumerable<Exercise>> GetExercisesByCategoryAsync(ExerciseCategory category);
        Task<Exercise> GetExerciseByIdAsync(int exerciseId);

        // Workout Plan methods
        Task<WorkoutPlan> CreateWorkoutPlanAsync(WorkoutPlan plan);
        Task<IEnumerable<WorkoutPlan>> GetUserWorkoutPlansAsync(int userId);

        // Workout Session methods
        Task<WorkoutSession> CreateWorkoutSessionAsync(WorkoutSession session);
        Task<WorkoutSession> UpdateWorkoutSessionAsync(WorkoutSession session);
        Task<IEnumerable<WorkoutSession>> GetUserWorkoutHistoryAsync(int userId);

        // Progress methods
        Task<ProgressRecord> CreateProgressRecordAsync(ProgressRecord record);
        Task<IEnumerable<ProgressRecord>> GetUserProgressRecordsAsync(int userId);

        // Statistics methods
        Task<Dictionary<string, object>> GetUserStatisticsAsync(int userId, DateTime startDate, DateTime endDate);
    }
}
