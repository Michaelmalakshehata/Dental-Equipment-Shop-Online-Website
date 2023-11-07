using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.DAL.Entities
{
    public class SubCategory : BaseModelSoftDelete
    {
        public virtual ICollection<Products> Products { get; set; }
        [Required]
        public string Type {  get; set;}
        [Required]
        public int MainCategoryId { get; set; }
        public virtual  MainCategory MainCategory { get; set; }
    }
}
