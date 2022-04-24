using Swibbl.Models.Shop;
using Swibbl.Models.User;
using Swibbl.Pages.Popups;
using Swibbl.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Swibbl.Pages
{
    public class MenuPageModel : LoginBaseModel
    {
        IRepository<UserInfo> Repository = DependencyService.Resolve<IRepository<UserInfo>>();

        public string Version { get; set; }
        private string name;
        private string email;

        public MenuPageModel()
        {
            Load();
            Version = AppInfo.VersionString;
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            if (AuthService.IsSignIn())
                Phone = AuthService.Phone();
        }

        public override void ReverseInit(object returnedData)
        {
            Load();
        }

        async void Load()
        {
            if (AuthService.IsSignIn())
            {
                var user = await Repository.GetUserAsync();
                Name = user.Name;
                Email = user.Email;
                Preferences.Set("name", Name);
                Preferences.Set("email", Email);
            }
        }

        public ICommand EditProfileCommand => new AsyncCommand(async () => await CoreMethods.PushPageModelWithNewNavigation<AccountPageModel>(true));
        public ICommand AboutCommand => new AsyncCommand(async () => await CoreMethods.PushPageModelWithNewNavigation<AboutPageModel>(true));
        public ICommand CareCommand => new AsyncCommand(async () => await CoreMethods.PushPageModelWithNewNavigation<CustomerCarePageModel>(true));
        public ICommand OpenLoginCommand => new AsyncCommand(async () => await CoreMethods.PushPageModelWithNewNavigation<LoginDockPageModel>(true));
        public ICommand WishlistCommand => new AsyncCommand(async () => await CoreMethods.PushPageModelWithNewNavigation<WishlistPageModel>(true));
        public ICommand OrdersCommand => new AsyncCommand(async () => await CoreMethods.PushPageModelWithNewNavigation<OrdersPageModel>(true));
        public ICommand CartCommand => new AsyncCommand(async () => await CoreMethods.PushPageModelWithNewNavigation<CartPageModel>(true));
        public ICommand AddressCommand => new AsyncCommand(async () => await CoreMethods.PushPageModelWithNewNavigation<AddressPageModel>(new Cart(),true));
        public ICommand SignOutCommand => new Command(OnSignOut);

        async void OnSignOut()
        {
            State = LayoutState.Loading;
            await Task.Delay(1000);
            AuthService.SignOut();
            State = LayoutState.None;
            Phone = null;
            MessagingCenter.Send(this, "LoggedOut");
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }
        
    }
}
