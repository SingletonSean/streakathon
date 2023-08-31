namespace Streakathon.MAUI.Shared.Firestore
{
    public class FirestoreDocumentList<TFields>
    {
        public IEnumerable<FirestoreDocument<TFields>> Documents { get; set; }
    }
}
