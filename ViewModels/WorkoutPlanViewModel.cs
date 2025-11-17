using FitnessTracker.WPF.Helpers;
using FitnessTracker.WPF.Models;
using FitnessTracker.WPF.Services.Interfaces;
using FitnessTracker.WPF.ViewModels.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FitnessTracker.WPF.ViewModels
{
    // 1. Định nghĩa DTO để hứng dữ liệu JSON từ AI
    public class AIExerciseDTO
    {
        public int Day { get; set; }
        public string ExerciseName { get; set; }
        public int Sets { get; set; }
        public string Reps { get; set; } // AI trả về string (ví dụ: "10-12")
    }

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
                GeneratedPlanText = await _aiService.GenerateWorkoutPlanAsync(SelectedGoal, SelectedEnvironment, DaysPerWeek);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SavePlanAsync()
        {
            try
            {
                // Tạo Plan cha
                var newPlan = new WorkoutPlan
                {
                    UserId = 1, // Mock ID
                    PlanName = $"{SelectedGoal} Plan ({DateTime.Now:dd/MM})",
                    Description = "AI Generated Plan",
                    Goal = SelectedGoal,
                    Environment = SelectedEnvironment,
                    IsAIGenerated = true,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    Exercises = new List<WorkoutPlanExercise>()
                };

                // Deserialize JSON
                var aiExercises = JsonConvert.DeserializeObject<List<AIExerciseDTO>>(GeneratedPlanText);

                if (aiExercises != null)
                {
                    foreach (var item in aiExercises)
                    {
                        // Xử lý parse Reps từ string "10-12" sang int (lấy số đầu)
                        int parsedReps = 10;
                        if (!string.IsNullOrEmpty(item.Reps))
                        {
                            var parts = item.Reps.Split('-');
                            int.TryParse(parts[0], out parsedReps);
                        }

                        newPlan.Exercises.Add(new WorkoutPlanExercise
                        {
                            ExerciseId = 1, // Tạm thời fix cứng ID, logic thực tế cần tìm ID theo tên
                            DayNumber = item.Day,

                            // 2. FIX: Map đúng tên property trong Model WorkoutPlanExercise
                            PlannedSets = item.Sets,
                            PlannedReps = parsedReps,

                            OrderIndex = 1,
                            Notes = item.ExerciseName // Lưu tên bài tập tạm vào Notes
                        });
                    }
                }

                await _databaseService.CreateWorkoutPlanAsync(newPlan);
                MessageBox.Show("Đã lưu kế hoạch thành công!", "Thông báo");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi");
            }
        }
    }
}