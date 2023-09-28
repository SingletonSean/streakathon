using Refit;
using Streakathon.MAUI.Shared.Firestore;
using System.Reflection.Metadata;

namespace Streakathon.MAUI.Entities.Streaks.Data
{
    public interface ICreateStreakEntryCommand
    {
        [Post("/streaks/{streakId}/entries")]
        Task<CreateStreakEntryCommandResponse> Execute(string streakId, [Body] CreateStreakEntryCommandRequest request);
    }

    public class CreateStreakEntryCommandRequest : FirestoreDocument<FirestoreStreakEntryFields> { }

    public class CreateStreakEntryCommandResponse : FirestoreDocument<FirestoreStreakEntryFields> 
    {
        public string StreakId => Name?.Split("/")?.TakeLast(3)?.FirstOrDefault();
        public string StreakEntryId => Name?.Split("/")?.LastOrDefault();
    }
}
