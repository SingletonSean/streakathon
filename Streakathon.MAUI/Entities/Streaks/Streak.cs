namespace Streakathon.MAUI.Entities.Streaks
{
    public class Streak
    {
        private readonly List<StreakEntry> _entries;

        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<StreakEntry> Entries => _entries;
        public int Length
        {
            get
            {
                if (!Entries.Any())
                {
                    return 0;
                }

                IOrderedEnumerable<StreakEntry> recentOrderedEntries = Entries.OrderByDescending(e => e.Created);

                StreakEntry latestEntry = recentOrderedEntries.First();

                bool isActiveStreak =
                    latestEntry.Created.ToLocalTime().Date == DateTime.Now.Date ||
                    latestEntry.Created.ToLocalTime().Date == DateTime.Now.AddDays(-1).Date;

                if (!isActiveStreak)
                {
                    return 0;
                }

                DateTime? lastEntryDate = null;

                IEnumerable<StreakEntry> activeStreakEntries = recentOrderedEntries.TakeWhile(e =>
                {
                    if (!lastEntryDate.HasValue)
                    {
                        lastEntryDate = e.Created;
                        return true;
                    }

                    DateTime currentEntryTime = e.Created;
                    DateTime expectedEntryTime = lastEntryDate.Value.AddDays(-1);

                    lastEntryDate = currentEntryTime;

                    return currentEntryTime.ToLocalTime().Date == expectedEntryTime.ToLocalTime().Date;
                });

                return activeStreakEntries.Count();
            }
        }

        public StreakLengthScore LengthScore
        {
            get
            {
                if (Length == 0)
                {
                    return StreakLengthScore.BAD;
                }

                if (Length < 10)
                {
                    return StreakLengthScore.MEDIUM;
                }

                return StreakLengthScore.GOOD;
            }
        }

        public Streak() 
        {
            _entries = new List<StreakEntry>();
        }

        public void AddEntry(StreakEntry entry)
        {
            _entries.Add(entry);
        }

        public void ResetEntries(IEnumerable<StreakEntry> entries)
        {
            _entries.Clear();
            _entries.AddRange(entries);
        }
    }
}
