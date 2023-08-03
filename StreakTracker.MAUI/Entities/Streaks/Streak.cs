namespace StreakTracker.MAUI.Entities.Streaks
{
    public class Streak
    {
        private readonly List<StreakEntry> _entries;

        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<StreakEntry> Entries => _entries;

        public Streak()
        {
            _entries = new List<StreakEntry>();
        }

        public void AddEntry(StreakEntry entry)
        {
            _entries.Add(entry);
        }
    }
}
