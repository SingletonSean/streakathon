using Refit;
using System.Linq;

namespace Streakathon.MAUI.Entities.Streaks.Data
{

    public interface IGetAllStreakEntriesQuery
    {
        [Post("/documents:runQuery")]
        Task<IEnumerable<StreakEntryDocumentResponse>> Execute([Body] Request request);
    }

    public class Request
    {
        public StructuredQuery StructuredQuery { get; set; } = new StructuredQuery();
    }

    public class StructuredQuery
    {
        public IEnumerable<FromItem> From { get; set; } = new List<FromItem>()
        {
            new FromItem()
        };
    }

    public class FromItem
    {
        public string CollectionId { get; set; } = "entries";
        public bool AllDescendants { get; set; } = true;
    }

    public class StreakEntryDocumentResponse
    {
        public Document Document { get; set; }

        public string StreakId => Document?.Name?.Split("/")?.TakeLast(3)?.FirstOrDefault();
        public string StreakEntryId => Document?.Name?.Split("/")?.LastOrDefault();
    }

    public class Document
    {
        public string Name { get; set; }
        public Fields Fields { get; set; }
    }

    public class Fields
    {
        public Created Created { get; set; }
    }

    public class Created
    {
        public DateTime TimestampValue { get; set; }
    }
}
