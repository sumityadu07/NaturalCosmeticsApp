using Swibbl.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Swibbl.Pages
{
    public class ReviewsPageModel : ItemPageModel
    {
        public ReviewsPageModel()
        {
            reviews = new ObservableCollection<Review>();
            Load();
        }

        async void Load()
        {
            Reviews.Clear();
            Reviews = await reviewrepository.GetCollectionbyItemUid("products", Product.Id, 20);
        }
    }
}
