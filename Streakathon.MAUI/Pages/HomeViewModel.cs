using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Streakathon.MAUI.Entities.Streaks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Streakathon.MAUI.Pages
{
    public partial class HomeViewModel : ObservableValidator
    {
        private readonly ILogger<HomeViewModel> _logger;

        private readonly StreakStore _streakStore;
        private readonly ObservableCollection<StreakOverviewViewModel> _streakOverviewViewModels;

        public IEnumerable<StreakOverviewViewModel> StreakOverviewViewModels => _streakOverviewViewModels;

        [ObservableProperty]
        private bool _isLoading;

        public bool HasStreaks => StreakOverviewViewModels.Count() > 0;

        public HomeViewModel(StreakStore streakStore, ILogger<HomeViewModel> logger)
        {
            _streakStore = streakStore;
            _logger = logger;

            _streakOverviewViewModels = new ObservableCollection<StreakOverviewViewModel>();

            StrongReferenceMessenger.Default.Register<StreakAddedMessage>(this, OnStreakAdded);
            StrongReferenceMessenger.Default.Register<StreakEntryAddedMessage>(this, OnStreakEntryAdded);

            LoadStreaksCommand.ExecuteAsync(null);

            _streakOverviewViewModels.CollectionChanged += StreakOverviewViewModels_CollectionChanged;
        }

        [RelayCommand]
        private async Task LoadStreaks()
        {
            IsLoading = true;

            try
            {
                await _streakStore.Load();

                _logger.LogInformation("Loaded streaks: {Count}", _streakStore.Streaks.Count());

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
            await Shell.Current.GoToAsync("New");
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

        private void OnStreakEntryAdded(object recipient, StreakEntryAddedMessage message)
        {
            StreakOverviewViewModel viewModel = _streakOverviewViewModels.FirstOrDefault(s => s.Id == message.Streak.Id);

            if (viewModel == null)
            {
                return;
            }

            int viewModelIndex = _streakOverviewViewModels.IndexOf(viewModel);
            _streakOverviewViewModels[viewModelIndex] = ToStreakOverviewViewModel(message.Streak);
        }

        private static StreakOverviewViewModel ToStreakOverviewViewModel(Streak streak)
        {
            return new StreakOverviewViewModel(streak);
        }

        private void StreakOverviewViewModels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasStreaks));
        }
    }
}
