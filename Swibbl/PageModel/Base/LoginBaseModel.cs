using Swibbl.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Swibbl.Pages
{
    public class LoginBaseModel : BasePageModel
    {
        private string phone;
        public ICommand SendCodeCommand => new AsyncCommand<string>(SendOtp);

        public async Task SendOtp(string str)
        {
            if (!string.IsNullOrEmpty(Phone) && Phone.Length == 10)
            {
                var arry = new string[] {str,Phone };
                await CoreMethods.PushPageModel<VerifyCodePageModel>(arry,animate:false);
                await AuthService.SendOtpCodeAsync(Phone);
            }
            else
            {
                await DialogService.ToastAsync("Check the number entered");
            }
        }

        public string Phone
        {
            get => phone;
            set => SetProperty(ref phone, value);
        }
    }
}