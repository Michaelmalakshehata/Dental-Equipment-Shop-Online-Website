using AspNetCoreHero.ToastNotification.Abstractions;
using DentaEquip.BL.IRepositories;
using DentaEquip.BL.Pagination;
using DentaEquip.BL.ViewModels.Requests;
using DentaEquip.DAL.Constant;
using DentaEquip.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentaEquip.Controllers
{
    [Authorize(Roles = Roles.userrole)]
    public class OrderController : Controller
    {
        private readonly IServiceRequests serviceOrder;
        private readonly IServiceFinishOrder serviceFinishedOrder;
        private readonly IServiceCart serviceCart;
        private readonly INotyfService notyf;

        private static List<OrdersRequest> ordersRequests;
        public OrderController(IServiceCart serviceCart, INotyfService notyf,IServiceRequests serviceOrder, IServiceFinishOrder serviceFinishedOrder)
        {
            this.serviceOrder = serviceOrder;
            this.serviceFinishedOrder = serviceFinishedOrder;
            this.serviceCart=serviceCart;
            this.notyf=notyf;
        }
        public async Task<IActionResult> AddOrder()
        {
            string name = User.Identity.Name;
            if (string.IsNullOrWhiteSpace(name) == false)
            {
                var list = await serviceOrder.CheckoutOrder(name);
                ordersRequests = await serviceOrder.GetAllCartOfUser(name);
                ViewBag.totalprice = await serviceCart.totalprice(name);
                list.ordersRequests = ordersRequests;
                return View(list);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrder(RequestsViewModel orderViewModel)
        {
            if (ModelState.IsValid)
            {
                orderViewModel.ordersRequests = ordersRequests;
               string result= await serviceOrder.AddOrder(orderViewModel);
                ordersRequests = null;
                if (string.IsNullOrWhiteSpace(result) == false)
                {
                    notyf.Success("Request Order Done", 10);
                    return RedirectToAction("ShowOrders");
                }
                else
                {
                    notyf.Error("Some Product Have Not Enough Quantity", 10);
                    
                    return RedirectToAction("AddOrder");
                }
            }
            return View(orderViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> ShowOrders(int pg = 1)
        {
            string name = User.Identity.Name;
            ViewBag.list = await serviceOrder.Order(name);
            var listorders = await serviceFinishedOrder.GetAllFinishedOrderByName(name);
            var data = Pagination<FinishedOrders>.GetPaginationData(pg, listorders,1);
            this.ViewBag.Pager = data.Item2;
            return View(data.Item1);
        }
    }
}
