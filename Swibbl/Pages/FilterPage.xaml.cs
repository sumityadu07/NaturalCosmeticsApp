
using Swibbl.Pages.Popups;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Swibbl.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterPage : ContentPage
    {
        public FilterPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            Pop();
            return true;
        }

        async void Pop()
        {
            var result = await Application.Current.MainPage.Navigation.ShowPopupAsync(new AlertPopup("Unapplied Filters", "Do you want to apply filters?"));
            if (!result)
                return;
            await Navigation.PopAsync(animated:false);
        }
    }
}