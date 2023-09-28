using Streakathon.MAUI.Entities.Streaks.Data;
using Streakathon.MAUI.Shared.Firestore;

namespace Streakathon.MAUI.Entities.Streaks
{
    public class CreateStreakEntryCommand
    {
        private readonly ICreateStreakEntryCommand _createStreakEntryCommand;

        public CreateStreakEntryCommand(ICreateStreakEntryCommand createStreakEntryCommand)
        {
            _createStreakEntryCommand = createStreakEntryCommand;
        }

        public async Task<StreakEntry> Execute(string streakId)
        {
            CreateStreakEntryCommandRequest request = new CreateStreakEntryCommandRequest()
            {
                Fields = new FirestoreStreakEntryFields()
                {
                    Created = new FirestoreTimestampField()
                    {
                        TimestampValue = DateTime.Now
                    }
                }
            };

            CreateStreakEntryCommandResponse response = await _createStreakEntryCommand.Execute(streakId, request);

            return new StreakEntry(
                response.StreakEntryId, 
                response.StreakId,
                response.Fields.Created.TimestampValue);
        }
    }
}
