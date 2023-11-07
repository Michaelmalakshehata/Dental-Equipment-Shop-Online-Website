using DentaEquip.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.ViewModels.SubCategory
{
    public class ShowSubCategoryModels : BaseModelSoftDelete
    {
       
        public string MainCategoryName { get; set; }
        public string Type { get; set; }
    }
}
