using DentaEquip.BL.ValidationAttributes.Requests;
using DentaEquip.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.ViewModels.Requests
{
    public class RequestsViewModel
    {
       
        public string UserName { get; set; }
     
        public string Email { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "Length more than 4 Lettres")]
        public string Address { get; set; }
        [Required]
        [PhonePattern]
        public string PhoneNumber { get; set; }
        public decimal? TotalPrice { get; set; }
        public List<OrdersRequest> ordersRequests { get; set; }

        public string UserId { get; set; }
    }
}
