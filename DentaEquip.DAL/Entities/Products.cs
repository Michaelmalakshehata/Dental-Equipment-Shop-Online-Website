using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.DAL.Entities
{
    public class Products:BaseModelSoftDelete
    {
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string imgpath { get; set; }
        public string imgpath2 { get; set; }
        public string imgpath3 { get; set; }
        public string Shade { get; set; }
        public string CureType { get; set; }
        public string CountryOfOrigin { get; set; }
        public string BurColorCode { get; set; }
        public int? BurMaximumRPM { get; set; }
        public string BurShape { get; set; }
        public string BurOrderNumber { get; set; }
        public string FileMaterial { get; set; }
        public string FileShape { get; set; }
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
        public string Detailes { get; set; }
        [Required]
        public int SubCategoryId { get; set; }
        public virtual SubCategory SubCategories { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public int? BrandId { get; set; }
        public virtual Brand Brand { get; set; }
    }
}
