using FitnessTracker.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.WPF.Repositories.Interfaces
{
    public interface IExerciseRepository
    {
        Task<Exercise> GetByIdAsync(int exerciseId);
        Task<IEnumerable<Exercise>> GetAllAsync();
        Task<IEnumerable<Exercise>> GetByCategoryAsync(ExerciseCategory category);
        Task<IEnumerable<Exercise>> GetByMuscleGroupAsync(string muscleGroup);
        Task<Exercise> AddAsync(Exercise exercise);
        Task<Exercise> UpdateAsync(Exercise exercise);
        Task<bool> DeleteAsync(int exerciseId);
    }
}
