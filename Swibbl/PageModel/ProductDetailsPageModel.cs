using Swibbl.Models.Shop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swibbl.Pages
{
    public class ProductDetailsPageModel : FreshMvvm.FreshBasePageModel
    {
        public Product Product { get; set; }
        
        public override void Init(object initData)
        {
            Product = initData as Product;
        }
    }
}
