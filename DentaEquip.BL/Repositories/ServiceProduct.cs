using DentaEquip.BL.IRepositories;
using DentaEquip.BL.ViewModels.Product;
using DentaEquip.DAL.Context;
using DentaEquip.DAL.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DentaEquip.BL.Repositories
{
    public class ServiceProduct : IServiceProduct
    {
        private readonly EntityContext context;
        private readonly IGenericServiceSoftDelete<Products> genericRepository;
        private readonly IGenericServiceSoftDelete<SubCategory> genericRepositoryCategory;
        private readonly IHostingEnvironment _hosting;
        private readonly IRelationModelsRestoreAndDelete relationModelsRestoreAndDelete;
        public ServiceProduct(IRelationModelsRestoreAndDelete relationModelsRestoreAndDelete, IGenericServiceSoftDelete<Products> genericRepository, IGenericServiceSoftDelete<SubCategory> genericRepositoryCategory, EntityContext context, IHostingEnvironment hosting)
        {
            this.genericRepository = genericRepository;
            this.genericRepositoryCategory = genericRepositoryCategory;
            this.context = context;
            this._hosting = hosting;
            this.relationModelsRestoreAndDelete = relationModelsRestoreAndDelete;
        }
        public async Task<Products> add(ProductViewModel productViewModel, string name)
        {
            try
            {
                if (productViewModel is  null || string.IsNullOrWhiteSpace(name) == true)
                {
                    return new Products();
                }
                string filename = string.Empty;
                string filename2 = string.Empty;
                string filename3 = string.Empty;
                if (productViewModel.File is not null)
                {
                    string uploads = Path.Combine(_hosting.WebRootPath, ("uploaded_img"));
                    filename = Guid.NewGuid().ToString() + "_" + productViewModel.File.FileName;
                    string fullpath = Path.Combine(uploads, filename);
                    productViewModel.File.CopyTo(new FileStream(fullpath, FileMode.Create));
                }
                if (productViewModel.File2 is not null)
                {
                    string uploads = Path.Combine(_hosting.WebRootPath, ("uploaded_img"));
                    filename2 = Guid.NewGuid().ToString() + "_" + productViewModel.File2.FileName;
                    string fullpath = Path.Combine(uploads, filename2);
                    productViewModel.File2.CopyTo(new FileStream(fullpath, FileMode.Create));
                }
                if (productViewModel.File3 is not null)
                {
                    string uploads = Path.Combine(_hosting.WebRootPath, ("uploaded_img"));
                    filename3 = Guid.NewGuid().ToString() + "_" + productViewModel.File3.FileName;
                    string fullpath = Path.Combine(uploads, filename3);
                    productViewModel.File3.CopyTo(new FileStream(fullpath, FileMode.Create));
                }

                Products Product = new Products();


                Product.Name = productViewModel.Name;
                Product.Price = productViewModel.Price;
                Product.Quantity = productViewModel.Quantity;
                Product.Discount = productViewModel.Discount;
                Product.Detailes = productViewModel.Detailes;
                Product.imgpath = filename;
                Product.imgpath2 = filename2;
                Product.imgpath3 = filename3;
                Product.SubCategoryId = productViewModel.CategoryId;
                Product.UserName = name;
                Product.Shade = productViewModel.Shade;
                Product.CureType = productViewModel.CureType;
                Product.CountryOfOrigin = productViewModel.CountryOfOrigin;
                Product.BurColorCode = productViewModel.BurColorCode;
                Product.BurMaximumRPM = productViewModel.BurMaximumRPM;
                Product.BurShape = productViewModel.BurShape;
                Product.BurOrderNumber = productViewModel.BurOrderNumber;
                Product.FileMaterial = productViewModel.FileMaterial;
                Product.FileShape = productViewModel.FileShape;
                if (productViewModel.BrandId > 0)
                {
                    Product.BrandId = productViewModel.BrandId;
                }
                else
                {
                    Product.BrandId = null;
                }

                return await genericRepository.add(Product);
            }
            catch (Exception)
            {
                return new Products();
            }
        }

        public async Task<int> Delete(int id, string name)
        {
            try
            {
                if (id == 0 || string.IsNullOrWhiteSpace(name) == true)
                {
                    return 0;
                }
                int result = await genericRepository.delete(id, name);
                if (result > 0)
                {
                    var comments = await context.Comment.Where(o => o.ProductId == id).AsNoTracking().ToListAsync();
                    context.Comment.RemoveRange(comments);
                    await context.SaveChangesAsync();
                }
                return result;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<List<Products>> GetalldeletedProducts()
        {
            try
            {
                var listPrd = await context.Product.
                   Where(o => o.IsDeleted == true)
                   .Include(o => o.SubCategories)
                   .Include(o => o.Brand).AsNoTracking().ToListAsync();

                if (listPrd is not null)
                {
                    return listPrd;
                }
                else
                {
                    return new List<Products>();
                }
            }
            catch (Exception)
            {
                return new List<Products>();
            }

        }

        public async Task<List<Products>> GetallProducts()
        {
            try
            {
                var listPrd = await context.Product.
                    Where(o => o.IsDeleted == false)
                    .Include(o => o.SubCategories)
                    .Include(o => o.Brand).AsNoTracking().ToListAsync();

                if (listPrd is not null)
                {
                    return listPrd;
                }
                else
                {
                    return new List<Products >();
                }
            }
            catch (Exception)
            {
                return new List<Products>();
            }
        }

        public async Task<ProductUpdateViewModel> GetByid(int id)
        {
            try
            {
                if (id == 0)
                {
                   return new ProductUpdateViewModel();

                }
                var result = await genericRepository.GetById(id);
                if (result is not null)
                {
                    ProductUpdateViewModel productUpdateViewModel = new ProductUpdateViewModel()
                    {
                        Id = result.Id,
                        Name = result.Name,
                        Price = result.Price,
                        Quantity = result.Quantity,
                        imagepath = result.imgpath,
                        imagepath2 = result.imgpath2,
                        imagepath3 = result.imgpath3,
                        Discount = result.Discount,
                        Detailes = result.Detailes,
                        CategoryId = result.SubCategoryId,
                        Shade = result.Shade,
                        CureType = result.CureType,
                        CountryOfOrigin = result.CountryOfOrigin,
                        BurColorCode = result.BurColorCode,
                        BurMaximumRPM = result.BurMaximumRPM,
                        BurShape = result.BurShape,
                        BurOrderNumber = result.BurOrderNumber,
                        FileMaterial = result.FileMaterial,
                        FileShape = result.FileShape,
                        BrandId = result.BrandId
                    };
                    return productUpdateViewModel;
                }
                return new ProductUpdateViewModel();
            }
            catch (Exception)
            {
                return new ProductUpdateViewModel();
            }
        }

        public async Task<int> RestoreProduct(int id, string name)
        {
            try
            {
                if (id ==0  || string.IsNullOrWhiteSpace(name) == true)
                {
                    return 0;
                }
                var product = await context.Product.Where(o => o.Id == id && o.IsDeleted == true).Select(o => new { o.SubCategoryId, o.BrandId }).AsNoTracking().FirstOrDefaultAsync();

                if (product.SubCategoryId > 0)
                {
                    return await relationModelsRestoreAndDelete.ProductRestore(id, product.SubCategoryId, product.BrandId, name);
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public List<Products> Sort(List<Products> list, string sort, bool delete)
        {
            try
            {
                if (list is not null && list.Any() && string.IsNullOrWhiteSpace(sort) == false)
                {
                    var result = sort switch
                    {
                        "SortNameA-Z" => list.Where(o => o.IsDeleted == delete).OrderBy(o => o.Name).ToList(),
                        "SortNameZ-A" => list.Where(o => o.IsDeleted == delete).OrderByDescending(o => o.Name).ToList(),
                        "SortSubCategoryA-Z" => list.Where(o => o.IsDeleted == delete).OrderBy(o => o.SubCategories.Name).ToList(),
                        "SortQuantityLow-High" => list.Where(o => o.IsDeleted == delete).OrderBy(o => o.Quantity).ToList(),
                        "SortSubCategoryZ-A" => list.Where(o => o.IsDeleted == delete).OrderByDescending(o => o.SubCategories.Name).ToList(),
                        "SortBrandA-Z" => list.Where(o => o.IsDeleted == delete).OrderBy(o => o.Brand.Name).ToList(),
                        "SortBrandZ-A" => list.Where(o => o.IsDeleted == delete).OrderByDescending(o => o.Brand.Name).ToList(),
                        "SortPriceHigh-Low" => list.Where(o => o.IsDeleted == delete).OrderByDescending(o => o.Price).ToList(),
                        "SortPriceLow-High" => list.Where(o => o.IsDeleted == delete).OrderBy(o => o.Price).ToList(),
                        _ => new List<Products>()
                    };
                    return result;
                }
                return new List<Products>();
            }
            catch (Exception)
            {
                return new List<Products>();
            }
        }

        public async Task<string> Update(ProductUpdateViewModel productUpdateViewModel, string name)
        {
            try
            {
                if (productUpdateViewModel is  null || string.IsNullOrWhiteSpace(name) == true)
                {
                    return string.Empty;
                }
                string filename = string.Empty;
                string filename2 = string.Empty;
                string filename3 = string.Empty;
                var oldobj = await genericRepository.GetById(productUpdateViewModel.Id);
                if (productUpdateViewModel.File is not null)
                {
                    string uploads = Path.Combine(_hosting.WebRootPath, ("uploaded_img"));
                    filename = Guid.NewGuid().ToString() + "_" + productUpdateViewModel.File.FileName;
                    string fullpath = Path.Combine(uploads, filename);
                    if (oldobj.imgpath is not null)
                    {
                        string oldfilename = oldobj.imgpath;
                        string oldfullpath = Path.Combine(uploads, oldfilename);
                        FileInfo fi = new FileInfo(Path.Combine(oldfullpath, oldfilename));
                        System.IO.File.Delete(oldfullpath);
                        if (fi.Exists)
                        {
                            fi.Delete();
                        }
                    }
                    productUpdateViewModel.File.CopyTo(new FileStream(fullpath, FileMode.Create));
                }
                else
                {
                    filename = oldobj.imgpath;
                }
                if (productUpdateViewModel.File2 is not null)
                {
                    string uploads = Path.Combine(_hosting.WebRootPath, ("uploaded_img"));
                    filename2 = Guid.NewGuid().ToString() + "_" + productUpdateViewModel.File2.FileName;
                    string fullpath = Path.Combine(uploads, filename2);
                    if (oldobj.imgpath2 != null)
                    {
                        string oldfilename = oldobj.imgpath2;
                        string oldfullpath = Path.Combine(uploads, oldfilename);
                        FileInfo fi = new FileInfo(Path.Combine(oldfullpath, oldfilename));
                        System.IO.File.Delete(oldfullpath);
                        if (fi.Exists)
                        {
                            fi.Delete();
                        }
                    }
                    productUpdateViewModel.File2.CopyTo(new FileStream(fullpath, FileMode.Create));
                }
                else
                {
                    filename2 = oldobj.imgpath2;
                }
                if (productUpdateViewModel.File3 is not null)
                {
                    string uploads = Path.Combine(_hosting.WebRootPath, ("uploaded_img"));
                    filename3 = Guid.NewGuid().ToString() + "_" + productUpdateViewModel.File3.FileName;
                    string fullpath = Path.Combine(uploads, filename3);
                    if (oldobj.imgpath3 is not null)
                    {
                        string oldfilename = oldobj.imgpath3;
                        string oldfullpath = Path.Combine(uploads, oldfilename);
                        FileInfo fi = new FileInfo(Path.Combine(oldfullpath, oldfilename));
                        System.IO.File.Delete(oldfullpath);
                        if (fi.Exists)
                        {
                            fi.Delete();
                        }
                    }
                    productUpdateViewModel.File3.CopyTo(new FileStream(fullpath, FileMode.Create));
                }
                else
                {
                    filename3 = oldobj.imgpath3;
                }
                oldobj.Name = productUpdateViewModel.Name;
                oldobj.Price = productUpdateViewModel.Price;
                oldobj.Quantity = productUpdateViewModel.Quantity;
                oldobj.Discount = productUpdateViewModel.Discount;
                oldobj.Detailes = productUpdateViewModel.Detailes;
                oldobj.imgpath = filename;
                oldobj.imgpath2 = filename2;
                oldobj.imgpath3 = filename3;
                oldobj.Shade = productUpdateViewModel.Shade;
                oldobj.CureType = productUpdateViewModel.CureType;
                oldobj.CountryOfOrigin = productUpdateViewModel.CountryOfOrigin;
                oldobj.BurColorCode = productUpdateViewModel.BurColorCode;
                oldobj.BurMaximumRPM = productUpdateViewModel.BurMaximumRPM;
                oldobj.BurShape = productUpdateViewModel.BurShape;
                oldobj.BurOrderNumber = productUpdateViewModel.BurOrderNumber;
                oldobj.FileMaterial = productUpdateViewModel.FileMaterial;
                oldobj.FileShape = productUpdateViewModel.FileShape;
                if (productUpdateViewModel.BrandId > 0)
                {
                    oldobj.BrandId = productUpdateViewModel.BrandId;
                }
                else
                {
                    oldobj.BrandId = null;
                }
                oldobj.SubCategoryId = productUpdateViewModel.CategoryId;
                var result = await genericRepository.update(oldobj, name);
                if (result is not null)
                {
                    Cart cartUpdatedItem = await context.Cart.Where(o => o.ProductId == oldobj.Id).FirstOrDefaultAsync();
                    if (cartUpdatedItem is not null)
                    {
                        context.Cart.Remove(cartUpdatedItem);
                    }
                    var wishListUpdatedItem = await context.WishList.Where(o => o.ProductId == oldobj.Id).FirstOrDefaultAsync();
                    if (wishListUpdatedItem is not null)
                    {
                        context.WishList.Remove(wishListUpdatedItem);
                    }
                    await context.SaveChangesAsync();

                }
                return result;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

    }
}
