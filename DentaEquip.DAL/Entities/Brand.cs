using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.DAL.Entities
{
    public class Brand:BaseModelSoftDelete
    {
        [Required]
        public string imgpath { get; set; }
        public virtual ICollection<Products> Products { get; set; }
    }
}
