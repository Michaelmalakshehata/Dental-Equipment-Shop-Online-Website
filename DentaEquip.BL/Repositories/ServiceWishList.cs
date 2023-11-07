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
    public class ServiceWishList : IServiceWishList
    {
        private readonly EntityContext context;
        public ServiceWishList(EntityContext context)
        {
            this.context = context;
        }
        public async Task<string> AddToWishList(int Id, string name)
        {
            try
            {
                if (Id == 0 || string.IsNullOrWhiteSpace(name) == true)
                {
                  return string.Empty;
                }
                var maximumWishlistItem = await GetWishListCount(name);
                if(maximumWishlistItem > 19)
                {
                    return "ReachmaximumWishlistItem";
                }
                var product = await context.Product.Where(o => o.IsDeleted == false && o.Quantity > 0).Where(o => o.Id == Id).Include(e => e.Brand).AsNoTracking().FirstOrDefaultAsync();
                if (product is  null)
                {
                   return string.Empty;
                }
                string existinwishlist = await context.WishList.Where(o => o.ProductId == Id).Select(o => o.Ordername).AsNoTracking().FirstOrDefaultAsync();
                if (existinwishlist is null)
                {
                    string userid = await context.Users.Where(o => o.UserName.Equals(name)).Select(o => o.Id).AsNoTracking().FirstOrDefaultAsync();
                    if (userid is  null)
                    {
                       return string.Empty;
                    }
                    string brandName = null;
                    if (product.Brand is not null)
                    {
                        brandName = product.Brand.Name;
                    }
                    WishList wishList = new WishList()
                    {
                        ProductId = product.Id,
                        Ordername = product.Name,
                        Price = product.Price,

                        imgpath = product.imgpath,
                        Discount = product.Discount,
                        Shade = product.Shade,
                        CureType = product.CureType,
                        CountryOfOrigin = product.CountryOfOrigin,
                        BurColorCode = product.BurColorCode,
                        BurMaximumRPM = product.BurMaximumRPM,
                        BurShape = product.BurShape,
                        BurOrderNumber = product.BurOrderNumber,
                        FileMaterial = product.FileMaterial,
                        FileShape = product.FileShape,
                        BrandName = brandName,
                        UserId = userid
                    };
                    if (wishList is not null)
                    {
                        await context.AddAsync(wishList);
                        await context.SaveChangesAsync();
                    }

                    await GetWishListCount(name);
                    return "added";

                }
                if(existinwishlist is not null)
                {
                    return "added";
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public async Task<string> DeleteAllWishListItems(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name) == true)
                {
                    return string.Empty;
                }
                var id = await context.Users.Where(o => o.UserName.Equals(name)).Select(o => o.Id).AsNoTracking().FirstOrDefaultAsync();
                if (id is  null)
                {
                   return string.Empty;
                }
                var listwishlist = await context.WishList.Where(o => o.UserId == id).ToListAsync();
                if (listwishlist is not null && listwishlist.Count > 0)
                {
                    context.WishList.RemoveRange(listwishlist);
                    await context.SaveChangesAsync();
                    await GetWishListCount(name);
                    return "deletedAll";
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public async Task<string> DeleteWishListItem(int id, string name)
        {
            try
            {
                if (id == 0 || string.IsNullOrWhiteSpace(name) == true)
                {
                    return string.Empty;
                }
                var result = await context.WishList.Where(o => o.Id == id).FirstOrDefaultAsync();
                if (result is not null)
                {
                    context.WishList.Remove(result);
                    await context.SaveChangesAsync();
                    await GetWishListCount(name);
                    return "deleted";
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public async Task<List<CartViewModel>> GetAllUserWishList(string Name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Name) == true)
                {
                    return new List<CartViewModel>();
                }
                string userid = await context.Users.Where(u => u.UserName.Equals(Name)).Select(u => u.Id).AsNoTracking().FirstOrDefaultAsync();
                if (userid is null)
                {
                    return new List<CartViewModel>();
                }
                List<WishList> wishLists = await context.WishList.Where(o => o.UserId.Equals(userid)).AsNoTracking().ToListAsync();

                if (wishLists is  null || wishLists.Count == 0)
                {
                    return new List<CartViewModel>();
                }
                List<CartViewModel> cartList = new List<CartViewModel>();
                foreach (var item in wishLists)
                {
                    var productFound = await context.Product.Where(o => o.Id == item.ProductId).Select(o=>new {o.Quantity ,o.IsDeleted}).AsNoTracking().FirstOrDefaultAsync();

                    if (productFound.IsDeleted==true || productFound.Quantity==0)
                    {
                        await DeleteWishListItem(item.Id, Name);
                        continue;
                    }
                    else
                    {
                        var QuantityMax = await context.Product.Where(o => o.IsDeleted == false && o.Quantity > 0).AsNoTracking().Select(o => o.Quantity).FirstOrDefaultAsync();
                        cartList.Add(new CartViewModel
                        {
                            WishlistId = item.Id,
                            ProductId = item.ProductId,
                            Ordername = item.Ordername,
                            Price = item.Price,
                            Quantity = QuantityMax,
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
                            BrandName = item.BrandName
                        });
                    }

                }
                return cartList;
            }
            catch (Exception)
            {
                return new List<CartViewModel>();
            }
        }

        public async Task<int> GetWishListCount(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name) == false)
                {
                    var id = await context.Users.Where(o => o.UserName.Equals(name)).AsNoTracking().Select(o => o.Id).FirstOrDefaultAsync();
                    int number = await context.WishList.Where(o => o.UserId.Equals(id)).AsNoTracking().CountAsync();
                    return number;
                }
                return 0;
            }
            catch(Exception)
            {
                return 0;
            }
        }
    }
}
