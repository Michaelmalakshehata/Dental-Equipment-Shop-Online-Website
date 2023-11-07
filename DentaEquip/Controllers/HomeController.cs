using DentaEquip.BL.IRepositories;
using DentaEquip.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DentaEquip.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService homeService;

        public HomeController(IHomeService homeService)
        {
            this.homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            var offers = await homeService.GetTenExclusiveOffersProduct();
            if(offers is not null &&offers.Any())
            {
                ViewBag.ListProductDiscount = offers;
            }
           
            var newest=await homeService.GetTenNewestProduct();
            if(newest is not null &&newest.Any())
            {
                ViewBag.ListProductNew = newest;
            }
            var category=await homeService.GetAllMainAndSubCategory();
            if(category is not null && category.Any())
            {
                ViewBag.ListCategories = category;
            }
            var brand = await homeService.GetTenBrand();
            if(brand is not null &&brand.Any())
            {
                ViewBag.ListBrand = brand;
            }

            return View();
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }
        

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }

    }
}