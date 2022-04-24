using Android.Content;
using Swibbl.Models;
using Swibbl.Models.Shop;
using Swibbl.Services;
using System;
using System.Collections.ObjectModel;

namespace Swibbl.Pages
{
    public class ShopPageModel : BasePageModel
    {

        public ShopPageModel()
        {
            categories = new ObservableCollection<Category>();
            Load();
        }

        protected override void LoadOnConnected()
        {
            Load();
        }

        void Load()
        {
            if (IsConnected)
            {
                categories.Clear();
                _ = Database.client.Child("categories").AsObservable<Category>().Subscribe((dbevent) =>
                {
                    Categories.Add(dbevent.Object);
                });
                
            }
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

        async void Selected(Category category)
        {
            if (category == null)
                return;
            SelectedItem = null;
            await CoreMethods.PushPageModelWithNewNavigation<SubCategoriesPageModel>(category,animate:false);
        }

        private ObservableCollection<Category> categories; //SubCategories Groups
        public ObservableCollection<Category> Categories
        {
            get => categories;
            set => SetProperty(ref categories, value);
        }
    }
}
