namespace StreakTracker.MAUI.Entities.Streaks
{
    public class StreakEntry
    {
        public DateTime Created { get; }

        public StreakEntry(DateTime created)
        {
            Created = created;
        }
    }
}
