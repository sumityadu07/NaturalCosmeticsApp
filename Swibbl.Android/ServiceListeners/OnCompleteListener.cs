using Android.Gms.Tasks;
using Firebase.Firestore;
using Swibbl.Models.User;
using System.Threading.Tasks;

namespace Swibbl.Droid.ServiceListeners
{
    public class OnCompleteListener : Java.Lang.Object, IOnCompleteListener
    {
        private TaskCompletionSource<UserInfo> _tcs;

        public OnCompleteListener(TaskCompletionSource<UserInfo> tcs)
        {
            _tcs = tcs;
        }

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task.IsSuccessful)
            {
                var result = task.Result;
                if (result is DocumentSnapshot doc)
                {
                    var user = new UserInfo();
                    user.Id = doc.Id;
                    user.Email = doc.GetString("email");
                    user.Name = doc.GetString("name");
                    _tcs.TrySetResult(user);
                    return;
                }
            }
            _tcs.TrySetResult(default(UserInfo));
        }
    }
}
