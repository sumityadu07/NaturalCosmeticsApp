using Android.App;
using Plugin.FirebasePushNotification;
using Swibbl.Models;
using Swibbl.Models.Shop;
using Swibbl.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Swibbl.Pages
{
    public class SummaryPageModel : BasePageModel
    {
        IPayment _repository = DependencyService.Get<IPayment>();
        IRepository<Order> repository = DependencyService.Get<IRepository<Order>>();
        string orderid = Guid.NewGuid().ToString().Substring(0, 28).ToUpper();

        public override void Init(object initData)
        {
            MessagingCenter.Subscribe<Activity, LayoutState>(this, "PaymentMethods", async (sender,arg) =>
            {
                State = arg;
                if(State == LayoutState.Success)
                {
                    foreach(var p in Cart.Products)
                    {
                        await repository.SaveOrder(p,Cart.ShippingAddress,Cart.OrderDate,orderid);
                    }
                    await CoreMethods.PushPageModel<ConfirmPageModel>();
                }
            });
            Cart = initData as Cart;
        }

        public int Total
        {
            get
            {
                var total = new List<int>();
                foreach (var p in Cart.Products)
                {
                    total.Add(p.Total);
                }
                return total.Sum();
            }
        }

        public Cart Cart
        { 
            get;
            set;
        }

        public ICommand OrderCommand => new Command(MakePayment);
        public ICommand ContinueCommand => new AsyncCommand(async () => await CoreMethods.PushPageModelWithNewNavigation<ListPageModel>(true));
        public ICommand EditOrderCommand => new AsyncCommand(async () => await CoreMethods.PopPageModel());

        void MakePayment()
        {
            if (Cart != null) 
            {
                var upi = new Upi
                {
                    Am = Total.ToString(),
                    Apk = "com.google.android.apps.nbu.paisa.user"
                };

                _repository.Pay(upi.Am,upi.Apk);
            }
        }

    }
}

