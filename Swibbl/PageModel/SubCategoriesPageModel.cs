using Swibbl.Models.Shop;
using Swibbl.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;

namespace Swibbl.Pages
{
    public class SubCategoriesPageModel : BasePageModel
    {
        public SubCategoriesPageModel()
        {
            allsub_categories = new ObservableCollection<SubCategory>();
            sub_categories = new ObservableCollection<SubCategory>();
        }

        public override void Init(object initData)
        {
            var category = initData as Category;
            Load(category.Name);
        }

        async void Load(string name)
        {
            Title = name;
            State = LayoutState.Loading;
            await Task.Delay(1000);
            AllSubCategories.Clear();
            _ = Database.client.Child("subategories").AsObservable<SubCategory>().Subscribe((dbevent) =>
            {
                AllSubCategories.Add(dbevent.Object);
            });
            SubCategories.Clear();
            foreach (var p in AllSubCategories.Where(c => c.Category == name))
            {
                SubCategories.Add(p);
            }
            State = LayoutState.None;
        }

        SubCategory subCategory;
        public SubCategory SelectedSubCategory
        {
            get => subCategory;
            set
            {
                SetProperty(ref subCategory, value);
                Selected(SelectedSubCategory);
            }
        }

        async void Selected(SubCategory item)
        {
            if (item == null)
                return;

            SelectedSubCategory = null;
        
            object[] key = new object[] { null, null, item.Name };
            await CoreMethods.PushPageModel<ListPageModel>(key, true);
        }

        private ObservableCollection<SubCategory> sub_categories;
        public ObservableCollection<SubCategory> SubCategories
        {
            get => sub_categories;
            set => SetProperty(ref sub_categories, value);
        }
        private ObservableCollection<SubCategory> allsub_categories;
        public ObservableCollection<SubCategory> AllSubCategories
        {
            get => allsub_categories;
            set => SetProperty(ref allsub_categories, value);
        }
    }
}
