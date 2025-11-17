using FitnessTracker.WPF.Models;
using FitnessTracker.WPF.Services.Interfaces;
using FitnessTracker.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.WPF.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly IDatabaseService _databaseService;
        private int _totalWorkouts;
        private int _caloriesBurned;
        private int _activeMinutes;

        public DashboardViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            LoadDashboardData(); // Gọi async void cho constructor (lưu ý handle error)
        }

        public int TotalWorkouts
        {
            get => _totalWorkouts;
            set => SetProperty(ref _totalWorkouts, value);
        }

        public int CaloriesBurned
        {
            get => _caloriesBurned;
            set => SetProperty(ref _caloriesBurned, value);
        }

        public int ActiveMinutes
        {
            get => _activeMinutes;
            set => SetProperty(ref _activeMinutes, value);
        }

        public ObservableCollection<WorkoutSession> RecentActivities { get; } = new();

        private async void LoadDashboardData()
        {
            // Giả lập lấy User ID hiện tại (thực tế lấy từ MainViewModel hoặc SessionService)
            int currentUserId = 1;

            // Lấy lịch sử tập luyện
            var history = await _databaseService.GetUserWorkoutHistoryAsync(currentUserId);

            // Tính toán thống kê cơ bản
            TotalWorkouts = history.Count();
            CaloriesBurned = history.Sum(h => h.TotalCaloriesBurned ?? 0);
            ActiveMinutes = history.Sum(h => h.TotalDurationMinutes ?? 0);

            // Load danh sách gần đây
            RecentActivities.Clear();
            foreach (var item in history.Take(5))
            {
                RecentActivities.Add(item);
            }
        }
    }
}
