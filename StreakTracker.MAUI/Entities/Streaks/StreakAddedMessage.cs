namespace StreakTracker.MAUI.Entities.Streaks
{
    public class StreakAddedMessage
    {
        public Streak Streak { get; }

        public StreakAddedMessage(Streak streak)
        {
            Streak = streak;
        }
    }
}
