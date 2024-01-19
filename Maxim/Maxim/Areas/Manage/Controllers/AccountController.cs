using Maxim.Areas.Manage.ViewModels.Account;
using Maxim.DAL;
using Maxim.Models;
using Maxim.Utilities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maxim.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class AccountController : Controller
    {
        private readonly AppDbContext _contex;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(AppDbContext contex, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _contex = contex;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVm)
        {
            if (!ModelState.IsValid) return View();

            if(await _contex.Users.AnyAsync(u => u.UserName == registerVm.Username))
            {
                ModelState.AddModelError("Username", "Username is already in use");
                return View();
            }

            AppUser user = new AppUser()
            {
                Name = registerVm.Name,
                Surname = registerVm.Surname,
                Email = registerVm.Email,
                UserName = registerVm.Username,
            };

            IdentityResult result = await _userManager.CreateAsync(user,registerVm.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            await _contex.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Home", new { area = "" });
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if(!ModelState.IsValid) return View();

            AppUser user = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);

            if(user is null)
            {
                user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
                if(user is null)
                {
                    ModelState.AddModelError("", "Wrong username or password");
                }
            }

            var signCheck = _signInManager.CheckPasswordSignInAsync(user, loginVM.Password, false).Result;

            if (!signCheck.Succeeded)
            {
                ModelState.AddModelError("", "Wrong username or password");
            }

            await _signInManager.SignInAsync(user,false);
            return RedirectToAction(nameof(Index), "Home", new {area=""});
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home", new { area = "" });
        }

        public async Task<IActionResult> CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(UserRole)))
            {
                if(!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole()
                    {
                        Name = item.ToString(),
                    });
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
