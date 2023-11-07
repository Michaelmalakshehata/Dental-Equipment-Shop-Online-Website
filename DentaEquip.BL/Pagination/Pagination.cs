using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.Pagination
{
    public static class Pagination<T>
    {
        public static Tuple<List<T>, Pager> GetPaginationData(int pg, List<T> list,int pageSize)
        {
            try
            {
               
                if (pg < 1)
                    pg = 1;
                int recsCount = list.Count();
                var pager = new Pager(recsCount, pg, pageSize);
                int recSkip = (pg - 1) * pageSize;
                List<T> data = list.Skip(recSkip).Take(pager.PageSize).ToList();
                return new Tuple<List<T>, Pager>(data, pager);
            }
            catch
            {
                throw;
            }
        }
    }
}
