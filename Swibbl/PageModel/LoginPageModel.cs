using Swibbl.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Swibbl.Pages
{
    public class LoginPageModel : LoginBaseModel
    {
        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            if (AuthService.IsSignIn())
            {
                CoreMethods.SwitchOutRootNavigation(NavigationStack.MainAppStack);
            }
        }

        public ICommand SkipCommand => new AsyncCommand(Load);
        async Task Load() => await CoreMethods.PushPageModel<LoadPageModel>();
    }
}