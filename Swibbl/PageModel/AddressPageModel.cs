using Swibbl.Models.Shop;
using Swibbl.Models.User;
using Swibbl.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Swibbl.Pages
{
    public class AddressPageModel : BasePageModel
    {
        IRepository<Address> _repository = DependencyService.Get<IRepository<Address>>();
        public Cart Cart { get; set; }
        
        public AddressPageModel()
        {
            addresses = new ObservableCollection<Address>();
            GetLocations();
        }

        public override void Init(object initData)
        {
            Cart = initData as Cart;
        }

        async void GetLocations()
        {
            State = LayoutState.Loading;
            if (AuthService.IsSignIn())
                Addresses = await _repository.GetCollectionbyUserId("addresses");
            State = LayoutState.None;
        }

        async void AddAddress(string address) => await _repository.SaveAddress(address);
        public ICommand NewAddressCommand => new AsyncCommand(GoToGeolocationPage);
        public ICommand DltCommand => new AsyncCommand<Address>(DltAddress);
        public ICommand DoneCommand => new AsyncCommand(Done);

        async Task DltAddress(Address address)
        {
            if (address == null)
                return;

            _ = await _repository.Delete("addresses", address.Id);
            _ = Addresses.Remove(address);
        }

        async Task Done()
        {
            if (SelectedAddress != null)
            {
                Cart.ShippingAddress = SelectedAddress.Location;
                await CoreMethods.PushPageModel<SummaryPageModel>(Cart, animate: false);
            }
            else
                await DialogService.ToastAsync("Please select a Shipping Address");
        }

        public override void ReverseInit(object returnedData)
        {
            AddAddress(returnedData as string);
            GetLocations();
        }

        async Task GoToGeolocationPage()
        {
            if (AuthService.IsSignIn())
                await CoreMethods.PushPageModel<GeoLocationPageModel>(AuthService.Phone(), animate: false);
            else
                await DialogService.ToastAsync("Sign in to Continue");
        }

        private ObservableCollection<Address> addresses;
        public ObservableCollection<Address> Addresses
        {
            get { return addresses; }
            set => SetProperty(ref addresses, value);
        }

        private Address selectedAddress;
        public Address SelectedAddress
        {
            get => selectedAddress;
            set
            {
                SetProperty(ref selectedAddress, value);
            }
        }
    }
}
