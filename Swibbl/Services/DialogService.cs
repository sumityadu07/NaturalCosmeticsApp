using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;

namespace Swibbl.Services
{
    /// <summary>
    /// Allows for popping a dialog up for the user.
    /// </summary>
    public static class DialogService
    {
        public static Task AlertAsync(string message, string title, string buttonLabel)
            => App.Current.MainPage.DisplayAlert(message, title, buttonLabel);
        public static Task AlertPopup(string message, string title, string buttonLabel)
            => App.Current.MainPage.DisplayAlert(message, title, buttonLabel);


        public static Task SnackBarAsync(string message, TimeSpan duration, SnackBarActionOptions snackbarAction)
        {
            var messageoptions = new MessageOptions()
            {
                Foreground = Color.Black,
                Font = Font.SystemFontOfSize(16),
                Message = message,
                Padding = 25
            };

            var snackbarOptions = new SnackBarOptions()
            {
                CornerRadius = 10,
                BackgroundColor = Color.White,
                Duration = duration,
                Actions = new[] { snackbarAction },
                MessageOptions = messageoptions,
            };

            return App.Current.MainPage.DisplaySnackBarAsync(snackbarOptions);
        }

        public static Task ToastAsync(string message)
            => (Task)Application.Current.MainPage.DisplayToastAsync(message);
    }
}
