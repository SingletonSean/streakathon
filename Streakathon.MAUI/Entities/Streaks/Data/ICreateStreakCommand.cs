using Refit;
using Streakathon.MAUI.Shared.Firestore;

namespace Streakathon.MAUI.Entities.Streaks.Data
{
    public interface ICreateStreakCommand
    {
        [Post("/streaks")]
        Task<CreateStreakCommandResponse> Execute([Body] CreateStreakCommandRequest request);
    }

    public class CreateStreakCommandRequest : FirestoreDocument<FirestoreStreakFields> { }

    public class CreateStreakCommandResponse : FirestoreDocument<FirestoreStreakFields> { }
}
