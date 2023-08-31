namespace Streakathon.MAUI.Entities.Streaks
{
    public class StreakEntry
    {
        public string Id { get; }
        public string StreakId { get; }
        public DateTime Created { get; }

        public StreakEntry(string id, string streakId, DateTime created)
        {
            Id = id;
            StreakId = streakId;
            Created = created;
        }
    }
}
