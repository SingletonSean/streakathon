using CommunityToolkit.Mvvm.ComponentModel;

namespace Streakathon.MAUI.Entities.Streaks
{
    public class StreakOverviewViewModel : ObservableObject
    {
        public string Title { get; }
        public int Length { get; }

        public StreakOverviewViewModel(string title, int length)
        {
            Title = title;
            Length = length;
        }
    }
}
