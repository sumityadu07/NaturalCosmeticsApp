using Android.Gms.Tasks;
using Firebase.Firestore;
using Swibbl.Droid.Extensions;
using Swibbl.Services;
using System.Threading.Tasks;

namespace Swibbl.Droid.ServiceListeners
{
    public class OnDocumentCompleteListener<T> : Java.Lang.Object, IOnCompleteListener where T : IIdentifiable
    {
        private TaskCompletionSource<T> _tcs;

        public OnDocumentCompleteListener(TaskCompletionSource<T> tcs)
        {
            _tcs = tcs;
        }

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task.IsSuccessful)
            {
                var docObj = task.Result;
                if (docObj is DocumentSnapshot docRef)
                {
                    _tcs.TrySetResult(docRef.Convert<T>());
                    return;
                }
            }
            _tcs.TrySetResult(default(T));
        }
    }
}
