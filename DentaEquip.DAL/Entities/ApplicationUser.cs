using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Address { set; get; }
        public virtual ICollection<Requests> Requests { get; set; }
        public virtual ICollection<FinishedOrders> FinishedOrders { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<WishList> WishLists { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }

    }
}
