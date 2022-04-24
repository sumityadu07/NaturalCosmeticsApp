using Swibbl.Models.Shop;
using Swibbl.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Swibbl.Pages
{
    public class MainPageModel : BasePageModel
    {
        public MainPageModel()
        {
            categories = new ObservableCollection<Category>();
            carousel = new ObservableCollection<Brand>();
            offers = new ObservableCollection<Brand>();

            if(!IsConnected)
            {
                State = LayoutState.Success; //No Connection//
            }
            else
            {
                State = LayoutState.Loading;
                LoadCategories();
                LoadOffers();
                LoadCarousel();
                State = LayoutState.None;
            }

        }

        protected override void LoadOnConnected()
        {
            if (IsConnected)
            {
                LoadCategories();
                LoadOffers();
                LoadCarousel();
            }
        }

        void LoadCategories()
        {
            categories.Clear();
            _ = Database.client.Child("categories").AsObservable<Category>().Subscribe(obj => categories.Add(obj.Object));
        }

        void LoadOffers()
        {
            offers.Clear();
            _ = Database.client.Child("offers").AsObservable<Brand>().Subscribe(obj => offers.Add(obj.Object));
        }
        void LoadCarousel()
        {
            carousel.Clear();
            Database.client.Child("carousel").AsObservable<Brand>().Subscribe(obj =>
            {
                Carousel.Add(obj.Object);
            });
        
            //Device.StartTimer(TimeSpan.FromSeconds(2), () =>
            //{
            //    Position = (Position + 1) % carousel.Count;
            //    return true;
            //});
        }

        public ICommand OpenSearchCommand => new AsyncCommand(async () =>
            await CoreMethods.PushPageModelWithNewNavigation<SearchPageModel>(true));
        public ICommand CategoryCommand => new AsyncCommand<Category>(SelectedCategory);
        public ICommand BrandCommand => new AsyncCommand<Brand>(ViewBrand);

        async Task ViewBrand(Brand brand)
        {
            if (brand == null)
                return;
            await NavigateTo(brand.Name, null, null);
        }

        async Task SelectedCategory(Category category)
        {
            if (category == null)
                return;
            await NavigateTo(null, category.Name, null);
        }

        async Task NavigateTo(string brand, string category, string subcategory)
        {
            object[] key = new object[] { brand, category, subcategory };
            await CoreMethods.PushPageModelWithNewNavigation<ListPageModel>(key, false);
        }

        private ObservableCollection<Brand> offers;
        public ObservableCollection<Brand> Offers
        {
            get => offers;
            set => SetProperty(ref offers, value);
        }


        private ObservableCollection<Category> categories;
        public ObservableCollection<Category> Categories
        {
            get => categories;
            set => SetProperty(ref categories, value);
        }

        private ObservableCollection<Brand> carousel;
        public ObservableCollection<Brand> Carousel
        {
            get => carousel;
            set => SetProperty(ref carousel, value);
        }

        private int position;
        public int Position
        {
            get { return position; }
            set => SetProperty(ref position, value);
        }

    }
}