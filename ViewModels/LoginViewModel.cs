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
    public class LoginViewModel : ViewModelBase
    {
        private readonly IDatabaseService _databaseService;
        private string _username;
        private string _password;
        private string _errorMessage;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public event Action<User> LoginSuccessful;

        public LoginViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            LoginCommand = new RelayCommand(async _ => await LoginAsync(), _ => CanLogin());
            RegisterCommand = new RelayCommand(_ => NavigateToRegister());
        }

        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        private async Task LoginAsync()
        {
            try
            {
                ErrorMessage = string.Empty;
                var user = await _databaseService.AuthenticateUserAsync(Username, Password);

                if (user != null)
                {
                    LoginSuccessful?.Invoke(user);
                }
                else
                {
                    ErrorMessage = "Invalid username or password";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Login error: {ex.Message}";
            }
        }

        private void NavigateToRegister()
        {
            // Navigate to register view
        }
    }
}
