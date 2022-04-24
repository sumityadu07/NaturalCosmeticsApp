using Swibbl.Models;
using Swibbl.Models.Shop;
using Swibbl.Pages.Popups;
using Swibbl.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;

namespace Swibbl.Pages
{
    public class ListPageModel : BasePageModel
    {
        IRepository<Product> _repository = DependencyService.Get<IRepository<Product>>();
        
        string brand;
        string category;
        string subcategory;
        int id;
        
        public ListPageModel()
        {
            products = new ObservableCollection<Product>();
        }
        
        public override void Init(object initData)
        {
            var result = initData as object[];
            brand = result[0] as string;
            category = result[1] as string;
            subcategory = result[2] as string;
            Initalize();
        }

        async void Initalize()
        {
            try
            {
                State = LayoutState.Loading;
                await Task.Delay(1000);
                Products.Clear();
                Load();
                State = LayoutState.None;
            }
            catch (Exception)
            {
                State = LayoutState.Error;
            }
        }


        #region Command
        public ICommand WishlistCommand => new AsyncCommand<Product>(AddtoWishlist);
        public ICommand AddCartCommand => new AsyncCommand<Product>(SaveToCart);
        public ICommand OpenFilterCommand => new AsyncCommand(async () => await CoreMethods.PushPageModel<FilterPageModel>(null, animate: false));
        public ICommand SortCommand => new AsyncCommand(Pop);
        public ICommand FilterThresholdCommand => new Command(Load);
        public ICommand ProductsThresholdCommand => new AsyncCommand(Threshold);
        public ICommand ClearFilterCommand => new Command(Load);
        #endregion

        async void Load()
        {
            if (brand != null)
            {
                id = 1;
                Title = brand;
                Products = await _repository.SortItems(brand: brand);
            }

            if (category != null)
            {
                id = 2;
                Title = category;
                Products = await _repository.SortItems(category: category);
            }

            if (subcategory != null)
            {
                id = 3;
                Title = subcategory;
                Products = await _repository.SortItems(subcategory: subcategory);
            }
        }

        async Task Threshold()
        {
            IsBusy = true;
            var lastdoc = Products.Last().Id;
            if(id == 1)
            {
                Products = await _repository.SortItems(brand:Title,last_doc_id:lastdoc);
            }
            if(id == 2)
            {
                Products = await _repository.SortItems(category:Title,last_doc_id:lastdoc);
            }
            if(id == 3)
            {
                Products = await _repository.SortItems(subcategory:Title,last_doc_id:lastdoc);
            }
            IsBusy = false;
        }
        async Task Pop()
        {
            var sortPreferences = new List<SortPrefernce>
            {
                new SortPrefernce{ Name = "Discount: Low to High" },
                new SortPrefernce{ Name = "Discount: High to Low"},
                new SortPrefernce{ Name = "Price: Low to High"},
                new SortPrefernce{ Name = "Price: High to Low" },
                new SortPrefernce{ Name = "Sort by Popularity" }
            };

            SortAs = await Application.Current.MainPage.Navigation.ShowPopupAsync(new SortPopup(sortPreferences));
        }
        async void Selected(Product item)
        {
            if (item == null)
                return;
            SelectedItem = null;
            await CoreMethods.PushPageModel<ItemPageModel>(item, animate: false);
        }

        async Task AddtoWishlist(Product item)
        {
            if (!AuthService.IsSignIn())
                await CoreMethods.PushPageModel<LoginDockPageModel>();
            else
                await _repository.Wishlist(item);
        }

        async Task SaveToCart(Product product)
        {
            if (product == null)
                return;

            await _repository.CreateCart(product);
            var action = new SnackBarActionOptions()
            {
                ForegroundColor = Color.White,
                BackgroundColor = Color.FromHex("#6422b8"),
                Font = Font.SystemFontOfSize(14),
                Text = "View Cart",
                Action = async () => // null by default	
                {
                    await CoreMethods.PushPageModel<CartPageModel>();
                }
            };
            await DialogService.SnackBarAsync("Added to Cart", TimeSpan.FromSeconds(5), action);
        }

        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get => products;
            set => SetProperty(ref products, value);
        }

        private Product selectedItem;
        public Product SelectedItem
        {
            get => selectedItem;
            set
            {
                SetProperty(ref selectedItem, value);
                Selected(SelectedItem);
            }
        }
        

        private string sortAs = "None";
        public string SortAs
        {
            get => sortAs;
            set => SetProperty(ref sortAs, value);
        }

    }
}
