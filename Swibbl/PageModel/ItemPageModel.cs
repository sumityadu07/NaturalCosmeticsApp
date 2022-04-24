using Swibbl.Models;
using Swibbl.Models.Shop;
using Swibbl.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Swibbl.Pages
{
    public class ItemPageModel : BasePageModel
    {
        IRepository<Product> _itemrepository = DependencyService.Get<IRepository<Product>>();
        protected IRepository<Review> reviewrepository = DependencyService.Get<IRepository<Review>>();

        public ItemPageModel()
        {
            categoryitems = new ObservableCollection<Product>();
            reviews = new ObservableCollection<Review>();
        }

        public override void Init(object initData)
        {
            Product = initData as Product;
            LoadReviews();
            LoadCategories();
        }

        async void LoadReviews()
        {
            Reviews.Clear();
            Reviews = await reviewrepository.GetCollectionbyItemUid("reviews",product.Id, 2);
        }

        async void LoadCategories()
        {
            Categoryitems.Clear();
            Categoryitems = await _itemrepository.SortItems(subcategory: product.SubCategory);
        }

        public ICommand ShareCommand => new AsyncCommand(ShareUri);
        public ICommand ViewDetailsCommand => new AsyncCommand(async () => await CoreMethods.PushPageModel<ProductDetailsPageModel>(product));
        public ICommand BuyCommand => new AsyncCommand(Buy);
        public ICommand ViewReviewsCommand => new AsyncCommand(async () => await CoreMethods.PushPageModel<ReviewsPageModel>());
        public ICommand ViewMoreCommand => new AsyncCommand(ViewMore);
        public ICommand WishlistCommand => new AsyncCommand(AddtoWishlist);
        public ICommand AddCartCommand => new AsyncCommand(SaveToCart);
        public ICommand ViewItemCommand => new AsyncCommand(SaveToCart);

        
        async Task AddtoWishlist()
        {
            if (!AuthService.IsSignIn())
                await CoreMethods.PushPageModel<LoginDockPageModel>();
            else
                await _itemrepository.Wishlist(product);
        }

        async Task ViewMore()
        {
            var key = new[] { null, null, product.Category };
            await CoreMethods.PushPageModel<ListPageModel>(key, animate: false);
        }

        async Task SaveToCart()
        {
            if (product == null)
                return;
            await _itemrepository.CreateCart(product);
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
        async Task Buy() => await CoreMethods.PushPageModel<AddressPageModel>();

        public async Task ShareUri()
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                //Uri = uri,
                Title = product.Id
            });
        }

        private Product product;
        public Product Product
        {
            get { return product; }
            set => SetProperty(ref product, value);
        }

        private ObservableCollection<Product> categoryitems;
        public ObservableCollection<Product> Categoryitems
        {
            get { return categoryitems; }
            set => SetProperty(ref categoryitems, value);
        }

        private Product selectedItem;
        public Product SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }


        protected ObservableCollection<Review> reviews;
        public ObservableCollection<Review> Reviews
        {
            get => reviews;
            set => SetProperty(ref reviews, value);
        }
    }
}
