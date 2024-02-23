namespace Streakathon.MAUI.Entities.Streaks
{
    public class NewStreak
    {
        public string Title { get; }
        public string Description { get; }
        public string UserId { get; }

        public NewStreak(string title, string description, string userId)
        {
            Title = title;
            Description = description;
            UserId = userId;
        }
    }
}
