using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;

namespace Streakathon.MAUI.Pages
{
    public partial class SignInViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private bool _isLoading;

        public SignInViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
        }

        [RelayCommand]
        private async Task SignIn()
        {
            IsLoading = true;

            try
            {
                await _authClient.SignInWithEmailAndPasswordAsync(Email, Password);

                await Shell.Current.DisplayAlert("Success", "Successfully signed in!", "Ok");

                await Shell.Current.GoToAsync("//Streaks");
            }
            catch (Exception)
            {
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
