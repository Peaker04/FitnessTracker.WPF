using FitnessTracker.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.WPF.Repositories.Interfaces
{
    public interface IProgressRepository
    {
        Task<ProgressRecord> CreateAsync(ProgressRecord record);
        Task<IEnumerable<ProgressRecord>> GetUserRecordsAsync(int userId);
        Task<ProgressRecord> GetLatestRecordAsync(int userId);
        Task<bool> DeleteAsync(int recordId);
    }
}
