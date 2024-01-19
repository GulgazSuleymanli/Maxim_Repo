using Maxim.Areas.Manage.ViewModels.Services;
using Maxim.DAL;
using Maxim.Models;
using Maxim.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace Maxim.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ServiceController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<CustomService> services = await _context.CustomServices.ToListAsync();
            return View(services);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateServiceVM createServiceVM)
        {
            if (!ModelState.IsValid) return View();

            if (createServiceVM.Image.CheckLength(200))
            {
                if (!createServiceVM.Image.CheckType("/image"))
                {
                    ModelState.AddModelError("Image", "Wrong image type");
                }
            }
            else
            {
                ModelState.AddModelError("Image", "Wrong image length");
            }

            CustomService service = new CustomService()
            {
                Title = createServiceVM.Title,
                Description = createServiceVM.Description,
                ImageUrl = createServiceVM.Image.CreateFile(_env.WebRootPath, "Uploads/ServicesImages"),
                IsDeleted = false
            };

            await _context.CustomServices.AddAsync(service);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Service");
        }


        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();

            CustomService service = await _context.CustomServices.FirstOrDefaultAsync(s => s.Id == id);
            if (service == null) return NotFound();

            UpdateServiceVM updateService = new UpdateServiceVM()
            {
                Title = service.Title,
                Description = service.Description,
            };

            return View(updateService);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,UpdateServiceVM updateServiceVM)
        {
            if (id <= 0) return BadRequest();
            if (!ModelState.IsValid) return View();

            CustomService service = await _context.CustomServices.FirstOrDefaultAsync(s => s.Id == id);

            if (service == null) return NotFound();

            if(updateServiceVM.Title != null)
            {
                service.Title = updateServiceVM.Title;
            }

            if (updateServiceVM.Description != null)
            {
                service.Description = updateServiceVM.Description;
            }

            if (updateServiceVM.Image != null)
            {
                service.ImageUrl.DeleteFile(_env.WebRootPath, "Uploads/ServicesImages");

                service.ImageUrl = updateServiceVM.Image.CreateFile(_env.WebRootPath, "Uploads/ServicesImages");
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Service");
        }


        public async Task<IActionResult> Delete(int id)
        {
            if(id <= 0) return BadRequest();

            CustomService service = await _context.CustomServices.FirstOrDefaultAsync(s => s.Id == id);

            if(service == null) return NotFound();

            service.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Service");
        }
    }
}
