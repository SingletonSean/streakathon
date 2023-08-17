using Refit;

namespace Streakathon.MAUI.Entities.Streaks.Data
{

    public interface IGetStreakEntriesQuery
    {
        [Get("/streaks/{streakId}/entries")]
        Task<FirestoreQueryResponse<GetStreakEntriesQueryFieldsResponse>> Execute(string streakId);
    }
}
