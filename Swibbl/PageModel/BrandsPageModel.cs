using Swibbl.Models.Shop;
using Swibbl.Services;
using System;
using System.Collections.ObjectModel;

namespace Swibbl.Pages
{
    public class BrandsPageModel : BasePageModel
    {
        public BrandsPageModel()
        {
            brands = new ObservableCollection<Brand>();
            Load();
        }

        void Load()
        {
            brands.Clear();
            _ = Database.client.Child("brands").AsObservable<Brand>().Subscribe((obj) => brands.Add(obj.Object));
        }

        async void Selected(Brand brand)
        {
            if (brand == null)
                return;
            SelectedItem = null;

            object[] key = new object[] { brand.Name, null, null};
            await CoreMethods.PushPageModel<ListPageModel>(key);
        }

        private Brand selectedItem;
        public Brand SelectedItem
        {
            get => selectedItem;
            set
            {
                SetProperty(ref selectedItem, value);
                Selected(SelectedItem);
            }
        }


        private ObservableCollection<Brand> brands;
        public ObservableCollection<Brand> Brands
        {
            get => brands;
            set => SetProperty(ref brands, value);
        }
    }
}
