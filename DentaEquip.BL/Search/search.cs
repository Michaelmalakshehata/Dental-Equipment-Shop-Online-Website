using DentaEquip.BL.Pagination;
using DentaEquip.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.Search
{
    public class search<T> where T : BaseModel
    {
        public static List<T> SearchByName( string searchName, List<T> dataList)
        {
            try
            {
                var data = dataList.Where(o => o.UserName.Contains(searchName)).ToList();
                return data;
            }
            catch
            {
                throw;
            }
        }
    }
}
