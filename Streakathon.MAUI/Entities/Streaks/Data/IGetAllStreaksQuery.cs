using Refit;
using Streakathon.MAUI.Shared.Firestore;

namespace Streakathon.MAUI.Entities.Streaks.Data
{
    public interface IGetAllStreaksQuery
    {
        [Post("/documents:runQuery")]
        Task<IEnumerable<StreakCollectionGroupItem>> Execute([Body] FirestoreRunQueryRequest request);
    }

    public class StreakCollectionGroupItem : FirestoreCollectionGroupItem<FirestoreStreakFields> { }
}
