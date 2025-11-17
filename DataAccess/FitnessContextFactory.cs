using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.WPF.DataAccess
{
    public class FitnessContextFactory : IDesignTimeDbContextFactory<FitnessDbContext>
    {
        public FitnessDbContext CreateDbContext(string[] args)
        {
            // 1. Đọc cấu hình từ file appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // 2. Lấy Connection String
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // 3. Tạo OptionsBuilder
            var optionsBuilder = new DbContextOptionsBuilder<FitnessDbContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            // 4. Trả về instance của DbContext
            return new FitnessDbContext(optionsBuilder.Options);
        }
    }
}
