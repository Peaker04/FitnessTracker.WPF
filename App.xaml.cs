using FitnessTracker.WPF.DataAccess;
using FitnessTracker.WPF.Repositories;
using FitnessTracker.WPF.Repositories.Interfaces;
using FitnessTracker.WPF.Services;
using FitnessTracker.WPF.Services.Interfaces;
using FitnessTracker.WPF.ViewModels;
using FitnessTracker.WPF.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace FitnessTracker.WPF
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;
        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Load configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            // Setup DI
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();

            // Show main window
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Configuration
            services.AddSingleton(Configuration);

            // DbContext
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<FitnessDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IExerciseRepository, ExerciseRepository>();
            services.AddScoped<IWorkoutRepository, WorkoutRepository>();
            services.AddScoped<IProgressRepository, ProgressRepository>();

            // Services
            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddScoped<IAIService, AIService>();

            // ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<WorkoutPlanViewModel>();
            services.AddTransient<WorkoutSessionViewModel>();
            services.AddTransient<StatisticsViewModel>();

            // Views
            services.AddTransient<MainWindow>();
            services.AddTransient<LoginView>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceProvider?.Dispose();
            base.OnExit(e);
        }
    }
}
