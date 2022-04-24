
using Swibbl.Services;
using System;

namespace Swibbl.Models.Shop
{
    public class Order : IIdentifiable
    {
        public string Imgurl { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int OrderTotal { get; set; }
        public string UserId { get; set; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string OrderDate { get; set; }
        public string ActivityDate { get; set; }
        public string Comment { get; set; }
        public string CxName { get; set; }
        public string Refund { get; set; }
        public bool IsDelivered { get; set; }
        public string Status { get; set; }
        public string Id { get; set; }
    }
}
