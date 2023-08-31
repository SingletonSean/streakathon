using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Refit;
using Streakathon.MAUI.Entities.Streaks;
using Streakathon.MAUI.Entities.Streaks.Data;
using Streakathon.MAUI.Shared.Firestore;
using System.Collections.ObjectModel;

namespace Streakathon.MAUI.Pages
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly StreakStore _streakStore;
        private readonly IGetAllStreaksQuery _getAllStreaksQuery;
        private readonly IGetAllStreakEntriesQuery _getAllStreakEntriesQuery;
        private readonly ObservableCollection<StreakOverviewViewModel> _streakOverviewViewModels;

        public IEnumerable<StreakOverviewViewModel> StreakOverviewViewModels => _streakOverviewViewModels;

        [ObservableProperty]
        private bool _isLoading;

        public HomeViewModel(
            StreakStore streakStore, 
            IGetAllStreaksQuery getAllStreaksQuery,
            IGetAllStreakEntriesQuery getAllStreakEntriesQuery)
        {
            _streakStore = streakStore;
            _getAllStreaksQuery = getAllStreaksQuery;
            _getAllStreakEntriesQuery = getAllStreakEntriesQuery;

            _streakOverviewViewModels = new ObservableCollection<StreakOverviewViewModel>();

            StrongReferenceMessenger.Default.Register<StreakAddedMessage>(this, OnStreakAdded);

            LoadStreaksCommand.ExecuteAsync(null);
        }

        [RelayCommand]
        private async Task LoadStreaks()
        {
            IsLoading = true;

            try
            {
                GetAllStreaksQueryResponse streaksResponse = await _getAllStreaksQuery.Execute();

                IEnumerable<Streak> streaks = streaksResponse.Documents.Select(d => new Streak()
                {
                    Id = d.Name.Split("/").LastOrDefault(),
                    Title = d.Fields.Title.StringValue,
                    Description = d.Fields.Description.StringValue,
                }).ToList();

                IEnumerable<StreakEntryCollectionGroupItem> streakEntriesResponse = await _getAllStreakEntriesQuery.Execute(
                    new FirestoreRunQueryRequest()
                    {
                        StructuredQuery = new FirestoreStructuredQuery()
                        {
                            From = new List<FirestoreFromItem>()
                            {
                                new FirestoreFromItem()
                                {
                                    CollectionId = "entries",
                                    AllDescendants = true
                                }
                            }
                        }
                    });

                ILookup<string, StreakEntry> streakEntriesLookup = streakEntriesResponse
                    .Select(s => new StreakEntry(s.StreakEntryId, s.StreakId, s.Document.Fields.Created.TimestampValue))
                    .ToLookup(s => s.StreakId);

                foreach (Streak streak in streaks)
                {
                    IEnumerable<StreakEntry> entries = streakEntriesLookup[streak.Id];

                    streak.ResetEntries(entries);
                }

                _streakStore.Reset(streaks);

                UpdateStreaks();
            } 
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to load streaks.", "Ok");
            } 
            finally
            {
                IsLoading = false;
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
