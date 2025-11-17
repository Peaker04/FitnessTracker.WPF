using FitnessTracker.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.WPF.Repositories.Interfaces
{
    public interface IWorkoutRepository
    {
        // Workout Plans
        Task<WorkoutPlan> CreatePlanAsync(WorkoutPlan plan);
        Task<WorkoutPlan> GetPlanByIdAsync(int planId);
        Task<IEnumerable<WorkoutPlan>> GetUserPlansAsync(int userId);
        Task<WorkoutPlan> UpdatePlanAsync(WorkoutPlan plan);
        Task<bool> DeletePlanAsync(int planId);

        // Workout Sessions
        Task<WorkoutSession> CreateSessionAsync(WorkoutSession session);
        Task<WorkoutSession> GetSessionByIdAsync(int sessionId);
        Task<IEnumerable<WorkoutSession>> GetUserSessionsAsync(int userId);
        Task<WorkoutSession> UpdateSessionAsync(WorkoutSession session);
        Task<IEnumerable<WorkoutSession>> GetUserSessionsInRangeAsync(int userId, DateTime startDate, DateTime endDate);
    }
}
