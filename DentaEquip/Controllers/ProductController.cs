using AspNetCoreHero.ToastNotification.Abstractions;
using DentaEquip.BL.IRepositories;
using DentaEquip.BL.Pagination;
using DentaEquip.BL.Search;
using DentaEquip.BL.ViewModels.Product;
using DentaEquip.DAL.Constant;
using DentaEquip.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentaEquip.Controllers
{
    [Authorize(Roles = Roles.adminrole)]

    public class ProductController : Controller
    {
        private readonly IServiceProduct serviceproduct;
        private readonly IServiceSubCategory servicecategory;
        private readonly IServiceBrand serviceBrand;
        private readonly IGenericServiceSoftDelete<Products> genericServiceSoftDelete;
        private readonly INotyfService notyf;
        static List<Products> AllProductList;
        static List<Products> AllDeletedProduct;
        public ProductController(IServiceBrand serviceBrand, IServiceProduct serviceproduct, IServiceSubCategory servicecategory, INotyfService notyf, IGenericServiceSoftDelete<Products> genericServiceSoftDelete)
        {
            this.serviceproduct = serviceproduct;
            this.servicecategory = servicecategory;
            this.serviceBrand = serviceBrand;
            this.notyf = notyf;
            this.genericServiceSoftDelete = genericServiceSoftDelete;
        }
        #region all Products
        public async Task<IActionResult> Index(string clear,string sort,string search, int pg = 1)
        {

           
            if(string.IsNullOrWhiteSpace(clear) == false)
            {
                AllProductList = null;
            }
            if(AllProductList is null)
            {
                AllProductList = await serviceproduct.GetallProducts();
            }
            if (string.IsNullOrWhiteSpace(search) == false || string.IsNullOrWhiteSpace(sort) == false && AllProductList is not null)
            {
                bool delete = false;
                if (string.IsNullOrWhiteSpace(search) == false)
                {
                    AllProductList = SearchSoftDelete<Products>.SearchByName(AllProductList, search, delete);
                }
                else
                {
                     AllProductList = serviceproduct.Sort(AllProductList, sort, delete);

                   
                }
            }
            if (AllProductList is not null && AllProductList.Any())
            {
                var dataPagination = Pagination<Products>.GetPaginationData(pg, AllProductList,3);
                this.ViewBag.Pager = dataPagination.Item2;
                return View(dataPagination.Item1);
            }
      
            return View();
        }

        #endregion


        #region all deleted product
        [HttpGet]
        public async Task<IActionResult> Deletedproduct(string clear,string sort,string search, int pg = 1)
        {
            
            if(string.IsNullOrWhiteSpace(clear) == false)
            {
                AllDeletedProduct = null;
            }
            if(AllDeletedProduct is null)
            {
               AllDeletedProduct = await serviceproduct.GetalldeletedProducts();
            }
            if (string.IsNullOrWhiteSpace(search) == false || string.IsNullOrWhiteSpace(sort) == false && AllDeletedProduct is not null)
            {
                bool delete = true;
                if (string.IsNullOrWhiteSpace(search) == false)
                {
                    AllDeletedProduct = SearchSoftDelete<Products>.SearchByName(AllDeletedProduct, search, delete);
                }
                else
                {
                       AllDeletedProduct = serviceproduct.Sort(AllDeletedProduct, sort, delete);

                    
                }
              
            }
            if (AllDeletedProduct is not null && AllDeletedProduct.Any())
            {
                var dataPagination = Pagination<Products>.GetPaginationData(pg, AllDeletedProduct,3);
                this.ViewBag.Pager = dataPagination.Item2;
                return View(dataPagination.Item1);
            }
          
            return View();
        }

        #endregion

        #region all create product
        [HttpGet]
        public async Task<IActionResult> CreateProducts()
        {
            var list = await servicecategory.GetallSubCategories();
            string selectListIdName = "getIdNanme";
            var listBrand = await serviceBrand.GetallBrands(selectListIdName);
            ViewBag.list = list;
            ViewBag.listBrand = listBrand;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProducts(ProductViewModel productViewModel)
        {

            if (ModelState.IsValid)
            {
                var name = User.Identity.Name;
                var result = await serviceproduct.add(productViewModel, name);
                if(result is not null)
                {
                    notyf.Success("Create Product Done",10);
                    AllProductList = null;
                }
                return RedirectToAction("Index");
            }
            var list = await servicecategory.GetallSubCategories();
            string selectListIdName = "getIdNanme";
            var listBrand = await serviceBrand.GetallBrands(selectListIdName);
            ViewBag.list = list;
            ViewBag.listBrand = listBrand;
            return View(productViewModel);
        }

        #endregion

        #region all updated product

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int Id)
        {
            var result = await serviceproduct.GetByid(Id);
            var list = await servicecategory.GetallSubCategories();
            string selectListIdName = "getIdNanme";
            var listBrand = await serviceBrand.GetallBrands(selectListIdName);
            ViewBag.list = list;
            ViewBag.listBrand = listBrand;
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> UpdateProduct(ProductUpdateViewModel productUpdateViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool exist = await genericServiceSoftDelete.IsUpdateNameExist(productUpdateViewModel.Name, productUpdateViewModel.Id);
                    if (exist)
                    {
                        ModelState.AddModelError("Name", "Name Exist Or Restore it");
                        var listProduct = await servicecategory.GetallSubCategories();
                        string selectListIdNames = "getIdNanme";
                        var listBrands = await serviceBrand.GetallBrands(selectListIdNames);
                        ViewBag.list =listProduct;
                        ViewBag.listBrand = listBrands;
                        return View(productUpdateViewModel);

                    }
                    var name = User.Identity.Name;
                   string result= await serviceproduct.Update(productUpdateViewModel, name);
                    if (string.IsNullOrWhiteSpace(result) == false)
                    {
                        notyf.Success("Update Product Done", 10);
                        AllProductList = null;
                    }
                    return RedirectToAction("Index");
                }
                catch (ArgumentNullException)
                {
                    ModelState.AddModelError("Name", "Updated Name Already exist in Menus or Restor it from Deleted Menus");
                }

            }
            var list = await servicecategory.GetallSubCategories();
            string selectListIdName = "getIdNanme";
            var listBrand = await serviceBrand.GetallBrands(selectListIdName);
            ViewBag.list = list;
            ViewBag.listBrand = listBrand;
            return View(productUpdateViewModel);
        }

        #endregion

        #region  deleted product

        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int Id)
        {
            var name = User.Identity.Name;
           var result= await serviceproduct.Delete(Id, name);
            if (result >0)
            {
                notyf.Success("Delete Product Done", 10);
                AllDeletedProduct = null;
                AllProductList = null;
            }
            return RedirectToAction("Index");
        }

        #endregion


        #region restore product

        [HttpGet]
        public async Task<IActionResult> RestoreProduct(int Id)
        {
            var name = User.Identity.Name;
           var result= await serviceproduct.RestoreProduct(Id, name);
            if (result > 0)
            {
                notyf.Success("Restore Product Done", 10);
                AllDeletedProduct = null;
                AllProductList = null;

            }
            return RedirectToAction("Deletedproduct");
        }

        #endregion
    }
}
