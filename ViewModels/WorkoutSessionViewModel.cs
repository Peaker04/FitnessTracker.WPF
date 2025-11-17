using FitnessTracker.WPF.Helpers;
using FitnessTracker.WPF.Services.Interfaces;
using FitnessTracker.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public WorkoutSessionViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;

            // Setup Timer
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            TimerDisplay = "00:00:00";

            StartWorkoutCommand = new RelayCommand(_ => StartTimer());
            StopWorkoutCommand = new RelayCommand(_ => StopTimer());
            ResetTimerCommand = new RelayCommand(_ => ResetTimer());
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

        private void Timer_Tick(object sender, EventArgs e)
        {
            _currentTime = _currentTime.Add(TimeSpan.FromSeconds(1));
            TimerDisplay = _currentTime.ToString(@"hh\:mm\:ss");
        }

        private void StartTimer()
        {
            if (!IsTimerRunning)
            {
                _timer.Start();
                IsTimerRunning = true;
            }
        }

        private void StopTimer()
        {
            if (IsTimerRunning)
            {
                _timer.Stop();
                IsTimerRunning = false;
            }
        }

        private void ResetTimer()
        {
            StopTimer();
            _currentTime = TimeSpan.Zero;
            TimerDisplay = "00:00:00";
        }
    }
}
