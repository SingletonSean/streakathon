namespace Streakathon.MAUI.Entities.Streaks
{
    public class NewStreak
    {
        public string Title { get; }
        public string Description { get; }

        public NewStreak(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}
