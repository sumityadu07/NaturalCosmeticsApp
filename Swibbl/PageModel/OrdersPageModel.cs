using Swibbl.Models.Shop;
using Swibbl.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Swibbl.Pages
{
    class OrdersPageModel : BasePageModel
    {
        IRepository<Order> repository = DependencyService.Get<IRepository<Order>>();

        public OrdersPageModel()
        {
            products = new ObservableCollection<Order>();
            Load();
        }
        public ICommand ViewOrderCommand => new AsyncCommand<Order>(ViewOrder);

        async void Load()
        {
            State = LayoutState.Loading;
            await Task.Delay(1000);
            products.Clear();
            products = await repository.GetCollectionbyUserId("orders");
            if (Products.Count != 0)
            {
                State = LayoutState.None;
            }
            else
            {
                State = LayoutState.Empty;
            }
        }

        async Task ViewOrder(Order order) => await CoreMethods.PushPageModel<OrderPageModel>(order, false);

        private ObservableCollection<Order> products;
        public ObservableCollection<Order> Products
        {
            get => products;
            set => SetProperty(ref products, value);
        }
    }
}
