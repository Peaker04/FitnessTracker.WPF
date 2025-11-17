using FitnessTracker.WPF.Models;
using FitnessTracker.WPF.Repositories.Interfaces;
using FitnessTracker.WPF.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.WPF.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IUserRepository _userRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IProgressRepository _progressRepository;

        public DatabaseService(
            IUserRepository userRepository,
            IExerciseRepository exerciseRepository,
            IWorkoutRepository workoutRepository,
            IProgressRepository progressRepository)
        {
            _userRepository = userRepository;
            _exerciseRepository = exerciseRepository;
            _workoutRepository = workoutRepository;
            _progressRepository = progressRepository;
        }

        public async Task<User> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) return null;

            var hashedPassword = HashPassword(password, user.PasswordSalt);
            if (hashedPassword != user.PasswordHash) return null;

            user.LastLoginAt = DateTime.Now;
            await _userRepository.UpdateAsync(user);

            return user;
        }

        public async Task<User> RegisterUserAsync(User user, string password)
        {
            // Check if username or email exists
            if (await _userRepository.ExistsAsync(user.Username, user.Email))
            {
                throw new Exception("Username or email already exists");
            }

            // Generate salt and hash password
            user.PasswordSalt = GenerateSalt();
            user.PasswordHash = HashPassword(password, user.PasswordSalt);

            return await _userRepository.CreateAsync(user);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<bool> UpdateUserProfileAsync(User user)
        {
            var updated = await _userRepository.UpdateAsync(user);
            return updated != null;
        }

        // Exercise methods
        public async Task<IEnumerable<Exercise>> GetAllExercisesAsync()
        {
            return await _exerciseRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Exercise>> GetExercisesByCategoryAsync(ExerciseCategory category)
        {
            return await _exerciseRepository.GetByCategoryAsync(category);
        }

        public async Task<Exercise> GetExerciseByIdAsync(int exerciseId)
        {
            return await _exerciseRepository.GetByIdAsync(exerciseId);
        }

        // Workout Plan methods
        public async Task<WorkoutPlan> CreateWorkoutPlanAsync(WorkoutPlan plan)
        {
            return await _workoutRepository.CreatePlanAsync(plan);
        }

        public async Task<IEnumerable<WorkoutPlan>> GetUserWorkoutPlansAsync(int userId)
        {
            return await _workoutRepository.GetUserPlansAsync(userId);
        }

        // Workout Session methods
        public async Task<WorkoutSession> CreateWorkoutSessionAsync(WorkoutSession session)
        {
            return await _workoutRepository.CreateSessionAsync(session);
        }

        public async Task<WorkoutSession> UpdateWorkoutSessionAsync(WorkoutSession session)
        {
            return await _workoutRepository.UpdateSessionAsync(session);
        }

        public async Task<IEnumerable<WorkoutSession>> GetUserWorkoutHistoryAsync(int userId)
        {
            return await _workoutRepository.GetUserSessionsAsync(userId);
        }

        // Progress methods
        public async Task<ProgressRecord> CreateProgressRecordAsync(ProgressRecord record)
        {
            return await _progressRepository.CreateAsync(record);
        }

        public async Task<IEnumerable<ProgressRecord>> GetUserProgressRecordsAsync(int userId)
        {
            return await _progressRepository.GetUserRecordsAsync(userId);
        }

        // Statistics
        public async Task<Dictionary<string, object>> GetUserStatisticsAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var sessions = await _workoutRepository.GetUserSessionsInRangeAsync(userId, startDate, endDate);

            return new Dictionary<string, object>
            {
                ["TotalSessions"] = sessions.Count(),
                ["TotalMinutes"] = sessions.Sum(s => s.TotalDurationMinutes ?? 0),
                ["TotalCalories"] = sessions.Sum(s => s.TotalCaloriesBurned ?? 0),
                ["AverageDifficulty"] = sessions.Average(s => s.DifficultyRating ?? 0)
            };
        }

        // Helper methods for password hashing
        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + salt;
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
