using DentaEquip.BL.ViewModels.Product;
using DentaEquip.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.IRepositories
{
    public interface IServiceProduct
    {
        Task<List<Products>> GetallProducts();
        Task<List<Products>> GetalldeletedProducts();
        Task<ProductUpdateViewModel> GetByid(int id);
        Task<Products> add(ProductViewModel productViewModel, string name);
        Task<int> Delete(int id, string name);
        Task<int> RestoreProduct(int id, string name);
        Task<string> Update(ProductUpdateViewModel productUpdateViewModel, string name);
        List<Products> Sort(List<Products> list, string sort, bool delete);
    }
}
