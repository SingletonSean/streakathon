﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Streakathon.MAUI.Entities.Streaks
{
    public partial class StreakOverviewViewModel : ObservableObject
    {
        public string Id { get; }
        public string Title { get; }
        public int Length { get; }

        public StreakOverviewViewModel(string id, string title, int length)
        {
            Id = id;
            Title = title;
            Length = length;
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
