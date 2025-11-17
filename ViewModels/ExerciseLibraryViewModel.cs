using FitnessTracker.WPF.Models;
using FitnessTracker.WPF.Services.Interfaces;
using FitnessTracker.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FitnessTracker.WPF.ViewModels
{
    public class ExerciseLibraryViewModel : ViewModelBase
    {
        private readonly IDatabaseService _databaseService;
        private string _searchText;
        private ExerciseCategory? _selectedCategory;

        public ObservableCollection<Exercise> Exercises { get; } = new();

        public ExerciseLibraryViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            LoadExercises();
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                    FilterExercises();
            }
        }

        public ExerciseCategory? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (SetProperty(ref _selectedCategory, value))
                    FilterExercises();
            }
        }

        private async void LoadExercises()
        {
            var list = await _databaseService.GetAllExercisesAsync();
            Exercises.Clear();
            foreach (var item in list)
            {
                Exercises.Add(item);
            }
        }

        private void FilterExercises()
        {
            // Logic lọc client-side đơn giản
            // Trong thực tế nên dùng ICollectionView để filter UI
            CollectionViewSource.GetDefaultView(Exercises).Filter = o =>
            {
                if (o is Exercise ex)
                {
                    bool matchesSearch = string.IsNullOrEmpty(SearchText) ||
                                         ex.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
                    bool matchesCategory = SelectedCategory == null ||
                                           ex.Category == SelectedCategory;

                    return matchesSearch && matchesCategory;
                }
                return false;
            };
        }
    }
}
