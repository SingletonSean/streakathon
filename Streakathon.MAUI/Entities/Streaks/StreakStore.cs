using CommunityToolkit.Mvvm.Messaging;

namespace Streakathon.MAUI.Entities.Streaks
{
    public class StreakStore
    {
        private readonly List<Streak> _streaks;

        public IEnumerable<Streak> Streaks => _streaks;

        public StreakStore()
        {
            _streaks = new List<Streak>();
        }

        public void Add(Streak streak)
        {
            _streaks.Add(streak);

            StrongReferenceMessenger.Default.Send(new StreakAddedMessage(streak));
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
