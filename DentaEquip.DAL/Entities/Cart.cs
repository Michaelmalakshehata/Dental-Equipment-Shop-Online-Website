using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.DAL.Entities
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }
        [Required]
        public string Ordername { get; set; }
        [Required]
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

        [Required]
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }

        [Required]
        public int Quantity { get; set; }
        public decimal? DiscountPrice
        {

            get
            {
                if (Discount > 0)
                {
                    var priceDiscount = (Price - (Price * (Discount / 100)));
                    return Math.Round((decimal)priceDiscount, 2);



                }
                return Price;
            }
        }
        public decimal? TotalPrice
        {

            get
            {
                if (Discount > 0)
                {
                    var priceDiscount = (Price - (Price * (Discount / 100)))* Quantity;
                    return Math.Round((decimal)priceDiscount,2);



                }
                return Quantity * Price;
            }
        }
        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
