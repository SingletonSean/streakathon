using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using Streakathon.MAUI.Entities.Streaks;

namespace Streakathon.MAUI.Pages
{
    public partial class AddStreakViewModel : ObservableObject
    {
        private readonly StreakStore _streakStore;
        private readonly FirebaseAuthClient _authClient;

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private string _description;
        
        [ObservableProperty]
        private bool _isLoading;

        public AddStreakViewModel(StreakStore streakStore, FirebaseAuthClient authClient)
        {
            _streakStore = streakStore;
            _authClient = authClient;
        }

        [RelayCommand]
        private async Task SubmitStreak()
        {
            IsLoading = true;

            try 
            { 
                NewStreak streak = new NewStreak(Title, Description, _authClient.User.Uid);
                await _streakStore.Create(streak);

                await Shell.Current.GoToAsync("//Streaks");

                Title = string.Empty;
                Description = string.Empty;
            } 
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to create streak. Please try again later.", "Ok");
            } 
            finally
            {
                IsLoading = false;
            }
        }
    }
}
