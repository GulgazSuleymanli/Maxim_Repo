using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace Maxim.Areas.Manage.ViewModels.Services
{
    public class CreateServiceVM
    {
        public string Title { get; set; }
        [MinLength(25)]
        [MaxLength(100)]
        public string Description { get; set; }
        
        public IFormFile Image { get; set; }
    }
}
