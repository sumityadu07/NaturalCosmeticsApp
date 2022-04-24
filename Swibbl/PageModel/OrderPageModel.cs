using Swibbl.Models;
using Swibbl.Models.Shop;
using Swibbl.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Swibbl.Pages
{
    public class OrderPageModel : BasePageModel
    {
        IRepository<Order> _repository = DependencyService.Get<IRepository<Order>>();

        public OrderPageModel()
        {
    
        }
        public override void Init(object initData)
        {
            Order = initData as Order;
            Load();
        }

        void Load()
        {
            switch (Order.Status)
            {
                case "Cancelled":
                    StatusColor = Color.FromHex("#b91372");
                    IsVisible = true;
                    break;
                case "Refund Initiated":
                    StatusColor = Color.GreenYellow;
                    break;
                case "Refunded Successfully":
                    StatusColor = Color.Green;
                    break;
                case "Ordered":
                    StatusColor = Color.Lime;
                    IsBusy = true;//Cancel Button
                    break;
                case "Shipped":
                    StatusColor = Color.YellowGreen;
                    IsBusy = true;//Cancel Button
                    break;
                case "Out for Delivery":
                    IsBusy = true;//Cancel Button
                    StatusColor = Color.LimeGreen;
                    break;

                default:
                    StatusColor = Color.Blue;
                    break;
            }

        }
        public ICommand CancelCommand => new AsyncCommand(CancelOrder);
        async Task CancelOrder()
        {
            await DialogService.AlertAsync("These Cannot be Undone", "Cancellation", "Ok");
            await _repository.CancelOrder(Order.Id);
            Order.Status = "Cancelled";
            IsBusy = false;
            Load();
        }

        bool isTabVisible = false;
        public bool IsTabVisible
        {
            get => isTabVisible;
            set => SetProperty(ref isTabVisible, value);
        }

        Order order;
        public Order Order
        {
            get => order;
            set => SetProperty(ref order, value);
        }

        private Color statusColor;
        public Color StatusColor
        {
            get => statusColor;
            set => SetProperty(ref statusColor, value);
        }

        string comment;
        public string Comment
        { 
            get => comment;
            set => SetProperty(ref comment,value);
        }
    }
}
