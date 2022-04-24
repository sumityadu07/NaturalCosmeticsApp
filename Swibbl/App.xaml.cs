using FreshMvvm;
using Plugin.FirebasePushNotification;
using Swibbl.Pages;
using Swibbl.Services;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace Swibbl
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            InitializeComponent();
            InitNavigation();
        }

        public void InitNavigation()
        {
            var loginpage = FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
            var LoginStack = new FreshNavigationContainer(loginpage, NavigationStack.LoginNavigationStack);
            var FreshShell = new FreshTabbedNavigationContainer(NavigationStack.MainAppStack);

            //FreshShell.On<Android>().DisableSmoothScroll();
            //FreshShell.On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            //FreshShell.On<Android>().SetIsSwipePagingEnabled(false);
            
            var categoryicon = new FontImageSource() { Glyph = FontAwesome.FontAwesomeIcons.BarsStaggered, FontFamily = "FAS"};
            var brand = new FontImageSource() { Glyph = FontAwesome.FontAwesomeIcons.Tags, FontFamily = "FAS"};
            var menuicon = new FontImageSource() { Glyph = FontAwesome.FontAwesomeIcons.User, FontFamily = "FAR"};

            FreshShell.AddTab<MainPageModel>("Home", "home.png", null);
            FreshShell.AddTab<ShopPageModel>("Categories", null, null).IconImageSource = categoryicon;
            FreshShell.AddTab<BrandsPageModel>("Brands", null, null).IconImageSource = brand;
            FreshShell.AddTab<MenuPageModel>("Profile", null, null).IconImageSource = menuicon;
            
            FreshShell.SelectedTabColor = Color.FromHex("#d0f0c0");
            FreshShell.UnselectedTabColor = Color.Gray;
            FreshShell.BarBackgroundColor = Color.White;
            FreshShell.BarTextColor = Color.Gray;
            
            MainPage = LoginStack;
        }

        protected override void OnStart()
        {
            var guestid = Preferences.Get("guestId", "nil");
            
            if(guestid == "nil")
            {
                Preferences.Set("guestId", Guid.NewGuid().ToString().Substring(0, 28).ToUpper());
            }

            CrossFirebasePushNotification.Current.Subscribe("General");
        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {
        }

    }
}
