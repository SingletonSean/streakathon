using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Streakathon.MAUI.Entities.Streaks;

namespace Streakathon.MAUI.Pages
{
    public partial class AddStreakViewModel : ObservableObject
    {
        private readonly StreakStore _streakStore;

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private string _description;

        public AddStreakViewModel(StreakStore streakStore)
        {
            _streakStore = streakStore;
        }

        [RelayCommand]
        private async Task SubmitStreak()
        {
            Streak streak = new Streak()
            {
                Title = Title,
                Description = Description
            };
            _streakStore.Add(streak);

            await Shell.Current.GoToAsync("//Home");

            Title = string.Empty;
            Description = string.Empty;
        }
    }
}
