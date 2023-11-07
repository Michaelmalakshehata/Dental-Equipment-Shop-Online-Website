using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.IRepositories
{
    public interface IRelationModelsRestoreAndDelete
    {
        Task<int> ProductRestore(int ProductId, int SubCategoryId, int? BrandId,string name);
        Task<int> SubCategoryRestore(int SubCategoryId,int MainCategoryId, string name);
        Task<int> SubCategoryDelete (int SubCategoryId, string name);
        Task<int> MainCategoryDelete(int MainCategoryId, string name);
        Task<int> BrandDelete(int BrandId, string name);
    }
}
