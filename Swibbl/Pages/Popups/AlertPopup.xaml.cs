using Xamarin.CommunityToolkit.UI.Views;

namespace Swibbl.Pages.Popups
{
    public partial class AlertPopup : Popup<bool>
    {
        private string title;
        private string message;
        public AlertPopup(string _title, string _message)
        {
            InitializeComponent();

            title = _title;
            message = _message;
            head.Text = title;
            body.Text = message;
        }

        private void Cancel(object sender, System.EventArgs e)
        {
            Dismiss(false);
        }

        private void Ok(object sender, System.EventArgs e)
        {
            Dismiss(true);
        }
    }
}