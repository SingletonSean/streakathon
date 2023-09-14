using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Refit;
using Streakathon.MAUI.Entities.Streaks;
using Streakathon.MAUI.Entities.Streaks.Data;
using Streakathon.MAUI.Shared.Firestore;
using System.Collections.ObjectModel;

namespace Streakathon.MAUI.Pages
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly StreakStore _streakStore;
        private readonly ObservableCollection<StreakOverviewViewModel> _streakOverviewViewModels;

        public IEnumerable<StreakOverviewViewModel> StreakOverviewViewModels => _streakOverviewViewModels;

        [ObservableProperty]
        private bool _isLoading;

        public HomeViewModel(StreakStore streakStore)
        {
            _streakStore = streakStore;
            _streakOverviewViewModels = new ObservableCollection<StreakOverviewViewModel>();

            StrongReferenceMessenger.Default.Register<StreakAddedMessage>(this, OnStreakAdded);

            LoadStreaksCommand.ExecuteAsync(null);
        }

        [RelayCommand]
        private async Task LoadStreaks()
        {
            IsLoading = true;

            try
            {
                await _streakStore.Load();

                UpdateStreaks();
            } 
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to load streaks. Please try again later.", "Ok");
            } 
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task AddStreak()
        {
            await Shell.Current.GoToAsync("//AddStreak");
        }

        private void UpdateStreaks()
        {
            _streakOverviewViewModels.Clear();

            foreach (Streak streak in _streakStore.Streaks)
            {
                StreakOverviewViewModel streakViewModel = ToStreakOverviewViewModel(streak);
                _streakOverviewViewModels.Add(streakViewModel);
            }
        }

        public void OnStreakAdded(object receiver, StreakAddedMessage streakMessage)
        {
            Streak streak = streakMessage.Streak;

            StreakOverviewViewModel streakViewModel = ToStreakOverviewViewModel(streak);
            _streakOverviewViewModels.Add(streakViewModel);
        }

        private static StreakOverviewViewModel ToStreakOverviewViewModel(Streak streak)
        {
            return new StreakOverviewViewModel(streak.Title, streak.Length);
        }
    }
}
