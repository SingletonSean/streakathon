using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Refit;
using Streakathon.MAUI.Entities.Streaks;
using Streakathon.MAUI.Entities.Streaks.Data;
using System.Collections.ObjectModel;

namespace Streakathon.MAUI.Pages
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly StreakStore _streakStore;
        private readonly IGetAllStreaksQuery _getAllStreaksQuery;
        private readonly IGetStreakEntriesQuery _getStreakEntriesQuery;
        private readonly ObservableCollection<StreakOverviewViewModel> _streakOverviewViewModels;

        public IEnumerable<StreakOverviewViewModel> StreakOverviewViewModels => _streakOverviewViewModels;

        public HomeViewModel(
            StreakStore streakStore, 
            IGetAllStreaksQuery getAllStreaksQuery,
            IGetStreakEntriesQuery getStreakEntriesQuery)
        {
            _streakStore = streakStore;
            _getAllStreaksQuery = getAllStreaksQuery;
            _getStreakEntriesQuery = getStreakEntriesQuery;

            _streakOverviewViewModels = new ObservableCollection<StreakOverviewViewModel>();

            StrongReferenceMessenger.Default.Register<StreakAddedMessage>(this, OnStreakAdded);

            LoadStreaksCommand.ExecuteAsync(null);
        }

        [RelayCommand]
        private async Task LoadStreaks()
        {
            try
            {
                FirestoreQueryResponse<GetAllStreaksQueryFieldsResponse> streaksResponse = await _getAllStreaksQuery.Execute();

                IEnumerable<Streak> streaks = streaksResponse.Documents.Select(d => new Streak()
                {
                    Id = d.Name.Split("/").LastOrDefault(),
                    Title = d.Fields.Title.StringValue,
                    Description = d.Fields.Description.StringValue,
                }).ToList();
                
                IEnumerable<StreakEntry>[] streakEntryResponses = await Task.WhenAll(streaks.Select(async s =>
                {
                    FirestoreQueryResponse<GetStreakEntriesQueryFieldsResponse> entriesResponse = 
                        await _getStreakEntriesQuery.Execute(s.Id);

                    return entriesResponse
                        .Documents
                        .Select(d => new StreakEntry(d.Fields.Created.TimestampValue));
                }));

                for (int i = 0; i < streaks.Count(); i++)
                {
                    Streak currentStreak = streaks.ElementAt(i);
                    currentStreak.ResetEntries(streakEntryResponses[i]);
                }

                _streakStore.Reset(streaks);

                UpdateStreaks();
            } 
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to load streaks.", "Ok");
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
