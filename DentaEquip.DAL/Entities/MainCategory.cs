using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.DAL.Entities
{
    public class MainCategory:BaseModelSoftDelete
    {
        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }
}
