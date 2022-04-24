using Swibbl.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace Swibbl.Pages
{
    public class GeoLocationPageModel : BasePageModel
    {
        public override void Init(object initData)
        {
            Phone = initData as string;
        }

        public ICommand SaveAddressCommand => new AsyncCommand(SaveAddress);
        public ICommand DetectCommand => new AsyncCommand(GetCurrentLocation);

        async Task GetCurrentLocation()
        {
            var location = await Geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30)
                });

            }
            if (location != null || !location.IsFromMockProvider)
            {
                var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                Placemark placemark = placemarks.FirstOrDefault();
                var address = placemark.SubLocality + "," + placemark.Locality + "," + placemark.AdminArea + "," + placemark.PostalCode + "," + placemark.CountryName;
                Location = address;
            }
            else
            {
                await DialogService.ToastAsync("Loaction is not valid");
            }
        }

        async Task SaveAddress()
        {
            var address =  Details + " " + Gender + " " + Name + " " + Saveas + " " + Location + " " + Phone;
            await CoreMethods.PopPageModel(address);
        }
        private string phone;
        public string Phone
        {
            get => phone;
            set => SetProperty(ref phone, value);
        }
        private string location;
        public string Location
        {
            get => location;
            set => SetProperty(ref location, value);
        }
        private string details;
        public string Details
        {
            get => details;
            set => SetProperty(ref details, value);
        }
        private string gender;
        public string Gender
        {
            get => gender;
            set => SetProperty(ref gender, value);
        }

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string saveas;
        public string Saveas
        {
            get => saveas;
            set => SetProperty(ref saveas, value);
        }
    }
}
