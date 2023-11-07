using AspNetCoreHero.ToastNotification.Abstractions;
using DentaEquip.BL.IRepositories;
using DentaEquip.BL.Pagination;
using DentaEquip.BL.ViewModels.Cart;
using DentaEquip.DAL.Constant;
using DentaEquip.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentaEquip.Controllers
{
    [Authorize(Roles = Roles.userrole)]
    public class WishListController : Controller
    {
        private readonly IServiceWishList servicewishlist;
        private readonly IServiceFilterProduct serviceFilterProduct;
        private readonly INotyfService notyf;
        public WishListController(INotyfService notyf, IServiceFilterProduct serviceFilterProduct, IServiceWishList servicewishlist)
        {
            this.servicewishlist = servicewishlist;
            this.serviceFilterProduct = serviceFilterProduct;
            this.notyf = notyf;
        }
        public async Task<IActionResult> Index(int pg = 1)
        {
            string name = User.Identity.Name;
            if (string.IsNullOrWhiteSpace(name) == false)
            {
                var wishlist = await servicewishlist.GetAllUserWishList(name);
                var data = Pagination<CartViewModel>.GetPaginationData(pg, wishlist,10);
               
                this.ViewBag.Pager = data.Item2;
                ViewBag.list = data.Item1;
              if(ViewBag.list is not null && ViewBag.list.Count>0)
                {
                    ViewBag.Related = await serviceFilterProduct.GeTenRelatedProductMainCategory(ViewBag.list[0].ProductId);
                }

                ViewBag.listcount = wishlist.Count;
                return View();
            }
            else
            {
                ViewBag.listcount = 0;
                return View();
            }

        }

        [HttpGet]
        
        public async Task<IActionResult> AddToWishList(int Id)
        {

            string name = User.Identity.Name;

            if (string.IsNullOrWhiteSpace(name) == true || Id == 0)
            {
                return RedirectToAction("Index", "ShowProduct");
                
            }
            string result = await servicewishlist.AddToWishList(Id, name);
            if (string.IsNullOrWhiteSpace(result) == true)
            {
                return RedirectToAction("Index", "ShowProduct");            
            }
            if(result.Equals("ReachmaximumWishlistItem"))
            {
                notyf.Success("Reached Maximum WishList Items", 10);
                return RedirectToAction("Index", "ShowProduct");
            }
            notyf.Success("Add To Wishlist Done", 10);
            return RedirectToAction("Index", "WishList");
        }


        [HttpGet]
        public async Task<IActionResult> DeleteWishListItem(int Id)
        {
            string name = User.Identity.Name;
            if (string.IsNullOrWhiteSpace(name) == false && Id > 0)
            {
               string result= await servicewishlist.DeleteWishListItem(Id, name);
                if(string.IsNullOrWhiteSpace(result) == false)
                {
                    notyf.Success("Delete Wishlist Item Done", 10);
                }
            }
            return RedirectToAction("Index", "WishList");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteAllWishListItems()
        {
            string name = User.Identity.Name;
            if (string.IsNullOrWhiteSpace(name) == false)
            {
               string result= await servicewishlist.DeleteAllWishListItems(name);
                if(string.IsNullOrWhiteSpace(result) == false)
                {
                    notyf.Success("Delete All Wishlist Items Done", 10);
                }
            }
            return RedirectToAction("Index", "WishList");
        }
    }
}
