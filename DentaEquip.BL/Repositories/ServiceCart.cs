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
    public class ServiceCart : IServiceCart
    {
        private readonly EntityContext context;
        public ServiceCart(EntityContext context)
        {
            this.context = context;
        }

        public async Task<string> AddToCart(CartViewModel cartViewModel, string Name)
        {
            try
            {
                if (cartViewModel is  null || string.IsNullOrWhiteSpace(Name) == true)
                {
                    return string.Empty;
                }
                var maximumCartItems = await GetCartCount(Name);
                if (maximumCartItems > 9)
                {
                    return "ReachMaximumCartItem";
                }
                var quantity = await context.Product.Where(o => o.Id == cartViewModel.ProductId).AsNoTracking().Select(o => o.Quantity).FirstOrDefaultAsync();

                if (quantity < cartViewModel.Quantity)
                {
                    return string.Empty;
                }
                Cart existincart = await context.Cart.Where(o => o.ProductId == cartViewModel.ProductId).FirstOrDefaultAsync();
                if (existincart is not null)
                {
                    if (cartViewModel.Quantity + existincart.Quantity <= quantity)
                    {
                        existincart.Quantity = cartViewModel.Quantity + existincart.Quantity;
                        context.Cart.Update(existincart);
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    string userid = await context.Users.Where(u => u.UserName.Equals(Name)).Select(u => u.Id).AsNoTracking().FirstOrDefaultAsync();
                    Cart cart = new Cart()
                    {
                        ProductId = cartViewModel.ProductId,
                        Ordername = cartViewModel.Ordername,
                        Price = cartViewModel.Price,
                        imgpath = cartViewModel.imgpath,
                        Quantity = cartViewModel.Quantity,
                        Discount = cartViewModel.Discount,
                        UserId = userid,
                        Shade = cartViewModel.Shade,
                        CureType = cartViewModel.CureType,
                        CountryOfOrigin = cartViewModel.CountryOfOrigin,
                        BurColorCode = cartViewModel.BurColorCode,
                        BurMaximumRPM = cartViewModel.BurMaximumRPM,
                        BurShape = cartViewModel.BurShape,
                        BurOrderNumber = cartViewModel.BurOrderNumber,
                        FileMaterial = cartViewModel.FileMaterial,
                        FileShape = cartViewModel.FileShape,
                        BrandName = cartViewModel.BrandName
                    };
                    if (cart is not null)
                    {
                        var result = await context.AddAsync(cart);
                        await context.SaveChangesAsync();
                    }

                }
                return "added";
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public async Task<string> DeleteAllCartItems(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name) == true)
                {
                  return string.Empty;
                }
                var id = context.Users.Where(o => o.UserName == name).Select(o => o.Id).FirstOrDefault();
                if (id is  null)
                {
                  return string.Empty;
                }
                var listcart = context.Cart.Where(o => o.UserId == id).ToList();
                if (listcart is not null)
                {
                    context.Cart.RemoveRange(listcart);
                    await context.SaveChangesAsync();
                    return "deletedAll";
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public async Task<string> DeleteCartItem(int id, string name)
        {
            try
            {
                if (id == 0 || string.IsNullOrWhiteSpace(name) == true)
                {
                  return string.Empty;
                }
                var result = context.Cart.Where(o => o.Id == id).FirstOrDefault();
                if (result is not null)
                {
                    context.Remove(result);
                    await context.SaveChangesAsync();
                    return "deleted";
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


        public async Task<List<CartUpdateViewModel>> GetAllUserCart(string Name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Name) == true)
                {
                    return new List<CartUpdateViewModel>();
                }
                string userid = await context.Users.Where(u => u.UserName == Name).Select(u => u.Id).AsNoTracking().FirstOrDefaultAsync();
                if (userid is  null)
                {
                    return new List<CartUpdateViewModel>();
                }
                List<Cart> carts = await context.Cart.Where(o => o.UserId == userid).AsNoTracking().ToListAsync();
                if (carts is  null)
                {
                    return new List<CartUpdateViewModel>();
                }
                List<CartUpdateViewModel> cart = new List<CartUpdateViewModel>();

                foreach (var item in carts)
                {
                    var productFound = await context.Product.Where(o => o.Id == item.ProductId).Select(o => new {o.Quantity,o.IsDeleted}).AsNoTracking().FirstOrDefaultAsync();
                    if (productFound.Quantity==0||productFound.IsDeleted==true)
                    {
                        await DeleteCartItem(item.Id, Name);
                        continue;
                    }
                    else
                    {
                        var productMaxQuantity = await context.Product.Where(o => o.Id == item.ProductId).AsNoTracking().Select(o => o.Quantity).FirstOrDefaultAsync();
                        cart.Add(new CartUpdateViewModel
                        {
                            Id = item.Id,
                            ProductId = item.ProductId,
                            UserId = userid,
                            Quantity = item.Quantity,
                            ProductMaxQuantity = productMaxQuantity,
                            imgpath = item.imgpath,
                            Price = item.Price,
                            Discount = item.Discount,
                            Ordername = item.Ordername,
                            SubTotalPrice = item.TotalPrice,
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
                return cart;
            }
            catch (Exception)
            {
                return new List<CartUpdateViewModel>(); ;
            }
        }

        public async Task<int> GetCartCount(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name) == false)
                {
                    var id = await context.Users.Where(o => o.UserName == name).Select(o => o.Id).AsNoTracking().FirstOrDefaultAsync();
                    int numberCart = await context.Cart.Where(o => o.UserId == id).AsNoTracking().CountAsync();
                    return numberCart;
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<decimal?> totalprice(string name)
        {
            try
            {
                if (name is  null)
                {
                    return 0;
                }
                string userid = await context.Users.Where(u => u.UserName.Equals(name)).Select(u => u.Id).AsNoTracking().FirstOrDefaultAsync();
                if (userid is  null)
                {
                    return 0;
                }
                List<decimal?> carts = await context.Cart.Where(o => o.UserId.Equals(userid)).AsNoTracking().Select(o => o.TotalPrice).ToListAsync();
                if (carts is not null && carts.Count > 0)
                {
                    decimal? totalprice = 0;
                    foreach (var c in carts)
                    {
                        totalprice += c;
                    }
                    return totalprice;
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<string> UpdateCartItem(CartUpdateViewModel item)
        {
            try
            {
                if (item is  null)
                {
                    return string.Empty;
                }
                var quantity = await context.Product.Where(o => o.Id == item.ProductId).AsNoTracking().Select(o => o.Quantity).FirstOrDefaultAsync();

                if (quantity >= item.Quantity)
                {
                    Cart cart = new Cart()
                    {

                        Id = item.Id,
                        ProductId = item.ProductId,
                        UserId = item.UserId,
                        Quantity = item.Quantity,
                        imgpath = item.imgpath,
                        Price = item.Price,
                        Discount = item.Discount,
                        Ordername = item.Ordername,
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
                    };
                    if (cart is  null)
                    {
                        return string.Empty;
                    }
                    var result = context.Update(cart);
                    await context.SaveChangesAsync();
                    if (result is not null)
                    {
                        return "updated";
                    }

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
