using Refit;
using Streakathon.MAUI.Shared.Firestore;

namespace Streakathon.MAUI.Entities.Streaks.Data
{
    public interface IGetAllStreaksQuery
    {
        [Get("/streaks")]
        Task<GetAllStreaksQueryResponse> Execute();
    }

    public class GetAllStreaksQueryResponse : FirestoreDocumentList<FirestoreStreakFields> { }
}
