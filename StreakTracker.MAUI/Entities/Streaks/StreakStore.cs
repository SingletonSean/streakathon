using CommunityToolkit.Mvvm.Messaging;

namespace StreakTracker.MAUI.Entities.Streaks
{
    public class StreakStore
    {
        private readonly List<Streak> _streaks;

        public IEnumerable<Streak> Streaks => _streaks;

        public StreakStore()
        {
            _streaks = new List<Streak>();

            Streak stretchingStreak = new Streak()
            {
                Title = "Stretching",
                Description = "Do 15 minutes of stretching everyday"
            };
            stretchingStreak.AddEntry(new StreakEntry(DateTime.Now));

            Streak readingStreak = new Streak()
            {
                Title = "Reading",
                Description = "Read for 30 minutes everyday"
            };
            readingStreak.AddEntry(new StreakEntry(DateTime.Now));
            readingStreak.AddEntry(new StreakEntry(DateTime.Now.AddDays(-1)));

            Streak codingStreak = new Streak()
            {
                Title = "Coding",
                Description = "Do 30 minutes of LeetCode everyday"
            };
            codingStreak.AddEntry(new StreakEntry(DateTime.Now));
            codingStreak.AddEntry(new StreakEntry(DateTime.Now.AddDays(-3)));
            codingStreak.AddEntry(new StreakEntry(DateTime.Now.AddDays(-4)));

            Add(stretchingStreak);
            Add(readingStreak);
            Add(codingStreak);
        }

        public void Add(Streak streak)
        {
            _streaks.Add(streak);

            StrongReferenceMessenger.Default.Send(new StreakAddedMessage(streak));
        }
    }
}
