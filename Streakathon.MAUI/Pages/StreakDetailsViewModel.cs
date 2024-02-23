using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
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
        private readonly FirebaseAuthClient _authClient;

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
        [NotifyPropertyChangedFor(nameof(LengthScore))]
        private Streak _currentStreak;

        public string Title => CurrentStreak?.Title;
        public string Description => CurrentStreak?.Description;
        public int Length => CurrentStreak?.Length ?? 0;
        public StreakLengthScore LengthScore => CurrentStreak?.LengthScore ?? StreakLengthScore.BAD;

        [ObservableProperty]
        private bool _isLoading;

        public StreakDetailsViewModel(StreakStore streakStore, FirebaseAuthClient authClient)
        {
            _streakStore = streakStore;
            _authClient = authClient;
        }

        [RelayCommand]
        private async Task StreakCheckIn()
        {
            IsLoading = true;

            try
            {
                await _streakStore.AddStreakEntry(_authClient.User.Uid, Id);

                OnPropertyChanged(nameof(Length));
                OnPropertyChanged(nameof(LengthScore));
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to perform streak check-in. Please try again later.", "Ok");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
