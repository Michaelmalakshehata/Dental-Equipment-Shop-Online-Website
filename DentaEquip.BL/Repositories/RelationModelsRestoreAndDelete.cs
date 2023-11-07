using DentaEquip.BL.IRepositories;
using DentaEquip.DAL.Context;
using DentaEquip.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.Repositories
{
    public class RelationModelsRestoreAndDelete:IRelationModelsRestoreAndDelete
    {
        private readonly EntityContext entityContext;
        private readonly IGenericServiceSoftDelete<MainCategory> genericServiceMainCategory;
        private readonly IGenericServiceSoftDelete<SubCategory> genericServiceSubCategory;
        private readonly IGenericServiceSoftDelete<Brand> genericServiceBrand;
        private readonly IGenericServiceSoftDelete<Products> genericServiceProduct;
        public RelationModelsRestoreAndDelete(EntityContext entityContext, IGenericServiceSoftDelete<MainCategory> genericServiceMainCategory, IGenericServiceSoftDelete<SubCategory> genericServiceSubCategory, IGenericServiceSoftDelete<Brand> genericServiceBrand, IGenericServiceSoftDelete<Products> genericServiceProduct)
        {
            this.entityContext = entityContext;
            this.genericServiceMainCategory = genericServiceMainCategory;
            this.genericServiceSubCategory = genericServiceSubCategory;
            this.genericServiceBrand = genericServiceBrand;
            this.genericServiceProduct = genericServiceProduct;
        }


        #region Delete Product
        public async Task<int> ProductRestore(int ProductId, int SubCategoryId, int? BrandId, string name)
        {
            try
            {
                int brandId = 0;
                int mainCategoryId = 0;
                int restoreProduct = 0;
                int restoreSubCategory = 0;
                if (BrandId > 0)
                {
                    brandId = await entityContext.Brand.
                        Where(o => o.Id == BrandId && o.IsDeleted == true).AsNoTracking().
                        Select(o => o.Id).FirstOrDefaultAsync();

                }
                if (brandId > 0)
                {
                    await genericServiceBrand.Restordeleted(brandId, name);
                }
                if (SubCategoryId > 0)
                {
                    mainCategoryId = await entityContext.SubCategories
                    .Where(o => o.Id == SubCategoryId).AsNoTracking().Select(o => o.MainCategoryId)
                    .FirstOrDefaultAsync();
                }
                if (SubCategoryId > 0 && mainCategoryId > 0)
                {
                    restoreSubCategory = await SubCategoryRestore(SubCategoryId, mainCategoryId, name);
                }
                if (restoreSubCategory > 0 && ProductId > 0)
                {
                    restoreProduct = await genericServiceProduct.Restordeleted(ProductId, name);
                }
                return restoreProduct;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion


        #region Restore SubCategory
        public async Task<int> SubCategoryRestore(int SubCategoryId, int MainCategoryId, string name)
        {
            try
            {
                int RestoreSubCategory = 0;
                int RestoreMainCategory = 0;
                if (MainCategoryId > 0)
                {
                    RestoreMainCategory = await genericServiceMainCategory.Restordeleted(MainCategoryId, name);
                }
                if (RestoreMainCategory > 0 && SubCategoryId > 0)
                {
                    RestoreSubCategory = await genericServiceSubCategory.Restordeleted(SubCategoryId, name);
                }
                return RestoreSubCategory;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion


        #region Delete SubCategory
        public async Task<int> SubCategoryDelete(int SubCategoryId, string name)
        {
            try
            {
                List<int> productIdList = new List<int>();
                int DeleteSubCategory = 0;
                if (SubCategoryId > 0)
                {
                    productIdList = await entityContext.Product.
                        Where(o => o.SubCategoryId == SubCategoryId && o.IsDeleted == false).AsNoTracking().
                        Select(o => o.Id).ToListAsync();
                }
                if (productIdList is not null && productIdList.Any())
                {
                    foreach (var product in productIdList)
                    {
                        await genericServiceProduct.delete(product, name);
                    }

                }
                if (SubCategoryId > 0)
                {
                    DeleteSubCategory = await genericServiceSubCategory.delete(SubCategoryId, name);
                }
                return DeleteSubCategory;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion


        #region Delete Brand
        public async Task<int> BrandDelete(int BrandId, string name)
        {
            try
            {
                List<int> prdIdList = new List<int>();
                int DeleteBrand = 0;
                if (BrandId > 0)
                {
                    prdIdList = await entityContext.Product.
                        Where(o => o.BrandId == BrandId && o.IsDeleted == false).AsNoTracking().
                        Select(o => o.Id).ToListAsync();
                }
                if (prdIdList is  not null && prdIdList.Any())
                {
                    foreach (var product in prdIdList)
                    {
                        await genericServiceProduct.delete(product, name);
                    }

                }
                if (BrandId > 0)
                {
                    DeleteBrand = await genericServiceBrand.delete(BrandId, name);
                }
                return DeleteBrand;
            }
            catch (Exception)
            {
                return 0;
            }
        }
#endregion


        #region Delete MainCategory
        public async Task<int> MainCategoryDelete(int MainCategoryId, string name)
        {
            try
            {
                List<int> prdIdList = new List<int>();
                List<int> SubCategoryIdList = new List<int>();
                int DeleteMainCategory = 0;
                if (MainCategoryId > 0)
                {
                    SubCategoryIdList = await entityContext.SubCategories
                        .Where(o => o.MainCategoryId == MainCategoryId && o.IsDeleted == false).AsNoTracking()
                        .Select(o => o.Id).ToListAsync();
                }
                if (SubCategoryIdList is not null && SubCategoryIdList.Any())
                {
                    foreach (var subCategory in SubCategoryIdList)
                    {
                        var prdSubCategoryList = await entityContext.Product.
                                              Where(o => o.SubCategoryId == subCategory && o.IsDeleted == false).AsNoTracking().
                                              Select(o => o.Id).ToListAsync();
                        if (prdSubCategoryList.Any())
                        {
                            prdIdList.AddRange(prdSubCategoryList);
                        }
                    }


                }
                if (prdIdList is not null && prdIdList.Any())
                {
                    foreach (var prdid in prdIdList)
                    {
                        await genericServiceProduct.delete(prdid, name);
                    }
                }
                if (SubCategoryIdList.Any())
                {
                    foreach (var subCategoryId in SubCategoryIdList)
                    {
                        await genericServiceSubCategory.delete(subCategoryId, name);
                    }
                }
                DeleteMainCategory = await genericServiceMainCategory.delete(MainCategoryId, name);
                return DeleteMainCategory;

            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion



    }
}
