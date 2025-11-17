using FitnessTracker.WPF.Helpers;
using FitnessTracker.WPF.Models;
using FitnessTracker.WPF.Services.Interfaces;
using FitnessTracker.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FitnessTracker.WPF.ViewModels
{
    public class WorkoutPlanViewModel : ViewModelBase
    {
        private readonly IAIService _aiService;
        private readonly IDatabaseService _databaseService;

        private FitnessGoal _selectedGoal;
        private PreferredEnvironment _selectedEnvironment;
        private int _daysPerWeek = 3;
        private string _generatedPlanText;
        private bool _isBusy;

        public WorkoutPlanViewModel(IAIService aiService, IDatabaseService databaseService)
        {
            _aiService = aiService;
            _databaseService = databaseService;

            GenerateAIPlanCommand = new RelayCommand(async _ => await GeneratePlanAsync(), _ => !IsBusy);
            SavePlanCommand = new RelayCommand(async _ => await SavePlanAsync(), _ => !string.IsNullOrEmpty(GeneratedPlanText));
        }

        public FitnessGoal SelectedGoal
        {
            get => _selectedGoal;
            set => SetProperty(ref _selectedGoal, value);
        }

        public PreferredEnvironment SelectedEnvironment
        {
            get => _selectedEnvironment;
            set => SetProperty(ref _selectedEnvironment, value);
        }

        public int DaysPerWeek
        {
            get => _daysPerWeek;
            set => SetProperty(ref _daysPerWeek, value);
        }

        public string GeneratedPlanText
        {
            get => _generatedPlanText;
            set => SetProperty(ref _generatedPlanText, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public ICommand GenerateAIPlanCommand { get; }
        public ICommand SavePlanCommand { get; }

        private async Task GeneratePlanAsync()
        {
            IsBusy = true;
            try
            {
                // Gọi AI Service [cite: 159-160]
                GeneratedPlanText = await _aiService.GenerateWorkoutPlanAsync(SelectedGoal, SelectedEnvironment, DaysPerWeek);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SavePlanAsync()
        {
            var newPlan = new WorkoutPlan
            {
                UserId = 1, // Mock user ID
                PlanName = $"{SelectedGoal} Plan",
                Description = GeneratedPlanText,
                Goal = SelectedGoal,
                Environment = SelectedEnvironment,
                IsAIGenerated = true,
                IsActive = true
            };

            await _databaseService.CreateWorkoutPlanAsync(newPlan);
            // Show notification success here
        }
    }
}