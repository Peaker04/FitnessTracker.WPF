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
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly FitnessDbContext _context;

        public ExerciseRepository(FitnessDbContext context)
        {
            _context = context;
        }

        public async Task<Exercise> GetByIdAsync(int exerciseId)
        {
            return await _context.Exercises.FindAsync(exerciseId);
        }

        public async Task<IEnumerable<Exercise>> GetAllAsync()
        {
            return await _context.Exercises.ToListAsync();
        }

        public async Task<IEnumerable<Exercise>> GetByCategoryAsync(ExerciseCategory category)
        {
            return await _context.Exercises
                .Where(e => e.Category == category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Exercise>> GetByMuscleGroupAsync(string muscleGroup)
        {
            return await _context.Exercises
                .Where(e => e.MuscleGroup.Contains(muscleGroup))
                .ToListAsync();
        }

        public async Task<Exercise> AddAsync(Exercise exercise)
        {
            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();
            return exercise;
        }

        public async Task<Exercise> UpdateAsync(Exercise exercise)
        {
            exercise.UpdatedAt = DateTime.Now;
            _context.Exercises.Update(exercise);
            await _context.SaveChangesAsync();
            return exercise;
        }

        public async Task<bool> DeleteAsync(int exerciseId)
        {
            var exercise = await GetByIdAsync(exerciseId);
            if (exercise == null) return false;

            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
