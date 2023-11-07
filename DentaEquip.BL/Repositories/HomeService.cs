using DentaEquip.BL.IRepositories;
using DentaEquip.BL.ViewModels.Cart;
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
    public class HomeService : IHomeService
    {
        private readonly EntityContext context;
        public HomeService(EntityContext context)
        {
            this.context = context;
        }

        public async Task<List<MainCategory>> GetAllMainAndSubCategory()
        {
            try
            {
                return await context.MainCategories
                .Where(o => o.IsDeleted == false)
                .Include(o => o.SubCategories.Where(p => p.IsDeleted == false).OrderBy(s => s.Type))
                .AsNoTracking().ToListAsync();
            }
            catch(Exception)
            {
                return new List<MainCategory>();
            }
        }
        public async Task<List<Brand>> GetTenBrand()
        {
            try
            {
                var list = await context.Product.Where(o => o.IsDeleted == false && o.BrandId != null).GroupBy(p => p.BrandId).OrderByDescending(gp => gp.Count()).Take(10).AsNoTracking().Select(g => g.Key).ToListAsync();
                if (list is  null || list.Any()==false)
                {
                    return new List<Brand>();
                }
                List<Brand> brands = new List<Brand>();
                foreach (var item in list)
                {
                    var result = await context.Brand.Where(o => o.IsDeleted == false && o.Id == item).AsNoTracking().FirstOrDefaultAsync();
                    brands.Add(result);
                }
                return brands;
            }
            catch (Exception)
            {
                return new List<Brand>();
            }
        }

        public async Task<List<CartViewModel>> GetTenExclusiveOffersProduct()
        {
            try
            {
                var result = await context.Product.
                Where(o => o.IsDeleted == false &&o.Quantity>0&&o.Discount>0)
                .OrderByDescending(o => o.Discount)
                .Take(10).Include(o => o.Brand)
                .AsNoTracking().ToListAsync();

                if (result is null || result.Any()==false)
                {
                    return new List<CartViewModel>();
                }
                List<CartViewModel> cartViewModels = new List<CartViewModel>();
                foreach (var item in result)
                {
                    string brandname = null;
                    if (item.Brand is not null)
                    {
                        brandname = item.Brand.Name;

                    }

                    cartViewModels.Add(new CartViewModel
                    {
                        ProductId = item.Id,
                        Ordername = item.Name,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        imgpath = item.imgpath,
                        Discount = item.Discount,
                        Shade = item.Shade,
                        CureType = item.CureType,
                        CountryOfOrigin = item.CountryOfOrigin,
                        BurColorCode = item.BurColorCode,
                        BurMaximumRPM = item.BurMaximumRPM,
                        BurShape = item.BurShape,
                        BurOrderNumber = item.BurOrderNumber,
                        FileMaterial = item.FileMaterial,
                        FileShape = item.FileShape,
                        BrandName = brandname

                    });
                }
                return cartViewModels;
            }
            catch (Exception)
            {
                return new List<CartViewModel>();
            }
        }

        public async Task<List<CartViewModel>> GetTenNewestProduct()
        {
            try
            {
                var result = await context.Product.
               Where(o => o.IsDeleted == false && o.Quantity > 0)
               .OrderByDescending(o => o.CreateDate)
               .Take(10).Include(o => o.Brand)
               .AsNoTracking().ToListAsync();
                if (result is null ||result.Any()==false)
                {
                    return new List<CartViewModel>();
                }
                List<CartViewModel> cartViewModels = new List<CartViewModel>();
                foreach (var item in result)
                {
                    string brandname = null;
                    if (item.Brand is not null)
                    {
                        brandname = item.Brand.Name;

                    }
                    cartViewModels.Add(new CartViewModel
                    {
                        ProductId = item.Id,
                        Ordername = item.Name,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        imgpath = item.imgpath,
                        Discount = item.Discount,
                        Shade = item.Shade,
                        CureType = item.CureType,
                        CountryOfOrigin = item.CountryOfOrigin,
                        BurColorCode = item.BurColorCode,
                        BurMaximumRPM = item.BurMaximumRPM,
                        BurShape = item.BurShape,
                        BurOrderNumber = item.BurOrderNumber,
                        FileMaterial = item.FileMaterial,
                        FileShape = item.FileShape,
                        BrandName = brandname

                    });
                }
                return cartViewModels;
            }
            catch (Exception)
            {
               return new List<CartViewModel>();
            }
        }
    }
}
