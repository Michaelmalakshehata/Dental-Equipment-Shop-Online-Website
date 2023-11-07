using DentaEquip.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.IRepositories
{
    public interface IServiceFinishOrder
    {
        Task<List<FinishedOrders>> GetAllFinishedOrder();
        Task<List<FinishedOrders>> GetAllFinishedOrderByName(string name);
        List<FinishedOrders> Sort(string sort, List<FinishedOrders> requests);
        Task<List<OrdersCompelete>> ViewFinishedOrderDetailes(int FinishedId);
        Task<List<OrdersCompelete>> AllOrrdersCompeletes();
        List<OrdersCompelete> SearchOrdersCompeltes(string search, List<OrdersCompelete> ordersCompeletes);
       List<OrdersCompelete> SortOrdersCompeletes(string sort, List<OrdersCompelete> orders);
        Task<string> AddFinishedOrder(int id);
    }
}
