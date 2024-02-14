using Streakathon.MAUI.Entities.Streaks.Data;
using Streakathon.MAUI.Shared.Firestore;

namespace Streakathon.MAUI.Entities.Streaks
{
    public class CreateStreakCommand
    {
        private readonly ICreateStreakCommand _createStreakCommand;

        public CreateStreakCommand(ICreateStreakCommand createStreakCommand)
        {
            _createStreakCommand = createStreakCommand;
        }

        public async Task<Streak> Execute(NewStreak newStreak)
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
                        },
                        UserId = new FirestoreStringField()
                        {
                            StringValue = newStreak.UserId
                        }
                    }
                });

            return new Streak()
            {
                Id = createdStreakResponse.Id,
                Title = createdStreakResponse.Fields?.Title?.StringValue,
                Description = createdStreakResponse.Fields?.Description?.StringValue,
            };
        }
    }
}
