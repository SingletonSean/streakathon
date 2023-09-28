using CommunityToolkit.Mvvm.ComponentModel;
using Streakathon.MAUI.Entities.Streaks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streakathon.MAUI.Pages
{
    [QueryProperty(nameof(Id), "id")]
    public partial class StreakDetailsViewModel : ObservableObject
    {
        private readonly StreakStore _streakStore;

        private string _id;
        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                CurrentStreak = _streakStore.Streaks.FirstOrDefault(s => s.Id == _id);
            }
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Title))]
        [NotifyPropertyChangedFor(nameof(Description))]
        [NotifyPropertyChangedFor(nameof(Length))]
        private Streak _currentStreak;

        public string Title => CurrentStreak?.Title;
        public string Description => CurrentStreak?.Description;
        public int Length => CurrentStreak?.Length ?? 0;

        public StreakDetailsViewModel(StreakStore streakStore)
        {
            _streakStore = streakStore;
        }
    }
}
