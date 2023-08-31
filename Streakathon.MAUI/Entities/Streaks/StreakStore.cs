using CommunityToolkit.Mvvm.Messaging;
using Streakathon.MAUI.Entities.Streaks.Data;
using Streakathon.MAUI.Shared.Firestore;

namespace Streakathon.MAUI.Entities.Streaks
{
    public class StreakStore
    {
        private readonly ICreateStreakCommand _createStreakCommand;
        private readonly List<Streak> _streaks;

        public IEnumerable<Streak> Streaks => _streaks;

        public StreakStore(ICreateStreakCommand createStreakCommand)
        {
            _createStreakCommand = createStreakCommand;

            _streaks = new List<Streak>();
        }

        public async Task Create(Streak newStreak)
        {
            CreateStreakCommandResponse createdStreakResponse = 
                await _createStreakCommand.Execute(new CreateStreakCommandRequest()
                {
                    Fields = new FirestoreStreakFields()
                    {
                        Title = new FirestoreStringField()
                        {
                            StringValue = newStreak.Title
                        },
                        Description = new FirestoreStringField()
                        {
                            StringValue = newStreak.Description
                        }
                    }
                });

            _streaks.Add(newStreak);

            StrongReferenceMessenger.Default.Send(new StreakAddedMessage(newStreak));
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
