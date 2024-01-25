using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;

namespace Streakathon.MAUI.Pages
{
    public partial class SignUpViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _confirmPassword;

        [ObservableProperty]
        private bool _isLoading;

        public SignUpViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
        }

        [RelayCommand]
        private async Task SignUp()
        {
            if (Password != ConfirmPassword)
            {
                await Shell.Current.DisplayAlert("Error", "Password and confirm password must match.", "Ok");

                return;
            }

            IsLoading = true;

            try
            {
                await _authClient.CreateUserWithEmailAndPasswordAsync(Email, Password);

                await Shell.Current.DisplayAlert("Success", "Successfully signed up!", "Ok");

                await Shell.Current.GoToAsync("//SignIn");
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to sign up. Please try again later.", "Ok");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task ViewSignIn()
        {
            await Shell.Current.GoToAsync("//SignIn");
        }
    }
}
