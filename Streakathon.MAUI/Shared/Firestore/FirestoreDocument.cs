namespace Streakathon.MAUI.Shared.Firestore
{
    public class FirestoreDocument<TFields>
    {
        public string Name { get; set; }
        public TFields Fields { get; set; }
    }
}
