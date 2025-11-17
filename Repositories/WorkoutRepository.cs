using FitnessTracker.WPF.DataAccess;
using FitnessTracker.WPF.Models;
using FitnessTracker.WPF.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.WPF.Repositories
{
    public class WorkoutRepository : IWorkoutRepository
    {
        private readonly FitnessDbContext _context;

        public WorkoutRepository(FitnessDbContext context)
        {
            _context = context;
        }

        // --- Workout Plans ---

        public async Task<WorkoutPlan> CreatePlanAsync(WorkoutPlan plan)
        {
            _context.WorkoutPlans.Add(plan);
            await _context.SaveChangesAsync();
            return plan;
        }

        public async Task<WorkoutPlan> GetPlanByIdAsync(int planId)
        {
            return await _context.WorkoutPlans
                .Include(wp => wp.Exercises)
                .ThenInclude(wpe => wpe.Exercise)
                .FirstOrDefaultAsync(wp => wp.WorkoutPlanId == planId);
        }

        public async Task<IEnumerable<WorkoutPlan>> GetUserPlansAsync(int userId)
        {
            return await _context.WorkoutPlans
                .Where(wp => wp.UserId == userId && wp.IsActive)
                .Include(wp => wp.Exercises)
                .OrderByDescending(wp => wp.CreatedAt)
                .ToListAsync();
        }

        public async Task<WorkoutPlan> UpdatePlanAsync(WorkoutPlan plan)
        {
            plan.UpdatedAt = DateTime.Now;
            _context.WorkoutPlans.Update(plan);
            await _context.SaveChangesAsync();
            return plan;
        }

        public async Task<bool> DeletePlanAsync(int planId)
        {
            var plan = await _context.WorkoutPlans.FindAsync(planId);
            if (plan == null) return false;

            _context.WorkoutPlans.Remove(plan);
            await _context.SaveChangesAsync();
            return true;
        }

        // --- Workout Sessions ---

        public async Task<WorkoutSession> CreateSessionAsync(WorkoutSession session)
        {
            _context.WorkoutSessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<WorkoutSession> GetSessionByIdAsync(int sessionId)
        {
            return await _context.WorkoutSessions
                .Include(ws => ws.Exercises)
                    .ThenInclude(wse => wse.Exercise)
                .Include(ws => ws.Exercises)
                    .ThenInclude(wse => wse.Sets)
                .FirstOrDefaultAsync(ws => ws.WorkoutSessionId == sessionId);
        }

        public async Task<IEnumerable<WorkoutSession>> GetUserSessionsAsync(int userId)
        {
            return await _context.WorkoutSessions
                .Where(ws => ws.UserId == userId)
                .OrderByDescending(ws => ws.SessionDate)
                .ToListAsync();
        }

        public async Task<WorkoutSession> UpdateSessionAsync(WorkoutSession session)
        {
            session.UpdatedAt = DateTime.Now;
            _context.WorkoutSessions.Update(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<IEnumerable<WorkoutSession>> GetUserSessionsInRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            return await _context.WorkoutSessions
                .Where(ws => ws.UserId == userId &&
                             ws.SessionDate >= startDate &&
                             ws.SessionDate <= endDate)
                .OrderBy(ws => ws.SessionDate)
                .ToListAsync();
        }
    }
}
