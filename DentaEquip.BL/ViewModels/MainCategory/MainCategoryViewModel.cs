using DentaEquip.BL.ValidationAttributes.MainCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.ViewModels.MainCategory
{
    public class MainCategoryViewModel
    {
        [Required]
        [UniqueCategoryName]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Name length between 2 and 30 Letters")]
        public string Name { get; set; }
    }
}
