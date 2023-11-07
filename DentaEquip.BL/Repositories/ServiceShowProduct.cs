using DentaEquip.BL.IRepositories;
using DentaEquip.BL.ViewModels.Cart;
using DentaEquip.BL.ViewModels.Search;
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
    public class ServiceShowProduct : IServiceShowProduct
    {
        private readonly EntityContext context;
        public ServiceShowProduct( EntityContext context)
        {
            this.context = context;
        }
        public async Task<List<CartViewModel>> GetAllProduct()
        {
            try
            {
                List<Products> list = await context.Product.Where(e => e.IsDeleted == false && e.Quantity > 0).Include(e=>e.Brand).AsNoTracking().ToListAsync();
                if (list is  null || list.Any()==false)
                {
                    return new List<CartViewModel>();
                }
                List<CartViewModel> cart = new List<CartViewModel>();

                foreach (var item in list)
                {
                    var sumRaing = await context.Comment.Where(o => o.ProductId == item.Id).AsNoTracking().Select(o => o.Rating).SumAsync();
                    var countRating = await context.Comment.Where(o => o.ProductId == item.Id).AsNoTracking().CountAsync();
                    var sumQuantity = await context.OrdersCompeletes.Where(o => o.ProductId == item.Id).AsNoTracking().Select(o => o.Quantity).SumAsync();
                    var sumCountProduct = await context.OrdersCompeletes.Where(o => o.ProductId == item.Id).AsNoTracking().CountAsync();
                    string brandName = null;
                    if (item.Brand is not null)
                    {
                        brandName = item.Brand.Name;
                    }
                    cart.Add(new CartViewModel
                    {
                        BestSelling = sumQuantity * sumCountProduct,
                        CreateDate = item.CreateDate,
                        SumRating = sumRaing,
                        CountRating = countRating,
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
                        BrandName = brandName
                    });
                }
                return cart;
                
            }
            catch (Exception)
            {
                return new List<CartViewModel>();
            }
        }
        public async Task<Products> GetByid(int id)
        {
            try
            {
                if (id == 0)
                {
                    return new Products();
                }
                var result = await context.Product.
              Where(o => o.IsDeleted == false && o.Id == id && o.Quantity > 0)
              .Include(o => o.Brand).AsNoTracking()
              .FirstOrDefaultAsync();

                if (result is not null)
                {

                    return result;
                }
                return new Products();
            }
            catch (Exception)
            {
                return new Products();
            }
        }

        public async Task<List<CartViewModel>> GetProductByBrand(int id)
        {
            try
            {
                if (id == 0)
                {
                    return new List<CartViewModel>();
                }
                List<Products> list = await context.Product.Where(e => e.IsDeleted == false && e.Quantity > 0).Where(s => s.BrandId == id).Include(e => e.Brand).AsNoTracking().ToListAsync();
                if (list is  null || list.Any()==false)
                {
                    return new List<CartViewModel>();
                }
                List<CartViewModel> cart = new List<CartViewModel>();

                foreach (var item in list)
                {
                    var sumRaing = await context.Comment.Where(o => o.ProductId == item.Id).AsNoTracking().Select(o => o.Rating).SumAsync();
                    var countRating = await context.Comment.Where(o => o.ProductId == item.Id).AsNoTracking().CountAsync();
                    var sumQuantity = await context.OrdersCompeletes.Where(o => o.ProductId == item.Id).AsNoTracking().Select(o => o.Quantity).SumAsync();
                    var sumCountProduct = await context.OrdersCompeletes.Where(o => o.ProductId == item.Id).AsNoTracking().CountAsync();
                    string brandName = null;
                    if (item.Brand is not null)
                    {
                        brandName = item.Brand.Name;
                    }
                    cart.Add(new CartViewModel
                    {
                        BestSelling = sumQuantity * sumCountProduct,
                        CreateDate = item.CreateDate,
                        SumRating = sumRaing,
                        CountRating = countRating,
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
                        BrandName = brandName
                    });
                }
                return cart;
            }
            catch (Exception)
            {
                return new List<CartViewModel>();
            }
        }

        public async Task<List<CartViewModel>> GetProductByCategory(int id)
        {
            try
            {
                if (id == 0)
                {
                    return new List<CartViewModel>();
                }
                List<Products> list = await context.Product.Where(e => e.IsDeleted == false && e.Quantity > 0).Where(s => s.SubCategoryId == id).Include(e => e.Brand).AsNoTracking().ToListAsync();
                if (list is  null || list.Any()==false)
                {
                    return new List<CartViewModel>();
                }
                List<CartViewModel> cart = new List<CartViewModel>();

                foreach (var item in list)
                {
                    var sumRaing = await context.Comment.Where(o => o.ProductId == item.Id).AsNoTracking().Select(o => o.Rating).SumAsync();
                    var countRating = await context.Comment.Where(o => o.ProductId == item.Id).AsNoTracking().CountAsync();
                    var sumQuantity = await context.OrdersCompeletes.Where(o => o.ProductId == item.Id).AsNoTracking().Select(o => o.Quantity).SumAsync();
                    var sumCountProduct = await context.OrdersCompeletes.Where(o => o.ProductId == item.Id).AsNoTracking().CountAsync();
                    string brandName = null;
                    if (item.Brand is not null)
                    {
                        brandName = item.Brand.Name;
                    }
                    cart.Add(new CartViewModel
                    {
                        BestSelling = sumQuantity * sumCountProduct,
                        CreateDate = item.CreateDate,
                        SumRating = sumRaing,
                        CountRating = countRating,
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
                        BrandName = brandName
                    });
                }
                return cart;
            }
            catch (Exception)
            {
                return new List<CartViewModel>();
            }
        }

        public async Task<List<CartViewModel>> SearchProduct(SearchViewModel searchViewModel)
        {
            try
            {
                if (searchViewModel is  null)
                {
                    return new List<CartViewModel>();
                }
                List<Products> list = await context.Product.Where(o => o.IsDeleted == false && o.Quantity > 0).Where(o => o.Name.Contains(searchViewModel.name) || o.Detailes.Contains(searchViewModel.name)).Include(e => e.Brand).AsNoTracking().ToListAsync();
                if (list is  null || list.Any()==false)
                {
                    return new List<CartViewModel>();
                }
                List<CartViewModel> cart = new List<CartViewModel>();

                foreach (var item in list)
                {
                    var sumRaing = await context.Comment.Where(o => o.ProductId == item.Id).AsNoTracking().Select(o => o.Rating).SumAsync();
                    var countRating = await context.Comment.Where(o => o.ProductId == item.Id).AsNoTracking().CountAsync();
                    var sumQuantity = await context.OrdersCompeletes.Where(o => o.ProductId == item.Id).AsNoTracking().Select(o => o.Quantity).SumAsync();
                    var sumCountProduct = await context.OrdersCompeletes.Where(o => o.ProductId == item.Id).AsNoTracking().CountAsync();
                    string brandName = null;
                    if (item.Brand is not null)
                    {
                        brandName = item.Brand.Name;
                    }
                    cart.Add(new CartViewModel
                    {
                        BestSelling = sumQuantity * sumCountProduct,
                        CreateDate = item.CreateDate,
                        SumRating = sumRaing,
                        CountRating = countRating,
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
                        BrandName = brandName
                    });
                }
                return cart;
            }
            catch (Exception)
            {
                return new List<CartViewModel>();
            }

        }

    }
}
