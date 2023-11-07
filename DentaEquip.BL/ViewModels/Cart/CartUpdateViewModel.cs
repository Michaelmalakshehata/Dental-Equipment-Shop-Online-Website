using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.ViewModels.Cart
{
    public class CartUpdateViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Ordername { get; set; }
        public string imgpath { get; set; }
        public string Shade { get; set; }
        public string CureType { get; set; }
        public string CountryOfOrigin { get; set; }
        public string BurColorCode { get; set; }
        public int? BurMaximumRPM { get; set; }
        public string BurShape { get; set; }
        public string BurOrderNumber { get; set; }
        public string FileMaterial { get; set; }
        public string FileShape { get; set; }
        public string BrandName { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DiscountPrice
        {

            get
            {
                if (Discount > 0)
                {
                    return Math.Round((decimal)(Price - (Price * (Discount / 100))), 2);
                }
                return Price;
            }
        }

        [Required]
        public int Quantity { get; set; }
        public int ProductMaxQuantity { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public decimal? SubTotalPrice { get; set; }

    }
}
