namespace Streakathon.MAUI.Shared.Firestore
{
    public class FirestoreRunQueryRequest
    {
        public FirestoreStructuredQuery StructuredQuery { get; set; }
    }

    public class FirestoreStructuredQuery
    {
        public IEnumerable<FirestoreFromItem> From { get; set; }
        public FirestoreWhereItem Where { get; set; }
    }

    public class FirestoreFromItem
    {
        public string CollectionId { get; set; }
        public bool AllDescendants { get; set; }
    }

    public class FirestoreWhereItem
    {
        public FirestoreFieldFilter FieldFilter { get; set; }
    }

    public class FirestoreFieldFilter
    {
        public FirestoreField Field { get; set; }
        public string Op { get; set; }
        public FirestoreStringField Value { get; set; }
    }

    public class FirestoreField
    {
        public string FieldPath { get; set; }
    }
}
