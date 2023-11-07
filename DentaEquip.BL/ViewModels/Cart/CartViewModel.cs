using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.ViewModels.Cart
{
    public class CartViewModel
    {
        public int SumRating { get; set; }
        public int CountRating { get; set; }
        public int Rating
        {
            get
            {
                if (SumRating > 0 && CountRating > 0)
                {
                  int result=SumRating/CountRating;
                    var convert=Convert.ToInt32(result);
                    return convert;

                }
                return 0;
            }
        }
        public DateTime CreateDate { get; set; }
        public int BestSelling { get; set; }
        public int WishlistId { get; set; }
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

        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

    }
}
