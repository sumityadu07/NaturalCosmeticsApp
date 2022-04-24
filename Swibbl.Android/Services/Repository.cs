using Android.Gms.Extensions;
using Firebase;
using Firebase.Firestore;
using Java.Util;
using Swibbl.Droid;
using Swibbl.Droid.ServiceListeners;
using Swibbl.Models;
using Swibbl.Models.Marketing;
using Swibbl.Models.Shop;
using Swibbl.Models.User;
using Swibbl.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(Repository<UserInfo>))]
[assembly: Dependency(typeof(Repository<Address>))]
[assembly: Dependency(typeof(Repository<Order>))]
[assembly: Dependency(typeof(Repository<Ad>))]
[assembly: Dependency(typeof(Repository<Review>))]
[assembly: Dependency(typeof(Repository<Post>))]
[assembly: Dependency(typeof(Repository<Product>))]
//[assembly: Dependency(typeof(Repository<Cart>))]

namespace Swibbl.Droid
{
    public class Repository<T> : IRepository<T> where T : IIdentifiable
    {
        IAuthentication authService = DependencyService.Resolve<IAuthentication>();

        public string CurrentUid;

        FirebaseFirestore DataStore;
        DocumentReference docRef;

        public Repository()
        {
            DataStore = FirebaseFirestore.Instance;
            if (authService.IsSignIn())
            {
                CurrentUid = authService.CurrentUid();
                docRef = DataStore.Collection("users").Document(CurrentUid);
            }
        }

        #region User
        public async Task Save(string name,string email)
        {
            var phone = authService.Phone();
            var userData = new HashMap();
            userData.Put("phone", phone);
            userData.Put("name", name);
            userData.Put("email", email);
            await docRef.Set(userData);
        }
        public async Task SaveAddress(string address)
        {
            var docref = DataStore.Collection("addresses").Document();
            var userData = new HashMap();
            userData.Put("userId", CurrentUid);
            userData.Put("location", address);
            await docref.Set(userData);
        }
        public void AddMail(string email)
        {
            throw new NotImplementedException();
        }
        public Task<bool> Delete(string collection, string item)
        {
            var tcs = new TaskCompletionSource<bool>();
            DataStore.Collection(collection).Document(item).Delete().AddOnCompleteListener(new OnDeleteCompleteListener(tcs));
            return tcs.Task;
        }
        public Task<UserInfo> GetUserAsync()
        {
            var tcs = new TaskCompletionSource<UserInfo>();
            docRef.Get().AddOnCompleteListener(new OnCompleteListener(tcs));
            return tcs.Task;
        }
        public Task<ObservableCollection<T>> GetCollectionbyUserId(string collection)
        {
            var tcs = new TaskCompletionSource<ObservableCollection<T>>();
            DataStore.Collection(collection).WhereEqualTo("userId", CurrentUid).Get()
                    .AddOnCompleteListener(new OnCollectionCompleteListener<T>(tcs));
            return tcs.Task;
        }
        public Task<ObservableCollection<T>> GetProductReviews(string productId ,int limit)
        {
            var tcs = new TaskCompletionSource<ObservableCollection<T>>();
            DataStore.Collection("orders").WhereNotEqualTo("comment", false)
                     .WhereEqualTo("productId", productId).Limit(limit).Get()
                     .AddOnCompleteListener(new OnCollectionCompleteListener<T>(tcs));
            return tcs.Task;
        }

        public Task<ObservableCollection<T>> GetCollectionbyGuestId()
        {
            var tcs = new TaskCompletionSource<ObservableCollection<T>>();
            var guestid = Preferences.Get("guestId", "nil");

            DataStore.Collection("cart").WhereEqualTo("guestId", guestid).Get()
                    .AddOnCompleteListener(new OnCollectionCompleteListener<T>(tcs));
            return tcs.Task;
        }
        #endregion

        #region BasicDataCall for ItemsInCart,Items,Services
        public Task<ObservableCollection<T>> GetCollection(string collection)
        {
            var tcs = new TaskCompletionSource<ObservableCollection<T>>();
            DataStore.Collection(collection).Get()
                    .AddOnCompleteListener(new OnCollectionCompleteListener<T>(tcs));
            return tcs.Task;
        }

