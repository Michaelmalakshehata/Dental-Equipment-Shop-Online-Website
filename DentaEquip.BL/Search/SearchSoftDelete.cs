using DentaEquip.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.Search
{
    public class SearchSoftDelete<T> where T : BaseModelSoftDelete
    {
        public static List<T> SearchByName( List<T> dataList, string searchName,bool IsDelete)
        {
            try
            {
                var data = dataList.Where(o => o.Name.Contains(searchName) && o.IsDeleted == IsDelete).ToList();
                return data;
            }
            catch
            {
                throw;
            }

        }
    }
}
