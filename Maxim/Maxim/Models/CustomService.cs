using Maxim.Models.Common;

namespace Maxim.Models
{
    public class CustomService:BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
