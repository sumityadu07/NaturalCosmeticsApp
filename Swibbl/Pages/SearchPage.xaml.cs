using Swibbl.Helpers;
using Swibbl.Models.Shop;
using Swibbl.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Swibbl.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }
        //IRepository<Product> _repository = DependencyService.Get<IRepository<Product>>();

        //public SearchPage()
        //{
        //    InitializeComponent();
        //    items = new ObservableCollection<Product>();
        //    LoadItems().Await();
        //}
        //async Task LoadItems()
        //{
        //    Items.Clear();
        //    Items = await _repository.GetCollection("products");
        //}
        //private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    var keyword = SearchContent.Text;
        //    if (keyword.Length >= 1)
        //    {
        //        var suggestion = items.Where(c => c.Id.ToLower().Contains(keyword.ToLower()));
        //        ItemsList.ItemsSource = suggestion;
        //        ItemsList.IsVisible = true;
        //    }
        //    else
        //    {
        //        ItemsList.IsVisible = false;
        //    }
        //}

        //private void ItemList_ItemTapped(object sender, ItemTappedEventArgs e)
        //{
        //    if (e.Item as string == null)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        ItemsList.ItemsSource = items.Where(c => c.Equals(e.Item as string));
        //        ItemsList.IsVisible = true;
        //        SearchContent.Text = e.Item as string;
        //    }

        //}
        //private ObservableCollection<Product> items;
        //public ObservableCollection<Product> Items
        //{
        //    get { return items; }
        //    set => OnPropertyChanged("Items");
        //}
    }
}