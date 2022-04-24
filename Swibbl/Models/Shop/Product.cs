using Swibbl.Services;
using Xamarin.Forms;

namespace Swibbl.Models.Shop
{
    public class Product :BindableObject, IIdentifiable
    {
        public string Id { get; set; }//Name
        public string Brand { get; set; }
        public string Status { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string SubCategory { get; set; }
        public int Price { get; set; }
        public int DiscountedPrice { get { return Price - (Price * Discount / 100); } }
        public int Savings { get { return Price * Discount / 100; } }
        public int Stock { get; set; }
        public int Total{ get { return DiscountedPrice * Quantity; } }
        public string ImgUrl { get; set; }

        public int Discount { get; set; }

        int quantity = 1;
        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }
        public string Description { get; set; }
        public string Usage { get; set; }
    }
}
