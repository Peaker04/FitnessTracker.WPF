using FitnessTracker.WPF.Helpers;
using FitnessTracker.WPF.Models;
using FitnessTracker.WPF.Services.Interfaces;
using FitnessTracker.WPF.ViewModels.Base;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FitnessTracker.WPF.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly IDatabaseService _databaseService;
        private string _username;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private string _errorMessage;

        public RegisterViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            RegisterCommand = new RelayCommand(async _ => await RegisterAsync(), _ => CanRegister());
            GoBackCommand = new RelayCommand(_ => RequestGoBack?.Invoke());
        }

        public string Username { get => _username; set => SetProperty(ref _username, value); }
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        public string ConfirmPassword { get => _confirmPassword; set => SetProperty(ref _confirmPassword, value); }
        public string ErrorMessage { get => _errorMessage; set => SetProperty(ref _errorMessage, value); }

        public ICommand RegisterCommand { get; }
        public ICommand GoBackCommand { get; }

        // Sự kiện để báo cho App.xaml.cs biết cần chuyển màn hình
        public event Action RequestGoBack;
        public event Action<User> RegistrationSuccessful;

        private bool CanRegister()
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Email) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !string.IsNullOrWhiteSpace(ConfirmPassword);
        }

        private async Task RegisterAsync()
        {
            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Mật khẩu xác nhận không khớp!";
                return;
            }

            try
            {
                ErrorMessage = string.Empty;
                var newUser = new User
                {
                    Username = Username,
                    Email = Email,
                    FullName = Username, // Mặc định lấy username làm tên
                    IsAIEnabled = true,
                    IsActive = true
                    // Các trường khác sẽ có giá trị mặc định hoặc null
                };

                var createdUser = await _databaseService.RegisterUserAsync(newUser, Password);

                // Đăng ký thành công -> chuyển vào Main
                RegistrationSuccessful?.Invoke(createdUser);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Lỗi đăng ký: {ex.Message}";
            }
        }
    }
}