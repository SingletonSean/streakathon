namespace Streakathon.MAUI.Entities.Streaks
{
    public class StreakEntryAddedMessage
    {
        public Streak Streak { get; }
        public StreakEntry StreakEntry { get; }

        public StreakEntryAddedMessage(Streak streak, StreakEntry streakEntry)
        {
            Streak = streak;
            StreakEntry = streakEntry;
        }
    }
}
