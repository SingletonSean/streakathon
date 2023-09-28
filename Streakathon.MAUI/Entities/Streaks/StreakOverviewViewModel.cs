using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Streakathon.MAUI.Entities.Streaks
{
    public partial class StreakOverviewViewModel : ObservableObject
    {
        private readonly string _id;

        public string Title { get; }
        public int Length { get; }

        public StreakOverviewViewModel(string id, string title, int length)
        {
            _id = id;
            Title = title;
            Length = length;
        }

        [RelayCommand]
        private async Task NavigateStreakDetails()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "id", _id  }
            };
               
            await Shell.Current.GoToAsync("Details", parameters);
        }
    }
}
