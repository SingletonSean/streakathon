using Refit;
using Streakathon.MAUI.Shared.Firestore;

namespace Streakathon.MAUI.Entities.Streaks.Data
{

    public interface IGetAllStreakEntriesQuery
    {
        [Post("/documents:runQuery")]
        Task<IEnumerable<StreakEntryCollectionGroupItem>> Execute([Body] FirestoreRunQueryRequest request);
    }

    public class StreakEntryCollectionGroupItem : FirestoreCollectionGroupItem<FirestoreStreakEntryFields>
    {
        public string StreakId => Document?.Name?.Split("/")?.TakeLast(3)?.FirstOrDefault();
        public string StreakEntryId => Document?.Name?.Split("/")?.LastOrDefault();
    }
}
