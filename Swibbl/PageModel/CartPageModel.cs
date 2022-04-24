using Swibbl.Models.Shop;
using Swibbl.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Swibbl.Pages
{
    public class CartPageModel : BasePageModel
    {
        readonly IRepository<Product> _repository = DependencyService.Get<IRepository<Product>>();
        public CartPageModel()
        {
            MessagingCenter.Subscribe<SummaryPageModel>(this, "Completed", async (sender) =>
            {
                foreach (var p in Products)
                {
                    await _repository.Delete("cart", p.Id);
                }

                Products.Clear();
            });
            products = new ObservableCollection<Product>();
            moreItems = new ObservableCollection<Product>();
            Load();
        }

        async void Load()
        {
            State = LayoutState.Loading;
            await Task.Delay(1000);
            Products.Clear();
            Products = await _repository.GetCollectionbyGuestId();
            if (Products.Count != 0)
            {
                SumUp();
                MoreItems = await _repository.SortItems(category: Products.FirstOrDefault().Category);
                State = LayoutState.None;
            }
            else
                State = LayoutState.Empty;
        }

        void SumUp()
        {
            var amt = new List<int>();
            foreach (var p in Products)
            {
                if (p is Product ser)
                {
                    amt.Add(ser.Price * ser.Quantity);
                }
            }

            Total = amt.Sum();
            Savings = Total * Discount / 100;
            SubTotal = Total + ConvenienceFee;
            AllTotal = SubTotal - Savings;
        }
        public ICommand DecrementCommand => new Command<Product>(Decrement);
        public ICommand IncrementCommand => new Command<Product>(Increment);
        public ICommand DeleteCommand => new AsyncCommand<Product>(Delete);
        public ICommand AddCommand => new AsyncCommand<Product>(Add);
        public ICommand OrderCommand => new AsyncCommand(Order);

        async Task Add(Product product)
        {
            State = LayoutState.Loading;
            await _repository.CreateCart(product);
            products.Add(product);
            SumUp();
            State = LayoutState.None;
        }

        async Task Order()
        {
            if (AuthService.IsSignIn())
            {
                var cart = new Cart
                {
                    Products = products,
                    ShippingAddress = null,
                    OrderDate = DateTime.Now.ToLongDateString()
                };
                await CoreMethods.PushPageModel<AddressPageModel>(cart, animate: false);
            }
            else
            {
                await CoreMethods.PushPageModel<LoginDockPageModel>(animate: false);
            }
        }

        void Decrement(Product product)
        {
            if (product.Quantity > 1)
                product.Quantity--;
            SumUp();
        }

        async Task Delete(Product product)
        {
            State = LayoutState.Loading;
            _ = await _repository.Delete("cart", product.Id);
            _ = Products.Remove(product);

            SumUp();
            State = LayoutState.None;
        }

        void Increment(Product service)
        {
            service.Quantity++;
            SumUp();
        }

        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get => products;
            set => SetProperty(ref products, value);
        }

        private ObservableCollection<Product> moreItems;
        public ObservableCollection<Product> MoreItems
        {
            get => moreItems;
            set => SetProperty(ref moreItems, value);
        }

        int savings;
        public int Savings
        {
            get => savings;
            set => SetProperty(ref savings, value);
        }
        int subTotal;
        public int SubTotal
        {
            get => subTotal;
            set => SetProperty(ref subTotal, value);
        }

        int allTotal;
        public int AllTotal
        {
            get => allTotal;
            set => SetProperty(ref allTotal, value);
        }

        int total;
        public int Total
        {
            get => total;
            set => SetProperty(ref total, value);
        }

        int discount = 15;
        public int Discount
        {
            get => discount;
            set => SetProperty(ref discount, value);
        }

        int convenienceFee = 15;
        public int ConvenienceFee
        {
            get => convenienceFee;
            set => SetProperty(ref convenienceFee, value);
        }

    }
}
