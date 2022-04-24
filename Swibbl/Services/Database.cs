using Firebase.Database;

namespace Swibbl.Services
{
    public static class Database
    {
        public static readonly FirebaseClient client = new("https://swibbl-store-default-rtdb.asia-southeast1.firebasedatabase.app/");
    }
}
