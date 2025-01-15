using AgencyMVC.Models;
using AgencyMVC.Utilities.Enums;
using AgencyMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AgencyMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> usermeneger;
        private readonly SignInManager<AppUser> signuser;
        private readonly RoleManager<IdentityRole> userRole;

        public AccountController(UserManager<AppUser> usermeneger,SignInManager<AppUser> signuser,RoleManager<IdentityRole> userRole)
        {
            this.usermeneger = usermeneger;
            this.signuser = signuser;
            this.userRole = userRole;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM uservm)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = new()
            {
                UserName = uservm.UserName,
                Email = uservm.Email,
                Name = uservm.Name,
                Surname = uservm.Surname,
            };

            var result=await usermeneger.CreateAsync(user,uservm.Password);
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();  
            }
            await usermeneger.AddToRoleAsync(user,UserRoles.Member.ToString());
            await signuser.SignInAsync(user,false);
            return RedirectToAction(nameof(HomeController.Index), "Home");  
        }

         public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM uservm,string? returnurl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user=await usermeneger.Users.FirstOrDefaultAsync(u=>u.UserName==uservm.UserorEmail || u.Email==uservm.UserorEmail);
            if(user is null)
            {
                ModelState.AddModelError(string.Empty, "UserName/Email or Password is incorrect");
                return View();
            }

            var result = await signuser.PasswordSignInAsync(user, uservm.Password, false, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "UserName/Email or Password is incorrect");
                return View();
            }
            if(result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Your Account has been blocked!");
                return View();
            }

            if(returnurl is null)
            {
                return RedirectToAction(nameof(HomeController.Index),"Home");
            }

            return Redirect(returnurl);
        }

        public async Task<IActionResult> Logout()
        {
            await signuser.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> CreateRoles()
        {
            foreach(var roles in Enum.GetValues(typeof(UserRoles)))
            {
                if(!await userRole.RoleExistsAsync(roles.ToString()))
                {
                    await userRole.CreateAsync(new IdentityRole { Name= roles.ToString() });    
                }
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
