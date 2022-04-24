
using Swibbl.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Swibbl.Pages
{
    public class VerifyCodePageModel : BasePageModel
    {
        private string code;
        private string count;
        private string pageType;
        public string phone;

        public VerifyCodePageModel()
        {
            StartTimer = TimeSpan.FromSeconds(30);
            Count = StartTimer.Seconds.ToString();
        }

        void RunTimer()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (StartTimer.TotalSeconds > 0)
                {
                    StartTimer = StartTimer - TimeSpan.FromSeconds(1);
                    Count = StartTimer.Seconds.ToString();
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }
        public ICommand LoginCommand => new AsyncCommand(LoginAction);
        public ICommand ResendCodeCommand => new AsyncCommand(ResendOtp);
        public TimeSpan StartTimer { get; set; }

        public override void Init(object initData)
        {
            RunTimer();
            var result = initData as object[];
            pageType = result[0] as string;
            phone = result[1] as string;
        }

        async Task LoginAction()
        {
            if (!string.IsNullOrEmpty(Code) && Code.Length == 6)
            {
                bool isVerified = await AuthService.VerifyOtpCodeAsync(Code);
                if (isVerified)
                {
                    await Login();
                    Code = null;
                }
                else
                {
                    await DialogService.ToastAsync("Check the code entered");
                }
            }
            else
            {
                await DialogService.ToastAsync("Check the number entered");
            }
        }

        async Task Login()
        {
            IsBusy = true;
            switch (pageType)
            {
                case "Main":
                    await CoreMethods.PushPageModel<LoadPageModel>();
                    break;
                case "Dock":
                    await CoreMethods.PopToRoot(true);
                    break;
                case "Update":
                    await AuthService.VerifyAndUpdateNumber(Code);
                    break;
            }
            IsBusy = false;
        }


        public async Task ResendOtp()
        {
            if (Count != "0")
                return;

            RunTimer();
            await AuthService.ResendOtpCodeAsync(phone);
        }

        public string Code
        {
            get => code;
            set => SetProperty(ref code, value);
        }
        public string Count
        {
            get => count;
            set => SetProperty(ref count, value);
        }
    }
}
