using FitnessTracker.WPF.Models;
using FitnessTracker.WPF.Services.Interfaces;
using FitnessTracker.WPF.ViewModels.Base;
using FitnessTracker.WPF.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks; // Đảm bảo có namespace này
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace FitnessTracker.WPF.ViewModels
{
    public class WorkoutSessionViewModel : ViewModelBase
    {
        private readonly IDatabaseService _databaseService;
        private DispatcherTimer _timer;
        private TimeSpan _currentTime;
        private bool _isTimerRunning;
        private string _timerDisplay;

        private WorkoutPlan _selectedPlan;
        private ObservableCollection<WorkoutPlan> _availablePlans;

        public WorkoutSessionViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += Timer_Tick;
            TimerDisplay = "00:00:00";

            AvailablePlans = new ObservableCollection<WorkoutPlan>();
            LoadPlans();

            StartWorkoutCommand = new RelayCommand(_ => StartTimer(), _ => !IsTimerRunning);
            StopWorkoutCommand = new RelayCommand(_ => StopTimer(), _ => IsTimerRunning);
            ResetTimerCommand = new RelayCommand(_ => ResetTimer());

            // Kiểm tra thời gian tập > 0 mới cho lưu
            FinishWorkoutCommand = new RelayCommand(async _ => await FinishSessionAsync(), _ => _currentTime.TotalSeconds > 0);
        }

        public ObservableCollection<WorkoutPlan> AvailablePlans
        {
            get => _availablePlans;
            set => SetProperty(ref _availablePlans, value);
        }

        public WorkoutPlan SelectedPlan
        {
            get => _selectedPlan;
            set => SetProperty(ref _selectedPlan, value);
        }

        public string TimerDisplay
        {
            get => _timerDisplay;
            set => SetProperty(ref _timerDisplay, value);
        }

        public bool IsTimerRunning
        {
            get => _isTimerRunning;
            set => SetProperty(ref _isTimerRunning, value);
        }

        public ICommand StartWorkoutCommand { get; }
        public ICommand StopWorkoutCommand { get; }
        public ICommand ResetTimerCommand { get; }
        public ICommand FinishWorkoutCommand { get; }

        private async void LoadPlans()
        {
            var plans = await _databaseService.GetUserWorkoutPlansAsync(1); // UserID = 1
            AvailablePlans.Clear();
            foreach (var p in plans) AvailablePlans.Add(p);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _currentTime = _currentTime.Add(TimeSpan.FromSeconds(1));
            TimerDisplay = _currentTime.ToString(@"hh\:mm\:ss");
        }

        private void StartTimer()
        {
            _timer.Start();
            IsTimerRunning = true;
        }

        private void StopTimer()
        {
            _timer.Stop();
            IsTimerRunning = false;
        }

        private void ResetTimer()
        {
            StopTimer();
            _currentTime = TimeSpan.Zero;
            TimerDisplay = "00:00:00";
        }

        private async Task FinishSessionAsync()
        {
            StopTimer();

            double totalMinutes = _currentTime.TotalMinutes;
            int calories = (int)(totalMinutes * 8); // Công thức tính calo đơn giản

            // Tên plan hoặc mặc định
            string planName = SelectedPlan?.PlanName ?? "Tập tự do";

            var session = new WorkoutSession
            {
                UserId = 1,
                WorkoutPlanId = SelectedPlan?.WorkoutPlanId,

                // --- FIX LỖI Ở ĐÂY: Gán giá trị cho SessionName ---
                SessionName = $"{planName} - {DateTime.Now:dd/MM/yyyy HH:mm}",
                // --------------------------------------------------

                SessionDate = DateTime.Now,
                StartTime = DateTime.Now.AddMinutes(-totalMinutes),
                EndTime = DateTime.Now,
                TotalDurationMinutes = (int)totalMinutes,
                TotalCaloriesBurned = calories,
                Notes = $"Hoàn thành bài tập: {planName}"
            };

            try
            {
                // Gọi hàm lưu xuống DB
                await _databaseService.CreateWorkoutSessionAsync(session);

                MessageBox.Show($"Đã lưu buổi tập thành công!\nThời gian: {TimerDisplay}\nCalo tiêu thụ: {calories}", "Thành công");
                ResetTimer();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi lưu: {ex.Message}\n{ex.InnerException?.Message}", "Lỗi Database");
            }
        }
    }
}