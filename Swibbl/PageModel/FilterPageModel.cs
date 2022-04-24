using Swibbl.Models.Shop;
using System;
using Swibbl.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;

namespace Swibbl.Pages
{
    public class FilterPageModel : BasePageModel
    {
        public FilterPageModel()
        {
            sub_categories = new ObservableCollection<SubCategory>();
            selectedSubs = new ObservableCollection<SubCategory>();
            allsub_categories = new ObservableCollection<SubCategory>();
            categories = new ObservableCollection<Category>();
            LoadFilters();
        }

        void LoadFilters()
        {
            if (!IsConnected)
                return;
            
            State = LayoutState.Loading;
            categories.Clear();
            _ = Database.client.Child("filtercategories").AsObservable<Category>().Subscribe((dbevent) => categories.Add(dbevent.Object));

            allsub_categories.Clear();
            _ = Database.client.Child("filtersubcategories").AsObservable<SubCategory>().Subscribe((dbevent) => allsub_categories.Add(dbevent.Object));
            State = LayoutState.None;

        }

        void Selected(Category category)
        {
            if (category == null)
                return;

            sub_categories.Clear();
            var subs = allsub_categories.Where(c => c.Category == category.Name);

            foreach (var p in subs)
                sub_categories.Add(p);
        }


        public ICommand ApplyFilterCommand => new AsyncCommand(async () => await CoreMethods.PopPageModel(selectedSubs, animate: false));

        private ObservableCollection<SubCategory> sub_categories;
        public ObservableCollection<SubCategory> SubCategories
        {
            get { return sub_categories; }
            set => SetProperty(ref sub_categories, value);
        }

        private ObservableCollection<SubCategory> allsub_categories;
        public ObservableCollection<SubCategory> AllSubCategories
        {
            get { return allsub_categories; }
            set => SetProperty(ref allsub_categories, value);
        }

        private ObservableCollection<Category> categories; //SubCategories Groups
        public ObservableCollection<Category> Categories
        {
            get { return categories; }
            set => SetProperty(ref categories, value);
        }

        private Category selectedItem;
        public Category SelectedItem
        {
            get => selectedItem;
            set
            {
                SetProperty(ref selectedItem, value);
                Selected(SelectedItem);
            }
        }
        private ObservableCollection<SubCategory> selectedSubs;
        public ObservableCollection<SubCategory> SelectedSubs
        {
            get => selectedSubs;
            set => SetProperty(ref selectedSubs, value);
        }

    }
}
