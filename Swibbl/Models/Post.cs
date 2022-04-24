using Swibbl.Services;

namespace Swibbl.Models
{
    public class Post : IIdentifiable
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Imgurl { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get { return Description.Substring(0, 84); } }
    }
}
