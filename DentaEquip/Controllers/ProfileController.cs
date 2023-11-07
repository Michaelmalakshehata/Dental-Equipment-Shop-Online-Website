using AspNetCoreHero.ToastNotification.Abstractions;
using DentaEquip.BL.IRepositories;
using DentaEquip.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DentaEquip.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly INotyfService notyf;
        private readonly IServiceCart serviceCart;
        private readonly IServiceWishList serviceWishList;
        private readonly SignInManager<ApplicationUser> signInManager;

        public ProfileController(SignInManager<ApplicationUser> signInManager, IServiceWishList serviceWishList, IServiceCart serviceCart, INotyfService notyf, UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.notyf = notyf;
            this.serviceCart = serviceCart;
            this.serviceWishList = serviceWishList;
            this.signInManager=signInManager;
        }
        public async Task<IActionResult> Index()
        {
            string name = User.Identity.Name;
            ApplicationUser user = await userManager.FindByNameAsync(name);
            if (user is not null)
            {
                return View(user);
            }

            return View();
        }
        [HttpGet]
        public async Task<IActionResult>DeleteAccount(string applicationUser)
        {
            if(applicationUser is not null && ! applicationUser.Equals("AdminDenta"))
            {
                
                ApplicationUser user = await userManager.FindByNameAsync(applicationUser);
                if (user is not null)
                {
                    var result = await userManager.DeleteAsync(user);
                    if(result.Succeeded == true)
                    {
                        await serviceWishList.DeleteAllWishListItems(applicationUser);
                        await serviceCart.DeleteAllCartItems(applicationUser);
                        await signInManager.SignOutAsync();
                        notyf.Success("Delete Account Done", 10);
                        return RedirectToAction("Registration", "Account");
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}
