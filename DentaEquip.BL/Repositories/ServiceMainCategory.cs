using DentaEquip.BL.IRepositories;
using DentaEquip.BL.Search;
using DentaEquip.BL.ViewModels.MainCategory;
using DentaEquip.DAL.Context;
using DentaEquip.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.Repositories
{
    public class ServiceMainCategory : IServiceMainCategory
    {
        private readonly EntityContext context;
        private readonly IGenericServiceSoftDelete<MainCategory> genericRepository;
        private readonly IGenericServiceSoftDelete<Products> genericRepositoryProduct;
        private readonly IRelationModelsRestoreAndDelete relationModelsRestoreAndDelete;
        public ServiceMainCategory(IRelationModelsRestoreAndDelete relationModelsRestoreAndDelete, IGenericServiceSoftDelete<MainCategory> genericRepository, EntityContext context, IGenericServiceSoftDelete<Products> genericRepositoryProduct)
        {
            this.genericRepository = genericRepository;
            this.context = context;
            this.genericRepositoryProduct = genericRepositoryProduct;
            this.relationModelsRestoreAndDelete = relationModelsRestoreAndDelete;
          
        }

        public async Task<MainCategory> add(MainCategoryViewModel categoryViewModel, string name)
        {
            try
            {
                if (categoryViewModel is not null && string.IsNullOrWhiteSpace(name) == false)
                {
                    MainCategory categories = new MainCategory()
                    {
                        Name = categoryViewModel.Name,
                        UserName = name
                    };
                    return await genericRepository.add(categories);

                }
                return new MainCategory();
            }
            catch (Exception)
            {
                return new MainCategory();
            }
        }
        public async Task<int> Delete(int id, string name)
        {
            try
            {
                if(id>0 && string.IsNullOrWhiteSpace(name) == false)
                {
                    return await relationModelsRestoreAndDelete.MainCategoryDelete(id, name);
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<List<MainCategory>> GetallCategories()
        {
            try
            {
                var list = await genericRepository.Getall();
                if (list is not null)
                {
                    return list;
                }
                else
                {
                    return new List<MainCategory>();
                }
            }
            catch (Exception)
            {
                return new List<MainCategory>();
            }
        }

        public async Task<List<MainCategory>> GetalldeletedCategories()
        {
            try
            {
                List<MainCategory> list = await genericRepository.GetallSoftDeleted();
                if (list is not null)
                {
                    return list;
                }
                else
                {
                    return new List<MainCategory>();
                }
            }
            catch (Exception)
            {
                return new List<MainCategory>();
            }
        }

        public async Task<MainCategoryUpdateViewModel> GetByid(int id)
        {
            try
            {
                if (id == 0)
                {
                    return new MainCategoryUpdateViewModel();
                }
                var result = await genericRepository.GetById(id);
                if (result is not null)
                {
                    MainCategoryUpdateViewModel categoryUpdateViewModel = new MainCategoryUpdateViewModel()
                    {
                        Name = result.Name,
                        Id = result.Id
                    };
                    return categoryUpdateViewModel;
                }
                return new MainCategoryUpdateViewModel();
            }
            catch (Exception)
            {
                return new MainCategoryUpdateViewModel();
            }
        }

        public async Task<int> RestoreCategory(int id, string name)
        {
            try
            {
                if (id > 0 && string.IsNullOrWhiteSpace(name) == false)
                {
                    return await genericRepository.Restordeleted(id, name);
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<string> Update(MainCategoryUpdateViewModel categoryUpdateViewModel, string name)
        {
            try
            {
                if (categoryUpdateViewModel is  null || string.IsNullOrWhiteSpace(name) == true)
                {
                    return string.Empty;
                   
                }
                var oldcategory = await genericRepository.GetById(categoryUpdateViewModel.Id);
                if (oldcategory is not null)
                {
                    oldcategory.Name = categoryUpdateViewModel.Name;
                    var result = await genericRepository.update(oldcategory, name);
                    return result;
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