        public Task<ObservableCollection<T>> SortItems(string brand,  string category, string subcategory, string lastloadeddoc)
        {
            var collection = DataStore.Collection("products");

            Query query = collection.OrderBy("name").StartAfter(lastloadeddoc).Limit(10);
            
            var tcs = new TaskCompletionSource<ObservableCollection<T>>();

            if(subcategory != null)
                query = collection.WhereEqualTo("subcategory", subcategory);
            if (category != null)
                query = collection.WhereEqualTo("category", category);
            if (brand != null)
                query = collection.WhereEqualTo("brand", brand);

            query.Get()
                 .AddOnCompleteListener(new OnCollectionCompleteListener<T>(tcs));
            
            return tcs.Task;
        }
        public Task<ObservableCollection<T>> GetCollectionbyItemUid(string collection, string Uid, int limit)
        {
            var tcs = new TaskCompletionSource<ObservableCollection<T>>();
            DataStore.Collection(collection)
                .WhereEqualTo("productId", Uid)
                .OrderBy("id")
                .Limit(limit)
                .Get().AddOnCompleteListener(new OnCollectionCompleteListener<T>(tcs));
            return tcs.Task;
        }
        public Task<T> GetDocument(string collection, string id)
        {
            var tcs = new TaskCompletionSource<T>();
            DataStore.Collection(collection)
                        .Document(id)
                        .Get().AddOnCompleteListener(new OnDocumentCompleteListener<T>(tcs));
            return tcs.Task;
        }
        #endregion

        public async Task<string> CreateCart(Product product)
        {
            var tcs = new TaskCompletionSource<string>();
            var map = new HashMap();
            var guestid = Preferences.Get("guestId", "nil");
            map.Put("guestId", guestid);
            map.Put("price", product.Price);
            map.Put("name", product.Name);
            map.Put("imgUrl", product.ImgUrl);
            map.Put("discount", product.Discount);
            map.Put("quantity", 1);
            await DataStore.Collection("cart")
                                     .Add(map)
                                     .AddOnCompleteListener(new OnCreateCompleteListener(tcs));
            return await tcs.Task;
        }
        public async Task<string> Wishlist(Product product)
        {
            var tcs = new TaskCompletionSource<string>();
            var map = new HashMap();
            map.Put("userId", CurrentUid);
            map.Put("price", product.Price);
            map.Put("imgUrl", product.ImgUrl);
            map.Put("discount", product.Discount);
            map.Put("name", product.Name);
            map.Put("productId", product.Id);
            await DataStore.Collection("wishes")
                                     .Add(map)
                                     .AddOnCompleteListener(new OnCreateCompleteListener(tcs));
            return await tcs.Task;
        }
        public async Task CancelOrder(string orderid)
        {
            var doc = DataStore.Collection("orders").Document(orderid);
            var dictionay = new Dictionary<string, Java.Lang.Object>()
            {
                { "status", "Cancelled" },
                { "activityDate", DateTime.Now.ToLongDateString()},
            };
            await doc.Update(dictionay);
        }
        public async Task SaveReview(string orderId, string cxname, string comment)
        {
            var doc = DataStore.Collection("orders")
                            .Document(orderId);
            var dictionay = new Dictionary<string, Java.Lang.Object>()
            {
                { "reviewer", cxname},
                { "comment", comment},
            };
            await doc.Update(dictionay);
        }
        public async Task SavePost(Post post)
        {
            var doc = DataStore.Collection("savedposts")
                            .Document();
            var map = new HashMap();
            map.Put("imgurl", post.Imgurl);
            map.Put("userId", post.Title);
            map.Put("reviewer", post.Description);
            await doc.Set(map);
        }
        public async Task SaveOrder(Product product,string shippingAddress,string orderDate,string orderid)
        {
            var docref = DataStore.Collection("orders").Document();
            var map = new HashMap();
            map.Put("userId", CurrentUid);
            map.Put("productId", product.Id);
            map.Put("price", product.DiscountedPrice);
            map.Put("orderTotal", product.Total);
            map.Put("name", product.Name);
            map.Put("imgUrl", product.ImgUrl);
            map.Put("orderId", orderid);
            map.Put("shippingAddress", shippingAddress);
            map.Put("orderDate", orderDate);
            map.Put("status", "Ordered");
            map.Put("isDelivered", false);
            map.Put("quantity", product.Quantity);
            await docref.Set(map);

        }
    }
}
