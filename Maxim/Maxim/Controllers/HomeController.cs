using Maxim.DAL;
using Maxim.Models;
using Maxim.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maxim.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<CustomService> customServices = await _context.CustomServices.Where(cs => cs.IsDeleted == false).ToListAsync();
            if(customServices is null) return NotFound();

            HomeVM homeVM = new HomeVM()
            {
                CustomServices = customServices
            };

            return View(homeVM);
        }
    }
}
