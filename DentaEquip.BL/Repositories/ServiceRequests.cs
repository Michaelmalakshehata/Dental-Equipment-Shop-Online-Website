using DentaEquip.BL.IRepositories;
using DentaEquip.BL.ViewModels.Cart;
using DentaEquip.BL.ViewModels.Requests;
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
    public class ServiceRequests : IServiceRequests
    {
        private readonly IServiceCart serviceCart;
        private readonly EntityContext context;
        public ServiceRequests(IServiceCart serviceCart, EntityContext context)
        {
            this.serviceCart = serviceCart;
            this.context = context;
        }

        public async Task<string> AddOrder(RequestsViewModel order)
        {
            try
            {
                if (order is  null || order.ordersRequests is  null || order.ordersRequests.Any()==false)
                {
                    return string.Empty;
                }
                foreach (var itm in order.ordersRequests)
                {
                    int productQuantity = await context.Product.Where(o => o.Id == itm.ProductId && o.IsDeleted == false).AsNoTracking().Select(o => o.Quantity).FirstOrDefaultAsync();

                    if (itm.Quantity > productQuantity)
                    {
                        return string.Empty;
                    }
                }
                Requests orders = new Requests()
                {
                    AddressDetailes = order.Address,
                    Email = order.Email,
                    UserName = order.UserName,
                    Phonenumber = order.PhoneNumber,
                    TotalPrice = order.TotalPrice,
                    UserId = order.UserId,
                };
                var requests = await context.AddAsync(orders);
                await context.SaveChangesAsync();

                if (requests is not null)
                {
                    foreach (var item in order.ordersRequests)
                    {
                        var product = await context.Product.Where(o => o.Id == item.ProductId && o.IsDeleted == false).FirstOrDefaultAsync();

                        item.RequestsId = orders.Id;
                        product.Quantity = product.Quantity - item.Quantity;
                        context.Product.Update(product);

                    }
                    await context.OrdersRequests.AddRangeAsync(order.ordersRequests);
                    await context.SaveChangesAsync();
                    await serviceCart.DeleteAllCartItems(order.UserName);
                    return "Added";
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }

        public async Task<List<Requests>> AllOrders()
        {
            try
            {
                return await context.Requests.AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                return new List<Requests>();
            }

        }

        public async Task<RequestsViewModel> CheckoutOrder(string Name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Name) == true)
                {
                    return new RequestsViewModel();
                }
                string userid = await context.Users.Where(u => u.UserName.Equals(Name)).Select(u => u.Id).AsNoTracking().FirstOrDefaultAsync();
                if (userid is not null)
                {

                    var personalinfo = await context.Users.Where(u => u.Id == userid).Select(u => new { u.Address, u.Email }).AsNoTracking().FirstOrDefaultAsync();
                    RequestsViewModel orderViewModel = new RequestsViewModel()
                    {
                        UserId = userid,
                        Address = personalinfo.Address,
                        Email = personalinfo.Email,
                        UserName = Name
                    };
                    return orderViewModel;
                }
                return new RequestsViewModel();
            }
            catch (Exception)
            {
                return new RequestsViewModel();
            }
        }

        public async Task<string> DeleteOrder(int id)
        {
            try
            {
                if(id==0)
                {
                    return string.Empty;
                }
                var orders = await context.Requests.Where(o => o.Id == id).AsNoTracking().FirstOrDefaultAsync();
                var orderRequest = await context.OrdersRequests.Where(o => o.RequestsId == id).AsNoTracking().ToListAsync();
                if (orders is  null || orderRequest is  null || orderRequest.Any()==false)
                {
                    return string.Empty;
                }
                foreach (var item in orderRequest)
                {
                    var product = await context.Product.Where(o => o.IsDeleted == false && o.Id == item.ProductId).FirstOrDefaultAsync();
                    if (product is not null)
                    {
                        product.Quantity = product.Quantity + item.Quantity;
                        context.Product.Update(product);
                    }

                }
                context.OrdersRequests.RemoveRange(orderRequest);
                var result = context.Requests.Remove(orders);
                await context.SaveChangesAsync();
                if (result is not null)
                {
                    return "Deleted";
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public async Task<List<OrdersRequest>> GetAllCartOfUser(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) == true)
                {
                    return new List<OrdersRequest>();
                }
                var userId = await context.Users.Where(o => o.UserName.Equals(username)).AsNoTracking().Select(o => o.Id).FirstOrDefaultAsync();
                var carts = await context.Cart.Where(o => o.UserId.Equals(userId)).AsNoTracking().ToListAsync();
                if (carts is  null || carts.Any()==false)
                {
                    return new List<OrdersRequest>();
                }
                List<OrdersRequest> ordersRequests = new List<OrdersRequest>();
                foreach (var item in carts)
                {
                    var productFound = await context.Product.Where(o => o.Id == item.ProductId).Where(o => o.IsDeleted == true || item.Quantity > o.Quantity).AsNoTracking().FirstOrDefaultAsync();

                    if (productFound is not null)
                    {
                        context.Cart.Remove(item);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        ordersRequests.Add(new OrdersRequest
                        {
                            ProductId = item.ProductId,
                            Ordername = item.Ordername,
                            PriceOrder = item.DiscountPrice,
                            imgpath = item.imgpath,

                            Quantity = item.Quantity,

                            CountryOfOrigin = item.CountryOfOrigin,

                            BrandName = item.BrandName,
                            TotalPriceOrder = item.TotalPrice

                        });
                    }

                }
                return ordersRequests;
            }
            catch (Exception)
            {
                return new List<OrdersRequest>();
            }
        }

        public async Task<Requests> Order(string Name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Name) == false)
                {
                    return await context.Requests.Where(o => o.UserName.Equals(Name)).AsNoTracking().FirstOrDefaultAsync();
                }
                return new Requests();
            }
            catch (Exception)
            {
                return new Requests();
            }
        }

        public List<Requests> Sort(string sort, List<Requests> list)
        {
            try
            {
                if(list is not null && list.Any() && string.IsNullOrWhiteSpace(sort) == false)
                {
                    var result = sort switch
                    {
                        "SortNameA-Z" => list.OrderBy(o => o.UserName).ToList(),
                        "SortNameZ-A" => list.OrderByDescending(o => o.UserName).ToList(),
                        "SortNewRequests" => list.OrderByDescending(o => o.Date).ToList(),
                        "SortPriceHigh-Low" => list.OrderByDescending(o => o.TotalPrice).ToList(),
                        "SortPriceLow-High" => list.OrderBy(o => o.TotalPrice).ToList(),
                        _ => new List<Requests>()
                    };
                    return result;
                }
                return new List<Requests>();
            }
            catch (Exception)
            {
                return new List<Requests>();
            }
        }

        public async Task<List<OrdersRequest>> ViewOrderDetail(int id)
        {
            try
            {
                if (id > 0)
                {
                    var listorders = await context.OrdersRequests.Where(o => o.RequestsId == id).Include(e=>e.Requests).AsNoTracking().ToListAsync();
                    return listorders;
                }
                return new List<OrdersRequest>();
            }
            catch (Exception)
            {
                return new List<OrdersRequest>();
            }
        }
    }
}
