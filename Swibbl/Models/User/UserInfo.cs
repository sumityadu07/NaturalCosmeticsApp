using Swibbl.Services;

namespace Swibbl.Models.User
{
    public class UserInfo : IIdentifiable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
