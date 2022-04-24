using Swibbl.Models.Shop;
using Swibbl.Services;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;

namespace Swibbl.Pages
{
    class WishlistPageModel : BasePageModel
    {
        readonly IRepository<Product> _repository = DependencyService.Get<IRepository<Product>>();

        public WishlistPageModel()
        {
            wishes = new ObservableCollection<Product>();
            GetCollection();
        }

        async void GetCollection()
        {
            State = LayoutState.Loading;
            await Task.Delay(1000);
            Wishes.Clear();
            Wishes = await _repository.GetCollectionbyUserId("wishes");
            if (Wishes.Count != 0)
                State = LayoutState.None;
            else
                State = LayoutState.Empty;
        }

        public ICommand DeleteCommand => new AsyncCommand<Product>(Delete);

        public ICommand AddCommand => new AsyncCommand<Product>(SaveToCart);

        async Task Delete(Product product)
        {
            if (product == null)
                return;

            State = LayoutState.Loading;
            _ = await _repository.Delete("wishes", product.Id);
            _ = wishes.Remove(product);
            State = LayoutState.None;
        }

        async Task SaveToCart(Product product)
        {
            if (product == null)
                return;

            await _repository.CreateCart(product);
            _ = await _repository.Delete("wishes", product.Id);
            _ = Wishes.Remove(product);
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

            await DialogService.SnackBarAsync("Added to Cart", TimeSpan.FromSeconds(3), action);
        }

        async void Selected(Product product)
        {
            if (product == null)
                return;
            SelectedWish = null;
            await CoreMethods.PushPageModel<ItemPageModel>(product.Id,animate:false);
        }

        private Product selectedWish;
        public Product SelectedWish
        {
            get => selectedWish;
            set
            {
                SetProperty(ref selectedWish, value);
                Selected(SelectedWish);
            }
        }

        private ObservableCollection<Product> wishes;
        public ObservableCollection<Product> Wishes
        {
            get => wishes;
            set => SetProperty(ref wishes, value);
        }
    }
}
