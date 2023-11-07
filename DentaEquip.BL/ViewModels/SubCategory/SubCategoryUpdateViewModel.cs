using DentaEquip.BL.ValidationAttributes.SubCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.ViewModels.SubCategory
{
    public class SubCategoryUpdateViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Name length between 2 and 30 Letters")]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public int MainCategoryId { get; set; }
    }
}
