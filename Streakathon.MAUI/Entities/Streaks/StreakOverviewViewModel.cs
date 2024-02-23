using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Streakathon.MAUI.Entities.Streaks
{
    public partial class StreakOverviewViewModel : ObservableObject
    {
        private readonly Streak _streak;

        public string Id => _streak.Id;
        public string Title => _streak.Title;
        public int Length => _streak.Length;
        public StreakLengthScore LengthScore => _streak.LengthScore;

        public StreakOverviewViewModel(Streak streak)
        {
            _streak = streak;
        }

        [RelayCommand]
        private async Task NavigateStreakDetails()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "id", Id  }
            };
               
            await Shell.Current.GoToAsync("Details", parameters);
        }
    }
}
