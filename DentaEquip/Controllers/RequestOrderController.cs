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
    public class RequestOrderController : Controller
    {
        private readonly IServiceRequests serviceOrder;
        private readonly INotyfService notyf;
        static List<Requests> AllRequestList;

        public RequestOrderController(INotyfService notyf, IServiceRequests serviceOrder)
        {
            this.serviceOrder = serviceOrder;
            this.notyf=notyf;
        }
        public async Task<IActionResult> Index(string clear,string sort,string search,int pg = 1)
        {

           
            if(string.IsNullOrWhiteSpace(clear) == false)
            {
                AllRequestList = null;
            }
            if(AllRequestList is null)
            {
                AllRequestList = await serviceOrder.AllOrders();
            }
            if(string.IsNullOrWhiteSpace(sort) == false || string.IsNullOrWhiteSpace(search) == false&& AllRequestList is not null)
            {
                if(string.IsNullOrWhiteSpace(search) == false)
                {
                    AllRequestList = search<Requests>.SearchByName(search, AllRequestList);
                }
                else
                {
                  
                        AllRequestList = serviceOrder.Sort(sort, AllRequestList);
                   
                }
            }
            if(AllRequestList is not null && AllRequestList.Any())
            {
                var orders = Pagination<Requests>.GetPaginationData(pg, AllRequestList,4);
                this.ViewBag.Pager = orders.Item2;
                return View(orders.Item1);
            }
       
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DeleteOrder(int Id)
        {
           string result= await serviceOrder.DeleteOrder(Id);
            if(string.IsNullOrWhiteSpace(result) == false)
            {
                notyf.Success("Delete Request Order Done", 10);
                AllRequestList = null;
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> ViewOrderDetails(int IdRequest)
        {
                var result = await serviceOrder.ViewOrderDetail(IdRequest);

                return View(result);
        }
        public static void ResetRequestsAccepts()
        {
            AllRequestList = null;
        }
       
    }
}
