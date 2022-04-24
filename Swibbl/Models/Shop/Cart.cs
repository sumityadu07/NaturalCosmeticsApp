using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Swibbl.Models.Shop
{
    public class Cart
    {
        public ObservableCollection<Product> Products
        {
            get;
            set;
        }

        public string ShippingAddress { get; set; }
        public string OrderDate { get; set; }
    }
}
