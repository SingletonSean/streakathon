using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using Microsoft.Extensions.Logging;

namespace Streakathon.MAUI.Pages
{
    public partial class SignInViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;
        private readonly ILogger<SignInViewModel> _logger;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private bool _isLoading;

        public SignInViewModel(FirebaseAuthClient authClient, ILogger<SignInViewModel> logger)
        {
            _authClient = authClient;
            _logger = logger;
        }

        [RelayCommand]
        private async Task SignIn()
        {
            _logger.LogInformation("Signing in");

            IsLoading = true;

            try
            {
                await _authClient.SignInWithEmailAndPasswordAsync(Email, Password);
                
                _logger.LogInformation("Successfully signed in");

                await Shell.Current.DisplayAlert("Success", "Successfully signed in!", "Ok");

                await Shell.Current.GoToAsync("//Streaks");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to sign in");

                await Shell.Current.DisplayAlert("Error", "Failed to sign in. Please try again later.", "Ok");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task ViewSignUp()
        {
            await Shell.Current.GoToAsync("//SignUp");
        }
    }
}
