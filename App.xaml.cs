// Đảm bảo bạn đã có các using này ở đầu file
using FitnessTracker.WPF.DataAccess;
using FitnessTracker.WPF.Models;
using FitnessTracker.WPF.Repositories;
using FitnessTracker.WPF.Repositories.Interfaces;
using FitnessTracker.WPF.Services;
using FitnessTracker.WPF.Services.Interfaces;
using FitnessTracker.WPF.ViewModels;
using FitnessTracker.WPF.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            // 1. Load configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            // 2. Setup DI
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();

            // 3. Xử lý logic hiển thị màn hình Login trước
            ShowLoginWindow();
        }

        private void ShowLoginWindow()
        {
            // 1. Khởi tạo Window
            var loginWindow = new Window
            {
                Title = "Fitness Tracker - Authentication",
                Height = 600,
                Width = 450,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.CanMinimize,
                SizeToContent = SizeToContent.Height
            };

            // 2. Hàm chuyển đổi View
            void SwitchToLogin()
            {
                var loginVm = _serviceProvider.GetRequiredService<LoginViewModel>();
                var loginView = _serviceProvider.GetRequiredService<LoginView>();
                loginView.DataContext = loginVm;

                // Xử lý sự kiện của LoginViewModel
                loginVm.LoginSuccessful += (user) =>
                {
                    OpenMainWindow(user);
                    loginWindow.Close();
                };

                // Khi ấn "Register here" -> Chuyển sang View Đăng ký
                loginVm.RequestToRegister += () => SwitchToRegister();

                loginWindow.Content = loginView;
            }

            void SwitchToRegister()
            {
                // Bạn nhớ đăng ký RegisterViewModel và RegisterView trong ConfigureServices nhé!
                // services.AddTransient<RegisterViewModel>();
                // services.AddTransient<RegisterView>();

                var registerVm = new RegisterViewModel(_serviceProvider.GetRequiredService<IDatabaseService>());
                var registerView = new RegisterView(); // Hoặc lấy từ DI
                registerView.DataContext = registerVm;

                // Khi ấn "Quay lại" -> Chuyển về Login
                registerVm.RequestGoBack += () => SwitchToLogin();

                // Khi đăng ký thành công -> Vào thẳng Dashboard
                registerVm.RegistrationSuccessful += (user) =>
                {
                    OpenMainWindow(user);
                    loginWindow.Close();
                };

                loginWindow.Content = registerView;
            }

            // 3. Bắt đầu bằng màn hình Login
            SwitchToLogin();
            loginWindow.Show();
        }

        private void OpenMainWindow(User user)
        {
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.SetCurrentUser(user);
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // ... (Phần Configuration giữ nguyên) ...

            // 1. SỬA LỖI: Đổi AddDbContext thành Transient (Mặc định là Scoped)
            // Thêm tham số ServiceLifetime.Transient
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<FitnessDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)),
                ServiceLifetime.Transient);

            // 2. SỬA LỖI: Đổi tất cả AddScoped -> AddTransient
            // Để đảm bảo mỗi lần gọi là một instance mới, không dùng chung Context cũ
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IExerciseRepository, ExerciseRepository>();
            services.AddTransient<IWorkoutRepository, WorkoutRepository>();
            services.AddTransient<IProgressRepository, ProgressRepository>();

            services.AddTransient<IDatabaseService, DatabaseService>();
            services.AddTransient<IAIService, AIService>();

            // ... (Phần ViewModels và Views giữ nguyên là AddTransient như cũ) ...
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<MainViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<WorkoutPlanViewModel>();
            services.AddTransient<WorkoutSessionViewModel>();
            services.AddTransient<StatisticsViewModel>();
            services.AddTransient<ExerciseLibraryViewModel>();
            services.AddTransient<SettingsViewModel>();

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