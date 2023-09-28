using CommunityToolkit.Mvvm.Messaging;

namespace Streakathon.MAUI.Entities.Streaks
{
    public class StreakStore
    {
        private readonly GetAllStreaksQuery _getAllStreaksQuery;
        private readonly CreateStreakCommand _createStreakCommand;
        private readonly CreateStreakEntryCommand _createStreakEntryCommand;
        private readonly List<Streak> _streaks;

        public IEnumerable<Streak> Streaks => _streaks;

        public StreakStore(
            GetAllStreaksQuery getAllStreaksQuery, 
            CreateStreakCommand createStreakCommand, 
            CreateStreakEntryCommand createStreakEntryCommand)
        {
            _getAllStreaksQuery = getAllStreaksQuery;
            _createStreakCommand = createStreakCommand;
            _createStreakEntryCommand = createStreakEntryCommand;

            _streaks = new List<Streak>();
        }

        public async Task Load()
        {
            IEnumerable<Streak> streaks = await _getAllStreaksQuery.Execute();

            _streaks.Clear();
            _streaks.AddRange(streaks);
        }

        public async Task Create(NewStreak newStreak)
        {
            Streak streak = await _createStreakCommand.Execute(newStreak);

            _streaks.Add(streak);

            StrongReferenceMessenger.Default.Send(new StreakAddedMessage(streak));
        }

        public async Task AddStreakEntry(string streakId)
        {
            StreakEntry streakEntry = await _createStreakEntryCommand.Execute(streakId);

            Streak streak = _streaks.FirstOrDefault(s => s.Id == streakEntry.StreakId);
            streak?.AddEntry(streakEntry);

            StrongReferenceMessenger.Default.Send(new StreakEntryAddedMessage(streak, streakEntry));
        }

        public void Reset(IEnumerable<Streak> streaks)
        {
            _streaks.Clear();

            foreach (Streak streak in streaks)
            {
                _streaks.Add(streak);
            }
        }
    }
}
