using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.DAL.Entities
{
    public class FinishedOrders:BaseModel
    {
      
        [Required]
        public decimal? TotalPrice { get; set; }
        public string AddressDetailes { get; set; }
       
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Phonenumber { get; set; }
        public string state { get; set; } = "Finished";
        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
      
        public virtual ICollection<OrdersCompelete> OrdersCompeletes { get; set; }
    }
}
