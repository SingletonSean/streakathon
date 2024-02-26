using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using Streakathon.MAUI.Entities.Streaks;
using Streakathon.MAUI.Shared.Shells;

namespace Streakathon.MAUI.Pages
{
    public partial class AddStreakViewModel : ObservableObject
    {
        private readonly StreakStore _streakStore;
        private readonly IFirebaseAuthClient _authClient;
        private readonly IShell _shell;

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private string _description;
        
        [ObservableProperty]
        private bool _isLoading;

        public AddStreakViewModel(
            StreakStore streakStore,
            IFirebaseAuthClient authClient,
            IShell shell)
        {
            _streakStore = streakStore;
            _authClient = authClient;
            _shell = shell;
        }

        [RelayCommand]
        private async Task SubmitStreak()
        {
            IsLoading = true;

            try 
            { 
                NewStreak streak = new NewStreak(Title, Description, _authClient?.User?.Uid ?? "");
                await _streakStore.Create(streak);

                await _shell.GoToAsync("//Streaks");

                Title = string.Empty;
                Description = string.Empty;
            } 
            catch (Exception)
            {
                await _shell.DisplayAlert("Error", "Failed to create streak. Please try again later.", "Ok");
            } 
            finally
            {
                IsLoading = false;
            }
        }
    }
}
