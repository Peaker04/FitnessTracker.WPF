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
    public class SettingsViewModel : ViewModelBase
    {
        private readonly IDatabaseService _databaseService;
        private User _currentUser;
        private bool _isAIEnabled;

        public SettingsViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            LoadSettings();
            SaveSettingsCommand = new RelayCommand(async _ => await SaveSettings());
        }

        public User CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        public bool IsAIEnabled
        {
            get => _isAIEnabled;
            set => SetProperty(ref _isAIEnabled, value);
        }

        public ICommand SaveSettingsCommand { get; }

        private async void LoadSettings()
        {
            // Mock user load
            CurrentUser = await _databaseService.GetUserByIdAsync(1);
            if (CurrentUser != null)
            {
                IsAIEnabled = CurrentUser.IsAIEnabled;
            }
        }

        private async Task SaveSettings()
        {
            if (CurrentUser != null)
            {
                CurrentUser.IsAIEnabled = IsAIEnabled;
                await _databaseService.UpdateUserProfileAsync(CurrentUser);
                // Show success message
            }
        }
    }
}
