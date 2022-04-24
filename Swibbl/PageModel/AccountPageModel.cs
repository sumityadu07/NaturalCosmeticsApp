using Swibbl.Models.User;
using Swibbl.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Swibbl.Pages
{
    public class AccountPageModel : LoginBaseModel
    {
        IRepository<UserInfo> Repository = DependencyService.Resolve<IRepository<UserInfo>>();

        private bool isAUpdate;
        private string newPhone;
        private string name;
        private string email;

        public AccountPageModel()
        {
            Phone = AuthService.Phone();
            Name = Preferences.Get("name", "nil");
            Email = Preferences.Get("email", "nil");
        }

        public ICommand SaveCommand => new AsyncCommand(OnSave);
        public ICommand OpenTabCommand => new Command(() => IsAUpdate = true);
        public ICommand CloseTabCommand => new Command(() => IsAUpdate = false);

        async Task OnSave()
        {
            await Repository.Save(Name,Email);
            await Task.Delay(2000);
            await CoreMethods.PopPageModel("load");
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public bool IsAUpdate
        {
            get => isAUpdate;
            set => SetProperty(ref isAUpdate, value);
        }

        public string NewPhone
        {
            get => newPhone;
            set => SetProperty(ref newPhone, value);
        }
    }
}
