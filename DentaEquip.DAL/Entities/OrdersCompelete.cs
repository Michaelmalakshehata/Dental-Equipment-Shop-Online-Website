using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.DAL.Entities
{
    public class OrdersCompelete
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Ordername { get; set; }
        [Required]
        public string imgpath { get; set; }
        public string CountryOfOrigin { get; set; }
    
        public string BrandName { get; set; }
        public int ProductId { get; set; }


        [Required]
        public decimal? PriceOrder { get; set; }

        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal? TotalPriceOrder { get; set; }
        [Required]
        public int FinishedOdersId { get; set; }
        public virtual FinishedOrders FinishedOrders { get; set; }
    }
}
