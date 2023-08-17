using Refit;

namespace Streakathon.MAUI.Entities.Streaks.Data
{

    public interface IGetAllStreaksQuery
    {
        [Get("/streaks")]
        Task<FirestoreQueryResponse<GetAllStreaksQueryFieldsResponse>> Execute();
    }
}
