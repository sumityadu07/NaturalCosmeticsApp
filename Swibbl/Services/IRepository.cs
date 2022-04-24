using Swibbl.Models;
using Swibbl.Models.Shop;
using Swibbl.Models.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Swibbl.Services
{
    public interface IIdentifiable
    {
        string Id { get; set; }
    }

    public interface IRepository<T> where T : IIdentifiable
    {
        #region User
        Task<ObservableCollection<T>> GetProductReviews(string productId,int limit);
        Task<ObservableCollection<T>> GetCollectionbyGuestId();
        Task<ObservableCollection<T>> GetCollectionbyUserId(string collection);
        Task<UserInfo> GetUserAsync();
        Task Save(string name, string email);
        Task SaveAddress(string address);
        void AddMail(string email);
        Task<bool> Delete(string collection, string item);
        #endregion
        Task CancelOrder(string orderid);
        Task<string> CreateCart(Product product);
        Task<string> Wishlist(Product product);
        Task SaveOrder(Product product, string shippingAddress, string orderDate,string orderid);
        Task SavePost(Post post);

        Task<ObservableCollection<T>> GetCollection(string collection);
        Task<ObservableCollection<T>> SortItems(string brand = null, string category = null,string subcategory = null, string last_doc_id = null);
        Task<ObservableCollection<T>> GetCollectionbyItemUid(string collection, string Uid, int limit);
        Task<T> GetDocument(string collection, string id);
        Task SaveReview(string productId, string cxname, string comment);
    }
}
