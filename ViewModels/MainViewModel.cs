using FitnessTracker.WPF.Helpers;
using FitnessTracker.WPF.Models;
using FitnessTracker.WPF.Services.Interfaces;
using FitnessTracker.WPF.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FitnessTracker.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDatabaseService _databaseService;
        private ViewModelBase _currentView;
        private User _currentUser;
        private string _headerTitle;

        public MainViewModel(IServiceProvider serviceProvider, IDatabaseService databaseService)
        {
            _serviceProvider = serviceProvider;
            _databaseService = databaseService;

            // Commands điều hướng
            NavigateCommand = new RelayCommand(Navigate);
            LogoutCommand = new RelayCommand(_ => Logout());

            // Mặc định vào Dashboard
            Navigate("Dashboard");
        }

        public User CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        public ViewModelBase CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public string HeaderTitle
        {
            get => _headerTitle;
            set => SetProperty(ref _headerTitle, value);
        }

        public ICommand NavigateCommand { get; }
        public ICommand LogoutCommand { get; }

        private void Navigate(object destination)
        {
            string dest = destination?.ToString();
            switch (dest)
            {
                case "Dashboard":
                    CurrentView = _serviceProvider.GetRequiredService<DashboardViewModel>();
                    HeaderTitle = "Dashboard";
                    break;
                case "WorkoutPlan":
                    CurrentView = _serviceProvider.GetRequiredService<WorkoutPlanViewModel>();
                    HeaderTitle = "Kế Hoạch Tập Luyện";
                    break;
                case "WorkoutSession":
                    CurrentView = _serviceProvider.GetRequiredService<WorkoutSessionViewModel>();
                    HeaderTitle = "Bắt Đầu Tập";
                    break;
                case "ExerciseLibrary":
                    CurrentView = _serviceProvider.GetRequiredService<ExerciseLibraryViewModel>();
                    HeaderTitle = "Thư Viện Bài Tập";
                    break;
                case "Statistics":
                    CurrentView = _serviceProvider.GetRequiredService<StatisticsViewModel>();
                    HeaderTitle = "Thống Kê & Tiến Độ";
                    break;
                case "Settings":
                    CurrentView = _serviceProvider.GetRequiredService<SettingsViewModel>();
                    HeaderTitle = "Cài Đặt";
                    break;
            }
        }

        private void Logout()
        {
            // Logic đăng xuất: Xóa User context và quay về màn hình Login
            // (Trong thực tế có thể cần restart app hoặc switch window)
            System.Windows.Application.Current.Shutdown();
        }

        // Hàm này được gọi từ LoginViewModel sau khi login thành công
        public void SetCurrentUser(User user)
        {
            CurrentUser = user;
            // Refresh lại view hiện tại với data của user mới
            Navigate("Dashboard");
        }
    }
}
