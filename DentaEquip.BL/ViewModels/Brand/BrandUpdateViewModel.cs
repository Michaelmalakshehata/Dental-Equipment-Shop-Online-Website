using DentaEquip.BL.ValidationAttributes.Brand;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.ViewModels.Brand
{
    public class BrandUpdateViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Name length between 2 and 30 Letters")]
        public string Name { get; set; }
        public string imagepath { set; get; }
        public IFormFile File { get; set; }
    }
}
