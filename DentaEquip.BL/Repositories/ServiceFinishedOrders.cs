using DentaEquip.BL.IRepositories;
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
    public class ServiceFinishedOrders : IServiceFinishOrder
    {
        private readonly EntityContext context;
        public ServiceFinishedOrders( EntityContext context)
        {
            this.context = context;
        }
        public async Task<string> AddFinishedOrder(int id)
        {
            try
            {
                if (id == 0)
                {
                    return string.Empty;
                }
                var orders = await context.Requests.Where(o => o.Id == id).FirstOrDefaultAsync();
                var orderRequests = await context.OrdersRequests.Where(o => o.RequestsId == id).ToListAsync();
                if (orders is  null)
                {

                    return string.Empty;
                }
                FinishedOrders finishedOrders = new FinishedOrders()
                {
                    UserName = orders.UserName,
                    AddressDetailes = orders.AddressDetailes,
                    Email = orders.Email,
                    Phonenumber = orders.Phonenumber,
                    TotalPrice = orders.TotalPrice,
                    UserId = orders.UserId
                };
                if (finishedOrders is  null)
                {
                    return string.Empty;
                }
                var result = context.FinishedOrders.Add(finishedOrders);
                await context.SaveChangesAsync();

                if (result is not null)
                {
                    List<OrdersCompelete> ordersCompeletes = new List<OrdersCompelete>();
                    foreach (var item in orderRequests)
                    {
                        ordersCompeletes.Add(new OrdersCompelete
                        {
                            BrandName = item.BrandName,
                            CountryOfOrigin = item.CountryOfOrigin,
                            Ordername = item.Ordername,
                            PriceOrder = item.PriceOrder,
                            ProductId = item.ProductId,
                            TotalPriceOrder = item.TotalPriceOrder,
                            Quantity = item.Quantity,
                            imgpath = item.imgpath,
                            FinishedOdersId = finishedOrders.Id
                        });
                    }
                    await context.OrdersCompeletes.AddRangeAsync(ordersCompeletes);
                    context.OrdersRequests.RemoveRange(orderRequests);
                    context.Requests.Remove(orders);
                    await context.SaveChangesAsync();

                }
                return "Added";
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public async Task<List<OrdersCompelete>> AllOrrdersCompeletes()
        {
            try
            {
                return await context.OrdersCompeletes.AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                return new List<OrdersCompelete>();
            }
        }

        public async Task<List<FinishedOrders>> GetAllFinishedOrder()
        {
            try
            {
                return await context.FinishedOrders.AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                return new List<FinishedOrders>();
            }
        }

        public async Task<List<FinishedOrders>> GetAllFinishedOrderByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name) == true)
                {
                    return new List<FinishedOrders>();
                }
                var ordersFinished = await context.FinishedOrders.Where(o => o.UserName.Equals(name)).AsNoTracking().ToListAsync();
                foreach (var item in ordersFinished)
                {
                    var ordersDetails = await context.OrdersCompeletes.Where(o => o.FinishedOdersId == item.Id).AsNoTracking().ToListAsync();
                    item.OrdersCompeletes = ordersDetails;
                }
                return ordersFinished;
            }
            catch (Exception)
            {
                return new List<FinishedOrders>();
            }
        }

        public List<OrdersCompelete> SearchOrdersCompeltes(string search, List<OrdersCompelete> ordersCompeletes)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(search) == false && ordersCompeletes is not null && ordersCompeletes.Any())
                {
                    return ordersCompeletes.Where(o => o.Ordername.Contains(search)).ToList();
                }
                return new List<OrdersCompelete>();
            }
            catch (Exception)
            {
                return new List<OrdersCompelete>();
            }
        }

        public List<FinishedOrders> Sort(string sort, List<FinishedOrders> list)
        {
            try
            {
                if (list is not null && list.Any() && string.IsNullOrWhiteSpace(sort) == false)
                {
                    var result = sort switch
                    {
                        "SortNameA-Z" => list.OrderBy(o => o.UserName).ToList(),
                        "SortNameZ-A" => list.OrderByDescending(o => o.UserName).ToList(),
                        "SortNewFinished" => list.OrderByDescending(o => o.Date).ToList(),
                        "SortPriceHigh-Low" => list.OrderByDescending(o => o.TotalPrice).ToList(),
                        "SortPriceLow-High" => list.OrderBy(o => o.TotalPrice).ToList(),
                        _ => new List<FinishedOrders>()
                    };
                    return result;
                }
                return new List<FinishedOrders>();
            }
            catch (Exception)
            {
                return new List<FinishedOrders>();
            }
        }

        public List<OrdersCompelete> SortOrdersCompeletes(string sort, List<OrdersCompelete> orders)
        {
            try
            {
                if (orders is not null && orders.Any() && string.IsNullOrWhiteSpace(sort) == false)
                {
                    var result = sort switch
                    {
                        "SortHighPrice" => orders.OrderByDescending(o => o.TotalPriceOrder).ToList(),
                        "SortBrand" => orders.OrderBy(o => o.BrandName).ToList(),
                        "SortCountryOfOrigin" => orders.OrderBy(o => o.CountryOfOrigin).ToList(),
                        "SortProductCode" => orders.OrderBy(o => o.ProductId).ToList(),
                        "SortQuantity" => orders.OrderByDescending(o => o.Quantity).ToList(),
                        _ => new List<OrdersCompelete>()
                    };
                    return result;
                }
                return new List<OrdersCompelete>();
            }
            catch (Exception)
            {
                return new List<OrdersCompelete>();
            }
        }

        public async Task<List<OrdersCompelete>> ViewFinishedOrderDetailes(int FinishedId)
        {
            try
            {
                if (FinishedId > 0)
                {
                    var result = await context.OrdersCompeletes.Where(o => o.FinishedOdersId == FinishedId).Include(o=>o.FinishedOrders).AsNoTracking().ToListAsync();
                    return result;
                }
                return new List<OrdersCompelete>();
            }
            catch (Exception)
            {
                return new List<OrdersCompelete>();
            }
        }
    }
}
