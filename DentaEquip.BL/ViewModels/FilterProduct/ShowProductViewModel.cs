using DentaEquip.BL.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.ViewModels.FilterProduct
{
    public class ShowProductViewModel
    {
        public virtual CartViewModel CartViewModel { get; set; }
        public virtual FilterProductViewModel FilterProductViewModel { get; set; }
    }
}
