using AspNetCoreHero.ToastNotification.Abstractions;
using DentaEquip.BL.IRepositories;
using DentaEquip.BL.Pagination;
using DentaEquip.BL.Search;
using DentaEquip.BL.ViewModels.Brand;
using DentaEquip.DAL.Constant;
using DentaEquip.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentaEquip.Controllers
{
    [Authorize(Roles = Roles.adminrole)]

    public class BrandController : Controller
    {
        private readonly IServiceBrand serviceBrand;
        private readonly IGenericServiceSoftDelete<Brand> GenericBrandService;
        private readonly INotyfService notyf;
        static List<Brand> AllBrandList;
        static List<Brand> DeletedBrandList;


        public BrandController(IServiceBrand serviceBrand, INotyfService notyf,IGenericServiceSoftDelete<Brand> genericServiceSoftDelete)
        {
            this.serviceBrand = serviceBrand;
            this.notyf = notyf;
            GenericBrandService = genericServiceSoftDelete;
        }
        #region all Brands
        public async Task<IActionResult> Index(string clear,string sort,string search, int pg = 1)
        {
            if(string.IsNullOrWhiteSpace(clear) == false)
            {
                AllBrandList = null;
            }
            if(AllBrandList is null)
            {
               AllBrandList = await serviceBrand.GetallBrands();
            }
            if (string.IsNullOrWhiteSpace(search) == false || string.IsNullOrWhiteSpace(sort) == false && AllBrandList is not null)
            {
                bool delete = false;
                if (string.IsNullOrWhiteSpace(search) == false)
                {
                    AllBrandList = SearchSoftDelete<Brand>.SearchByName(AllBrandList, search, delete);
                }
                else
                {
                       AllBrandList = GenericBrandService.Sort(AllBrandList, sort, delete);

                 
                }
            }
            if (AllBrandList is not null &&AllBrandList.Any())
            {
                var dataPagination = Pagination<Brand>.GetPaginationData(pg, AllBrandList,3);
                this.ViewBag.Pager = dataPagination.Item2;
                return View(dataPagination.Item1);
            }
         return View();
        }

        #endregion


        #region  deleted Brands
        [HttpGet]
        public async Task<IActionResult> DeletedBrand(string clear,string sort,string search, int pg = 1)
        {
            
            if(string.IsNullOrWhiteSpace(clear) == false)
            {
                DeletedBrandList = null;
            }
            if(DeletedBrandList is null)
            {
                DeletedBrandList = await serviceBrand.GetalldeletedBrands();
            }
            if (string.IsNullOrWhiteSpace(search) == false || string.IsNullOrWhiteSpace(sort) == false && DeletedBrandList is not null)
            {
                bool delete = true;
                if (string.IsNullOrWhiteSpace(search) == false)
                {
                    DeletedBrandList = SearchSoftDelete<Brand>.SearchByName(DeletedBrandList, search, delete);
                }
                else
                {
                   
                        DeletedBrandList = GenericBrandService.Sort(DeletedBrandList, sort, delete);

                }
               
            }
            if (DeletedBrandList is not null && DeletedBrandList.Any())
            {
                var dataPagination = Pagination<Brand>.GetPaginationData(pg, DeletedBrandList, 3);
                this.ViewBag.Pager = dataPagination.Item2;
                return View(dataPagination.Item1);
            }
          
            return View();
        }

        #endregion

        #region  create Brand
        [HttpGet]
        public IActionResult CreateBrand()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBrand(BrandViewModel brandViewModel)
        {

            if (ModelState.IsValid)
            {
                var name = User.Identity.Name;
                var result = await serviceBrand.add(brandViewModel, name);
                if(result is not null)
                {
                    notyf.Success("Create Brand Done", 10);
                    AllBrandList = null;
                }
                return RedirectToAction("Index");
            }

            return View(brandViewModel);
        }

        #endregion

        #region  updated Brand

        [HttpGet]
        public async Task<IActionResult> UpdateBrand(int Id)
        {
            var result = await serviceBrand.GetByid(Id);
            
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBrand(BrandUpdateViewModel brandUpdateViewModel)
        {
            if (ModelState.IsValid)
            {
                bool exist = await GenericBrandService.IsUpdateNameExist(brandUpdateViewModel.Name, brandUpdateViewModel.Id);
                if (exist)
                {
                    ModelState.AddModelError("Name", "Name Exist Or Restore it");
                    return View(brandUpdateViewModel);

                }
                var name = User.Identity.Name;
               string result= await serviceBrand.Update(brandUpdateViewModel, name);
                if(string.IsNullOrWhiteSpace(result) == false)
                {
                    notyf.Success("Update Brand Done", 10);
                    AllBrandList = null;
                }
                return RedirectToAction("Index");
            }
            return View(brandUpdateViewModel);
        }

        #endregion

        #region  delete Brands

        [HttpGet]
        public async Task<IActionResult> DeleteBrand(int Id)
        {
            var name = User.Identity.Name;
           var result= await serviceBrand.Delete(Id, name);
            if(result >0)
            {
                notyf.Success("Delete Brand Done", 10);
                DeletedBrandList = null;
                AllBrandList = null;
            }
            return RedirectToAction("Index");
        }

        #endregion


        #region restore Brand

        [HttpGet]
        public async Task<IActionResult> RestoreBrand(int Id)
        {
            var name = User.Identity.Name;
            var result =await serviceBrand.RestoreBrand(Id, name);
            if(result>0)
            {
                notyf.Success("Restore Brand Done", 10);
                DeletedBrandList = null;
                AllBrandList = null;
            }
            return RedirectToAction("DeletedBrand");
        }

        #endregion
    }
}
