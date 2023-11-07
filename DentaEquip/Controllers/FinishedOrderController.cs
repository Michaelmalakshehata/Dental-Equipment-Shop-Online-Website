using AspNetCoreHero.ToastNotification.Abstractions;
using DentaEquip.BL.IRepositories;
using DentaEquip.BL.Pagination;
using DentaEquip.BL.Search;
using DentaEquip.DAL.Constant;
using DentaEquip.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentaEquip.Controllers
{

    [Authorize(Roles = Roles.adminrole)]
    public class FinishedOrderController : Controller
    {
        private readonly IServiceFinishOrder serviceFinishedOrder;
        private readonly INotyfService notyf;
        private static List<FinishedOrders> AllFinishedList;
        private static List<OrdersCompelete> AllOrdersCompeletes;
        public FinishedOrderController( INotyfService notyf, IServiceFinishOrder serviceFinishedOrder)
        {
            this.serviceFinishedOrder = serviceFinishedOrder;
            this.notyf = notyf;
        }

        public async Task<IActionResult> Index(string clear, string sort, string search, int pg = 1)
        {

           
            if (string.IsNullOrWhiteSpace(clear) == false)
            {
                AllFinishedList = null;
            }
            if(AllFinishedList is null)
            {
                AllFinishedList = await serviceFinishedOrder.GetAllFinishedOrder();
            }
            if (string.IsNullOrWhiteSpace(sort) == false || string.IsNullOrWhiteSpace(search) == false && AllFinishedList is not null)
            {
                if (string.IsNullOrWhiteSpace(search) == false)
                {
                    AllFinishedList = search<FinishedOrders>.SearchByName(search, AllFinishedList);
                }
                else
                {
                   
                        AllFinishedList = serviceFinishedOrder.Sort(sort, AllFinishedList);
                    
                 
                }
            }
            if (AllFinishedList is not null && AllFinishedList.Any())
            {
                var orders = Pagination<FinishedOrders>.GetPaginationData(pg, AllFinishedList,4);
                this.ViewBag.Pager = orders.Item2;
                return View(orders.Item1);
            }
         
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AddFinishedOrder(int Id)
        {
           string result= await serviceFinishedOrder.AddFinishedOrder(Id);
            if(string.IsNullOrWhiteSpace(result) == false)
            {
                notyf.Success("Request Accept Done",10);
                RequestOrderController.ResetRequestsAccepts();
            }
            return RedirectToAction("Index", "RequestOrder");
        }
        [HttpGet]
        public async Task<IActionResult> ViewOrderDetails(int IdFinished)
        {
            var result = await serviceFinishedOrder.ViewFinishedOrderDetailes(IdFinished);

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> OrdersCompelete(string clear, string sort, string search, int pg = 1)
        {

           
            if (string.IsNullOrWhiteSpace(clear) == false)
            {
                AllOrdersCompeletes = null;
            }
            if(AllOrdersCompeletes is null)
            {
                AllOrdersCompeletes = await serviceFinishedOrder.AllOrrdersCompeletes();
            }
            if (string.IsNullOrWhiteSpace(sort) == false || string.IsNullOrWhiteSpace(search) == false && AllOrdersCompeletes is not null)
            {
                if (string.IsNullOrWhiteSpace(search) == false)
                {
                    AllOrdersCompeletes = serviceFinishedOrder.SearchOrdersCompeltes(search, AllOrdersCompeletes);
                }
                else
                {
                    AllOrdersCompeletes = serviceFinishedOrder.SortOrdersCompeletes(sort, AllOrdersCompeletes);
                   
                }
            }
            if (AllOrdersCompeletes is not null && AllOrdersCompeletes.Any())
            {
                var orders = Pagination<OrdersCompelete>.GetPaginationData(pg, AllOrdersCompeletes,3);
                this.ViewBag.Pager = orders.Item2;
                return View(orders.Item1);
            }
            
            return View();
        }

    }
}
