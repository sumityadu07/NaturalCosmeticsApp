
namespace Swibbl.Models
{
    public class Review : Services.IIdentifiable
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string Reviewer { get; set; }
        public string Comment { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
    }
}
