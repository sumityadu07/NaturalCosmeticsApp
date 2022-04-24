using Swibbl.Models;
using System.Collections.Generic;
using System.Linq;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.Xaml;

namespace Swibbl.Pages.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SortPopup : Popup<string>
    {
        SortPrefernce prefernce;
        public SortPrefernce Preference
        { 
            get => prefernce;
            set
            {
                prefernce = value;
                OnPropertyChanged("Preference");
                Close(Preference);
            }
        }

        private IList<SortPrefernce> _prefernces;
        public SortPopup(IList<SortPrefernce> preferences)
        {
            InitializeComponent();
            _prefernces = preferences;
            collection.ItemsSource = _prefernces;
            collection.SelectedItem = Preference;
            BindingContext = this;
        }

        void Close(SortPrefernce prefernce)
        {
            Dismiss(prefernce.Name);
        }

    }
}