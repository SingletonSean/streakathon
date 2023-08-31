using CommunityToolkit.Mvvm.Messaging;
using Streakathon.MAUI.Entities.Streaks.Data;

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
            FirestoreQueryDocumentResponse<GetAllStreaksQueryFieldsResponse> createdStreakResponse = 
                await _createStreakCommand.Execute(new FirestoreQueryDocumentResponse<GetAllStreaksQueryFieldsResponse>()
                {
                    Fields = new GetAllStreaksQueryFieldsResponse()
                    {
                        Title = new FirestoreQueryStringFieldResponse()
                        {
                            StringValue = newStreak.Title
                        },
                        Description = new FirestoreQueryStringFieldResponse()
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
