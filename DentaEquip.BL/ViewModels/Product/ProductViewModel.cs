using DentaEquip.BL.ValidationAttributes.Products;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.ViewModels.Product
{
    public class ProductViewModel
    {
        [Required]
        [UniqueProductName]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Name length between 2 and 20 Letters")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }


        public decimal? Discount { get; set; }
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Detailes length between 2 and 100 Letters")]
        public string Detailes { get; set; }
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Detailes length between 2 and 30 Letters")]
        public string Shade { get; set; }
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Detailes length between 2 and 30 Letters")]
        public string CureType { get; set; }
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Detailes length between 2 and 30 Letters")]
        public string CountryOfOrigin { get; set; }
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Detailes length between 2 and 30 Letters")]
        public string BurColorCode { get; set; }
        public int? BurMaximumRPM { get; set; }
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Detailes length between 2 and 30 Letters")]
        public string BurShape { get; set; }
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Detailes length between 2 and 30 Letters")]
        public string BurOrderNumber { get; set; }
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Detailes length between 2 and 30 Letters")]
        public string FileMaterial { get; set; }
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Detailes length between 2 and 30 Letters")]
        public string FileShape { get; set; }
        [Required]
        public IFormFile File { get; set; }
        public IFormFile File2 { get; set; }
        public IFormFile File3 { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public int? BrandId { get; set; }
    }
}
