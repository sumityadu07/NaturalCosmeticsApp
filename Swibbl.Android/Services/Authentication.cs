using Firebase;
using Firebase.Auth;
using Java.Util.Concurrent;
using Swibbl.Droid;
using Swibbl.Services;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly:Dependency(typeof(Authentication))]
namespace Swibbl.Droid
{
    public class Authentication : PhoneAuthProvider.OnVerificationStateChangedCallbacks, IAuthentication
    {

        const int OTP_TIMEOUT = 30;
        private TaskCompletionSource<bool> _phoneAuthTcs;
        private string _verificationId;
        private PhoneAuthProvider.ForceResendingToken resendtoken;
        public Authentication()
        {
        }

        public string CurrentUid() => FirebaseAuth.Instance.CurrentUser.Uid;
        public string userName;
        public string Phone() => FirebaseAuth.Instance.CurrentUser.PhoneNumber;

        public bool IsSignIn()
            => FirebaseAuth.Instance.CurrentUser != null;

        public void SignOut()
            => FirebaseAuth.Instance.SignOut();

        public Task<bool> VerifyAndUpdateNumber(string code)
        {
            if (!string.IsNullOrWhiteSpace(_verificationId))
            {
                var credential = PhoneAuthProvider.GetCredential(_verificationId, code);
                var tcs = new TaskCompletionSource<bool>();
                FirebaseAuth.Instance.CurrentUser.UpdatePhoneNumberAsync(credential)
                    .ContinueWith((task) => OnAuthCompleted(task, tcs));

                var userProfileChangeRequestBuilder = new UserProfileChangeRequest.Builder();
                userProfileChangeRequestBuilder.SetDisplayName(userName);

                return tcs.Task;
            }
            return Task.FromResult(false);
        }

        public Task<bool> VerifyEmail(string email)
        {
            var verify = new TaskCompletionSource<bool>();
            var action = ActionCodeSettings.NewBuilder()
                .SetUrl("")
                .SetHandleCodeInApp(true)
                .SetAndroidPackageName("com.swibblinc.swibbl", true, "8")
                .Build();
            FirebaseAuth.Instance.SendSignInLinkToEmail(email, action);
            return verify.Task;
        }

        [System.Obsolete]
        public Task<bool> SendOtpCodeAsync(string phoneNumber)
        {
            _phoneAuthTcs = new TaskCompletionSource<bool>();
            PhoneAuthProvider.Instance.VerifyPhoneNumber(
                 "+91" + phoneNumber,
                 OTP_TIMEOUT,
                 TimeUnit.Seconds,
                 Platform.CurrentActivity,
                 this);
            return _phoneAuthTcs.Task;
        }
         [System.Obsolete]
        public Task<bool> ResendOtpCodeAsync(string phoneNumber)
        {
            _phoneAuthTcs = new TaskCompletionSource<bool>();
            PhoneAuthProvider.Instance.VerifyPhoneNumber(
                 "+91" + phoneNumber,
                 OTP_TIMEOUT,
                 TimeUnit.Seconds,
                 Platform.CurrentActivity,
                 this,
                 resendtoken);
            return _phoneAuthTcs.Task;
        }

        public Task<bool> VerifyOtpCodeAsync(string code)
        {
            if (!string.IsNullOrWhiteSpace(_verificationId))
            {
                var credential = PhoneAuthProvider.GetCredential(_verificationId, code);
                var tcs = new TaskCompletionSource<bool>();
                FirebaseAuth.Instance.SignInWithCredentialAsync(credential)
                    .ContinueWith((task) => OnAuthCompleted(task, tcs));
                return tcs.Task;
            }
            return Task.FromResult(false);
        }

        public override void OnCodeSent(string verificationId, PhoneAuthProvider.ForceResendingToken forceResendingToken)
        {
            base.OnCodeSent(verificationId, forceResendingToken);
            _verificationId = verificationId;
            resendtoken = forceResendingToken;
            _phoneAuthTcs?.TrySetResult(true);
        }

        public override void OnCodeAutoRetrievalTimeOut(string p0)
        {
            base.OnCodeAutoRetrievalTimeOut(p0);
        }


        private void OnAuthCompleted(Task task, TaskCompletionSource<bool> tcs)
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                tcs.SetResult(false);
                return;
            }
            _verificationId = null;
            tcs.SetResult(true);
        }
        public override void OnVerificationFailed(FirebaseException exception)
        {
            System.Diagnostics.Debug.WriteLine("Verification Failed: " + exception.Message);
            _phoneAuthTcs?.TrySetResult(false);
        }

        public override void OnVerificationCompleted(PhoneAuthCredential credential)
        {
            System.Diagnostics.Debug.WriteLine("Verification Failed: " + credential);
        }

    }
}