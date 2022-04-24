using System.Threading.Tasks;

namespace Swibbl.Services
{
    public interface IAuthentication
    {
        void SignOut();
        string CurrentUid();
        string Phone();
        bool IsSignIn();
        Task<bool> SendOtpCodeAsync(string phoneNumber);
        Task<bool> ResendOtpCodeAsync(string phoneNumber);
        Task<bool> VerifyEmail(string email);
        Task<bool> VerifyOtpCodeAsync(string code);
        Task<bool> VerifyAndUpdateNumber(string code);
    }
}
