using AspNetCoreHero.ToastNotification.Abstractions;
using DentaEquip.BL.IRepositories;
using DentaEquip.BL.Pagination;
using DentaEquip.BL.Search;
using DentaEquip.BL.ViewModels.MainCategory;
using DentaEquip.DAL.Constant;
using DentaEquip.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentaEquip.Controllers
{
    [Authorize(Roles = Roles.adminrole)]

    public class MainCategoryController : Controller
    {
        private readonly IServiceMainCategory serviceCategory;
        private readonly IGenericServiceSoftDelete<MainCategory> genericServiceCategory;
        private readonly INotyfService notyf;
        static List<MainCategory> AllCategoryList;
        static List<MainCategory> DeletedListCategory;

        public MainCategoryController(IServiceMainCategory serviceCategory, INotyfService notyf, IGenericServiceSoftDelete<MainCategory> genericServiceCategory)
        {
            this.serviceCategory = serviceCategory;
            this.notyf = notyf;
            this.genericServiceCategory = genericServiceCategory;
        }
        #region all categories
        public async Task<IActionResult> Index(string clear,string sort,string search, int pg = 1)
        {
           
            if(string.IsNullOrWhiteSpace(clear) == false)
            {
                AllCategoryList = null;
            }
            if(AllCategoryList is null)
            {
                AllCategoryList = await serviceCategory.GetallCategories();
            }
            if (string.IsNullOrWhiteSpace(search) == false || string.IsNullOrWhiteSpace(sort) == false && AllCategoryList is not null)
            {
                bool delete = false;
                if (string.IsNullOrWhiteSpace(search) == false)
                {
                    AllCategoryList = SearchSoftDelete<MainCategory>.SearchByName(AllCategoryList, search, delete);
                }
                else
                {
                     AllCategoryList = genericServiceCategory.Sort(AllCategoryList, sort, delete);

                 }
            }
            if (AllCategoryList is not null && AllCategoryList.Any())
            {
                var dataPagination = Pagination<MainCategory>.GetPaginationData(pg, AllCategoryList,4);
                this.ViewBag.Pager = dataPagination.Item2;
                return View(dataPagination.Item1);
            }
                
                return View();
            
        }

        #endregion


        #region  deleted categories
        [HttpGet]
        public async Task<IActionResult> DeletedCategory(string clear,string sort,string search, int pg = 1)
        {
            
            if (string.IsNullOrWhiteSpace(clear) == false)
            {
                DeletedListCategory = null;
            }
            if(DeletedListCategory is null)
            {
                DeletedListCategory = await serviceCategory.GetalldeletedCategories();
            }

            if (string.IsNullOrWhiteSpace(search) == false || string.IsNullOrWhiteSpace(sort) == false && DeletedListCategory is not null)
            {
                bool delete = true;
                if (string.IsNullOrWhiteSpace(search) == false)
                {
                    DeletedListCategory = SearchSoftDelete<MainCategory>.SearchByName(DeletedListCategory, search, delete);
                }
                else
                {
                      DeletedListCategory = genericServiceCategory.Sort(DeletedListCategory, sort, delete);
                }
            }
            if (DeletedListCategory is not null &&DeletedListCategory.Any())
            {
                var dataPagination = Pagination<MainCategory>.GetPaginationData(pg, DeletedListCategory,4);
                this.ViewBag.Pager = dataPagination.Item2;
                return View(dataPagination.Item1);
            }
           
            return View();
        }

        #endregion

        #region  create categories
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(MainCategoryViewModel categoryViewModel)
        {

            if (ModelState.IsValid)
            {
                var name = User.Identity.Name;
                var result = await serviceCategory.add(categoryViewModel, name);
                if(result is not null)
                {
                    notyf.Success("Create MainCategory Done", 10);
                    AllCategoryList = null;

                }
                return RedirectToAction("Index");
            }

            return View(categoryViewModel);
        }

        #endregion

        #region  updated categories

        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int Id)
        {
            var result = await serviceCategory.GetByid(Id);
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategory(MainCategoryUpdateViewModel categoryUpdateViewModel)
        {
            if (ModelState.IsValid)
            {
                bool exist = await genericServiceCategory.IsUpdateNameExist(categoryUpdateViewModel.Name, categoryUpdateViewModel.Id);
                if (exist)
                {
                    ModelState.AddModelError("Name", "Name Exist Or Restore it");
                    return View(categoryUpdateViewModel);

                }
                var name = User.Identity.Name;
               string result= await serviceCategory.Update(categoryUpdateViewModel, name);
                if (string.IsNullOrWhiteSpace(result) == false)
                {
                    notyf.Success("Update MainCategory Done", 10);
                    AllCategoryList = null;

                }
                return RedirectToAction("Index");
            }
            return View(categoryUpdateViewModel);
        }

        #endregion

        #region  delete categories

        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int Id)
        {
            var name = User.Identity.Name;
           var result= await serviceCategory.Delete(Id, name);
            if (result >0)
            {
                notyf.Success("Delete MainCategory Done", 10);
                DeletedListCategory = null;
                AllCategoryList = null;

            }
            return RedirectToAction("Index");
        }

        #endregion


        #region restore categories

        [HttpGet]
        public async Task<IActionResult> RestoreCategory(int Id)
        {
            var name = User.Identity.Name;
            var result=await serviceCategory.RestoreCategory(Id, name);
            if (result >0)
            {
                notyf.Success("Restore MainCategory Done", 10);
                DeletedListCategory = null;
                AllCategoryList = null;
            }
            return RedirectToAction("DeletedCategory");
        }

        #endregion
    }
}
