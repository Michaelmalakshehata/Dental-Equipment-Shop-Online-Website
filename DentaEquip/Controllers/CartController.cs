using AspNetCoreHero.ToastNotification.Abstractions;
using DentaEquip.BL.IRepositories;
using DentaEquip.BL.Pagination;
using DentaEquip.BL.ViewModels.Cart;
using DentaEquip.DAL.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentaEquip.Controllers
{
    [Authorize(Roles = Roles.userrole)]
    public class CartController : Controller
    {
        private readonly IServiceCart serviceCart;
        private readonly INotyfService notyf;
        private readonly IServiceFilterProduct serviceFilterProduct;
        public CartController(IServiceFilterProduct serviceFilterProduct, INotyfService notyf, IServiceCart serviceCart)
        {
            this.serviceCart = serviceCart;
            this.notyf = notyf;
            this.serviceFilterProduct = serviceFilterProduct;
        }
        public async Task<IActionResult> Index(int pg = 1)
        {
            string name = User.Identity.Name;
            var carts = await serviceCart.GetAllUserCart(name);
            ViewBag.list = carts;
            decimal? totalprice = await serviceCart.totalprice(name);
            ViewBag.price = totalprice;
           if(ViewBag.list is not null && ViewBag.list.Count>0)
            {
                ViewBag.Related = await serviceFilterProduct.GeTenRelatedProductMainCategory(ViewBag.list[0].ProductId);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(CartViewModel cartViewModel)
        {
            if (ModelState.IsValid)
            {
                string name = User.Identity.Name;

                if (string.IsNullOrWhiteSpace(name) == true || cartViewModel is  null)
                {
                    return RedirectToAction("Index", "ShowProduct");
                }
                string result = await serviceCart.AddToCart(cartViewModel, name);
                if (result.Equals("ReachMaximumCartItem"))
                {
                    notyf.Success("Reached Maximum Cart Items", 10);
                    return RedirectToAction("Index", "ShowProduct");
                    
                }
                notyf.Success("Add To Cart Done", 10);

                return RedirectToAction("Index", "Cart");
            }

            return RedirectToAction("Index", "ShowProduct");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCart(CartUpdateViewModel cartUpdateViewModel)
        {
            if (ModelState.IsValid)
            {
               string result= await serviceCart.UpdateCartItem(cartUpdateViewModel);
                if(string.IsNullOrWhiteSpace(result) == false)
                {
                    notyf.Success("Update Cart Item Done", 10);
                }
            }
            return RedirectToAction("Index", "Cart");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCartItem(int Id)
        {
            string name = User.Identity.Name;
            if (Id != 0 && string.IsNullOrWhiteSpace(name) == false)
            {
                string result=await serviceCart.DeleteCartItem(Id, name);
                if (string.IsNullOrWhiteSpace(result) == false)
                {
                    notyf.Success("Delete Cart Item Done", 10);
                }
            }
            return RedirectToAction("Index", "Cart");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteAllCartItems()
        {
            string name = User.Identity.Name;
            if (string.IsNullOrWhiteSpace(name) == false)
            {
               string result= await serviceCart.DeleteAllCartItems(name);
                if (string.IsNullOrWhiteSpace(result) == false)
                {
                    notyf.Success("Delete All Cart Items Done", 10);
                }
            }
            return RedirectToAction("Index", "Cart");
        }
    }
}
