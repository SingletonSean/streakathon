using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using StreakTracker.MAUI.Entities.Streaks;
using System.Collections.ObjectModel;

namespace StreakTracker.MAUI.Pages
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly StreakStore _streakStore;
        private readonly ObservableCollection<StreakOverviewViewModel> _streakOverviewViewModels;

        public IEnumerable<StreakOverviewViewModel> StreakOverviewViewModels => _streakOverviewViewModels;

        public HomeViewModel(StreakStore streakStore)
        {
            _streakStore = streakStore;
            _streakOverviewViewModels = new ObservableCollection<StreakOverviewViewModel>();

            UpdateStreaks();

            StrongReferenceMessenger.Default.Register<StreakAddedMessage>(this, OnStreakAdded);
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
                StreakOverviewViewModel streakViewModel = ToStreakOverViewModel(streak);
                _streakOverviewViewModels.Add(streakViewModel);
            }
        }

        public void OnStreakAdded(object receiver, StreakAddedMessage streakMessage)
        {
            Streak streak = streakMessage.Streak;

            StreakOverviewViewModel streakViewModel = ToStreakOverViewModel(streak);
            _streakOverviewViewModels.Add(streakViewModel);
        }

        private static StreakOverviewViewModel ToStreakOverViewModel(Streak streak)
        {
            return new StreakOverviewViewModel(streak.Title, 3);
        }
    }
}
