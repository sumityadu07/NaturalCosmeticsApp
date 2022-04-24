using Swibbl.Services;

namespace Swibbl.Models.User
{
    public class Address : IIdentifiable
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Location { get; set; }
    }
}
