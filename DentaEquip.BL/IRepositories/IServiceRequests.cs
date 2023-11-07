using DentaEquip.BL.ViewModels.Cart;
using DentaEquip.BL.ViewModels.Requests;
using DentaEquip.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.IRepositories
{
    public interface IServiceRequests
    {
        Task<RequestsViewModel> CheckoutOrder(string name);
        Task<List<OrdersRequest>> GetAllCartOfUser(string username);
        Task<string> AddOrder(RequestsViewModel order);
        Task<Requests> Order(string name);
        Task<List<Requests>> AllOrders();
        Task<string> DeleteOrder(int id);
        List<Requests> Sort(string sort, List<Requests> requests);
        Task<List<OrdersRequest>> ViewOrderDetail(int id);
    }
}
