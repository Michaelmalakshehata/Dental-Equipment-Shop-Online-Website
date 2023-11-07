using DentaEquip.BL.IRepositories;
using DentaEquip.BL.ViewModels.SubCategory;
using DentaEquip.DAL.Context;
using DentaEquip.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.Repositories
{
    public class ServiceSubCategory : IServiceSubCategory
    {
        private readonly EntityContext context;
        private readonly IGenericServiceSoftDelete<SubCategory> serviceSubCategory;
        private readonly IRelationModelsRestoreAndDelete relationModelsRestoreAndDelete;
        public ServiceSubCategory(IRelationModelsRestoreAndDelete relationModelsRestoreAndDelete, IGenericServiceSoftDelete<SubCategory> serviceSubCategory, EntityContext context)
        {
            this.serviceSubCategory = serviceSubCategory;
            this.context = context;
            this.relationModelsRestoreAndDelete = relationModelsRestoreAndDelete;
        }

        public async Task<SubCategory> add(SubCategoryViewModel categoryViewModel, string name)
        {
            try
            {
                if (categoryViewModel is not null && string.IsNullOrWhiteSpace(name) == false)
                {
                    SubCategory categories = new SubCategory()
                    {
                        Name = categoryViewModel.Name,
                        MainCategoryId=categoryViewModel.MainCategoryId,
                        Type=categoryViewModel.Type,
                        UserName = name
                    };
                    return await serviceSubCategory.add(categories);

                }
                return new SubCategory();
            }
            catch (Exception)
            {
                return new SubCategory();
            }
        }
        public async Task<int> Delete(int id, string name)
        {
            try
            {
                if (id > 0 && string.IsNullOrWhiteSpace(name) == false)
                {
                    return await relationModelsRestoreAndDelete.SubCategoryDelete(id, name);
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public async Task<List<ShowSubCategoryModels>> GetallSubCategories()
        {
            try
            {
                var listSubCategory = await (from scgt in context.SubCategories
                                             join mcgt in context.MainCategories
                                             on scgt.MainCategoryId equals mcgt.Id
                                             where scgt.IsDeleted == false

                                             select new ShowSubCategoryModels
                                             {
                                                 Id=scgt.Id,
                                                 Name=scgt.Name,
                                                 Type=scgt.Type,
                                                 CreateDate=scgt.CreateDate,
                                                 UpdateDate=scgt.UpdateDate,
                                                 DeleteDate=scgt.DeleteDate,
                                                 RestoreDate=scgt.RestoreDate,
                                                 UserName=scgt.UserName,
                                                 IsDeleted=scgt.IsDeleted,
                                                 MainCategoryName =mcgt.Name
                                             }

                                          ).AsNoTracking().ToListAsync();
                if (listSubCategory is not null)
                {
                    return listSubCategory;
                }
               
                    return new List<ShowSubCategoryModels>();
                
            }
            catch (Exception)
            {
                return new List<ShowSubCategoryModels>();
            }
        }

        public async Task<List<ShowSubCategoryModels>> GetalldeletedSubCategories()
        {
            try
            {
                var listSubCategory = await (from scgt in context.SubCategories
                                             join mcgt in context.MainCategories
                                             on scgt.MainCategoryId equals mcgt.Id
                                             where scgt.IsDeleted == true

                                             select new ShowSubCategoryModels
                                             {
                                                 Id = scgt.Id,
                                                 Name = scgt.Name,
                                                 Type=scgt.Type,
                                                 CreateDate = scgt.CreateDate,
                                                 UpdateDate = scgt.UpdateDate,
                                                 DeleteDate = scgt.DeleteDate,
                                                 RestoreDate = scgt.RestoreDate,
                                                 UserName = scgt.UserName,
                                                 IsDeleted = scgt.IsDeleted,
                                                 MainCategoryName = mcgt.Name
                                             }

                                        ).AsNoTracking().ToListAsync();
                if (listSubCategory is not null)
                {
                    return listSubCategory;
                }
                
                    return new List<ShowSubCategoryModels>();
                
            }
            catch (Exception)
            {
                return new List<ShowSubCategoryModels>();
            }
        }

        public async Task<SubCategoryUpdateViewModel> GetByid(int id)
        {
            try
            {
                if (id == 0)
                {
                    return new SubCategoryUpdateViewModel();
                }
                var result = await serviceSubCategory.GetById(id);
                if (result is not null)
                {
                    SubCategoryUpdateViewModel categoryUpdateViewModel = new SubCategoryUpdateViewModel()
                    {
                        Name = result.Name,
                        MainCategoryId = result.MainCategoryId,
                        Type = result.Type,
                        Id = result.Id
                    };
                    return categoryUpdateViewModel;
                }
                return new SubCategoryUpdateViewModel();
            }
            catch (Exception)
            {
                return new SubCategoryUpdateViewModel();
            }
        }

        public async Task<int> RestoreSubCategory(int id, string name)
        {
            try
            {
                if (id == 0 || string.IsNullOrWhiteSpace(name) == true)
                {
                    return 0;
                }
                var mainCategoryId = await context.SubCategories.Where(o => o.Id == id && o.IsDeleted == true).AsNoTracking()
                       .Select(o => o.MainCategoryId).FirstOrDefaultAsync();
                if (mainCategoryId > 0)
                {
                    return await relationModelsRestoreAndDelete.SubCategoryRestore(id, mainCategoryId, name);
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<string> Update(SubCategoryUpdateViewModel categoryUpdateViewModel, string name)
        {
            try
            {
                if (categoryUpdateViewModel is  null || string.IsNullOrWhiteSpace(name) == true)
                {
                    return string.Empty;
                }
                var oldcategory = await serviceSubCategory.GetById(categoryUpdateViewModel.Id);
                if (oldcategory is not null)
                {
                    oldcategory.Name = categoryUpdateViewModel.Name;
                    oldcategory.MainCategoryId = categoryUpdateViewModel.MainCategoryId;
                    oldcategory.Type = categoryUpdateViewModel.Type;
                    var result = await serviceSubCategory.update(oldcategory, name);
                    return result;
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public List<ShowSubCategoryModels> Sort(List<ShowSubCategoryModels> list, string sort, bool delete)
        {
            try
            {
                if (list is not null && list.Any() && string.IsNullOrWhiteSpace(sort) == false)
                {
                    var result = sort switch
                    {
                        "SortNameA-Z" => list.Where(o => o.IsDeleted == delete).OrderBy(o => o.Name).ToList(),
                        "SortNameZ-A" => list.Where(o => o.IsDeleted == delete).OrderByDescending(o => o.Name).ToList(),
                        "SortSubCategoryTypeA-Z" => list.Where(o => o.IsDeleted == delete).OrderBy(o => o.Type).ToList(),
                        "SortMainCategoryA-Z" => list.Where(o => o.IsDeleted == delete).OrderBy(o => o.MainCategoryName).ToList(),
                        "SortMainCategoryZ-A" => list.Where(o => o.IsDeleted == delete).OrderByDescending(o => o.MainCategoryName).ToList(),
                        _ => new List<ShowSubCategoryModels>()
                    };
                    return result;
                }
                return new List<ShowSubCategoryModels>();
            }
            catch (Exception)
            {
                return new List<ShowSubCategoryModels>();
            }
          
        }
    }
}
