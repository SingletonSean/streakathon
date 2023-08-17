using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streakathon.MAUI.Entities.Streaks.Data
{
    public class FirestoreQueryResponse<TFieldsResponse>
    {
        public IEnumerable<FirestoreQueryDocumentResponse<TFieldsResponse>> Documents { get; set; }
    }

    public class FirestoreQueryDocumentResponse<TFieldsResponse>
    {
        public string Name { get; set; }
        public TFieldsResponse Fields { get; set; }
    }

    public class GetAllStreaksQueryFieldsResponse
    {
        public FirestoreQueryStringFieldResponse Title { get; set; }
        public FirestoreQueryStringFieldResponse Description { get; set; }
    }

    public class GetStreakEntriesQueryFieldsResponse
    {
        public FirestoreQueryTimestampFieldResponse Created { get; set; }
    }

    public class FirestoreQueryStringFieldResponse
    {
        public string StringValue { get; set; }
    }

    public class FirestoreQueryTimestampFieldResponse
    {
        public DateTime TimestampValue { get; set; }
    }
}
