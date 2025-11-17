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
    public class ProgressRepository : IProgressRepository
    {
        private readonly FitnessDbContext _context;

        public ProgressRepository(FitnessDbContext context)
        {
            _context = context;
        }

        public async Task<ProgressRecord> CreateAsync(ProgressRecord record)
        {
            _context.ProgressRecords.Add(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<IEnumerable<ProgressRecord>> GetUserRecordsAsync(int userId)
        {
            return await _context.ProgressRecords
                .Where(pr => pr.UserId == userId)
                .OrderByDescending(pr => pr.RecordDate)
                .ToListAsync();
        }

        public async Task<ProgressRecord> GetLatestRecordAsync(int userId)
        {
            return await _context.ProgressRecords
                .Where(pr => pr.UserId == userId)
                .OrderByDescending(pr => pr.RecordDate)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteAsync(int recordId)
        {
            var record = await _context.ProgressRecords.FindAsync(recordId);
            if (record == null) return false;

            _context.ProgressRecords.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}