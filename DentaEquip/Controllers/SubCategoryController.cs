﻿using AspNetCoreHero.ToastNotification.Abstractions;
using DentaEquip.BL.IRepositories;
using DentaEquip.BL.Pagination;
using DentaEquip.BL.Search;
using DentaEquip.BL.ViewModels.SubCategory;
using DentaEquip.DAL.Constant;
using DentaEquip.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentaEquip.Controllers
{
    [Authorize(Roles = Roles.adminrole)]
    public class SubCategoryController : Controller
    {
        private readonly IServiceSubCategory serviceSubCategory;
        private readonly IServiceMainCategory serviceMaincategory;
        private readonly IGenericServiceSoftDelete<SubCategory> serviceSubCategorySoftDelete;
        private readonly INotyfService notyf;
        static List<ShowSubCategoryModels> AllSubCategory;
        static List<ShowSubCategoryModels> AllDeletedSubCategory;
        public SubCategoryController(IServiceSubCategory serviceSubCategory, IServiceMainCategory serviceMaincategory, INotyfService notyf, IGenericServiceSoftDelete<SubCategory> serviceSubCategorySoftDelete)
        {
            this.serviceSubCategory = serviceSubCategory;
            this.serviceMaincategory = serviceMaincategory;
            this.notyf = notyf;
            this.serviceSubCategorySoftDelete = serviceSubCategorySoftDelete;
        }
        #region all SubCategory
        public async Task<IActionResult> Index(string clear,string sort,string search, int pg = 1)
        {

            
            if(string.IsNullOrWhiteSpace(clear) == false)
            {
                AllSubCategory = null;
            }
            if(AllSubCategory is null)
            {
                AllSubCategory = await serviceSubCategory.GetallSubCategories();
            }
            if (string.IsNullOrWhiteSpace(search) == false || string.IsNullOrWhiteSpace(sort) == false && AllSubCategory is not null)
            {
                bool delete = false;
                if (string.IsNullOrWhiteSpace(search) == false)
                {
                    AllSubCategory = SearchSoftDelete<ShowSubCategoryModels>.SearchByName(AllSubCategory, search, delete);
                }
                else
                {
                    AllSubCategory = serviceSubCategory.Sort(AllSubCategory, sort, delete);

                }
            }
            if (AllSubCategory is not null && AllSubCategory.Any())
            {
                var dataPagination = Pagination<ShowSubCategoryModels>.GetPaginationData(pg, AllSubCategory,4);
                this.ViewBag.Pager = dataPagination.Item2;
                return View(dataPagination.Item1);
            }
       
            return View();
        }

        #endregion


        #region all deleted SubCategory
        [HttpGet]
        public async Task<IActionResult> DeletedSubCategory(string clear,string sort,string search, int pg = 1)
        {
           
            if(string.IsNullOrWhiteSpace(clear) == false)
            {
                AllDeletedSubCategory = null;
            }
            if(AllDeletedSubCategory is null)
            {
                AllDeletedSubCategory = await serviceSubCategory.GetalldeletedSubCategories();
            }
            if (string.IsNullOrWhiteSpace(search) == false || string.IsNullOrWhiteSpace(sort) == false && AllDeletedSubCategory is not null)
            {
                bool delete = true;
                if (string.IsNullOrWhiteSpace(search) == false)
                {
                    AllDeletedSubCategory = SearchSoftDelete<ShowSubCategoryModels>.SearchByName(AllDeletedSubCategory, search, delete);
                }
                else
                {
                      AllDeletedSubCategory = serviceSubCategory.Sort(AllDeletedSubCategory, sort, delete);

                }
            }
            if (AllDeletedSubCategory is not null && AllDeletedSubCategory.Any())
            {
                var dataPagination = Pagination<ShowSubCategoryModels>.GetPaginationData(pg, AllDeletedSubCategory,4);
                this.ViewBag.Pager = dataPagination.Item2;
                return View(dataPagination.Item1);
            }
       
            return View();
        }

        #endregion

        #region all create SubCategory
        [HttpGet]
        public async Task<IActionResult> CreateSubCategory()
        {
            var list = await serviceMaincategory.GetallCategories();
            ViewBag.list = list;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubCategory(SubCategoryViewModel subCategoryViewModel)
        {

            if (ModelState.IsValid)
            {
                var name = User.Identity.Name;
                var result = await serviceSubCategory.add(subCategoryViewModel, name);
                if(result is not null)
                {
                    notyf.Success("Create SubCategory Done", 10);
                    AllSubCategory = null;
                }
                return RedirectToAction("Index");
            }
            var list = await serviceMaincategory.GetallCategories();
            ViewBag.list = list;
            return View(subCategoryViewModel);
        }

        #endregion

        #region all updated subCategory

        [HttpGet]
        public async Task<IActionResult> UpdateSubCategory(int Id)
        {
            var result = await serviceSubCategory.GetByid(Id);
            var list = await serviceMaincategory.GetallCategories();
            ViewBag.list = list;
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> UpdateSubCategory(SubCategoryUpdateViewModel subCategoryUpdateViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool exist = await serviceSubCategorySoftDelete.IsUpdateNameExist(subCategoryUpdateViewModel.Name, subCategoryUpdateViewModel.Id);
                    if (exist)
                    {
                        ModelState.AddModelError("Name", "Name Exist Or Restore it");
                        var listSubCategory = await serviceMaincategory.GetallCategories();
                        ViewBag.list = listSubCategory;
                        return View(subCategoryUpdateViewModel);

                    }
                    var name = User.Identity.Name;
                  string result= await serviceSubCategory.Update(subCategoryUpdateViewModel, name);
                    if (string.IsNullOrWhiteSpace(result) == false)
                    {
                        notyf.Success("Update SubCategory Done", 10);
                        AllSubCategory = null;
                    }
                    return RedirectToAction("Index");
                }
                catch (ArgumentNullException)
                {
                    ModelState.AddModelError("Name", "Updated Name Already exist in SubCategory or Restor it from Deleted Menus");
                }

            }
            var list =await serviceMaincategory.GetallCategories();
            ViewBag.list = list;
            return View(subCategoryUpdateViewModel);
        }

        #endregion

        #region  delete SubCategory

        [HttpGet]
        public async Task<IActionResult> DeleteSubCategory(int Id)
        {
            var name = User.Identity.Name;
            var result=await serviceSubCategory.Delete(Id, name);
            if (result >0)
            {
                notyf.Success("Delete SubCategory Done", 10);
                AllDeletedSubCategory = null;
                AllSubCategory = null;
            }
            return RedirectToAction("Index");
        }

        #endregion


        #region restore SubCategory

        [HttpGet]
        public async Task<IActionResult> RestoreSubCategory(int Id)
        {
            var name = User.Identity.Name;
            var result=await serviceSubCategory.RestoreSubCategory(Id, name);
            if (result >0)
            {
                notyf.Success("Restore SubCategory Done", 10);
                AllDeletedSubCategory = null;
                AllSubCategory = null;
            }
            return RedirectToAction("DeletedSubCategory");
        }

        #endregion
    }
}
