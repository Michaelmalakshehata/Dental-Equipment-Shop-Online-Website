using DentaEquip.BL.ViewModels.Cart;
using DentaEquip.BL.ViewModels.Search;
using DentaEquip.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.IRepositories
{
    public interface IServiceShowProduct
    {
        Task<List<CartViewModel>> GetAllProduct();
        Task<Products> GetByid(int id);
        Task<List<CartViewModel>> GetProductByCategory(int id);
        Task<List<CartViewModel>> GetProductByBrand(int id);
        Task<List<CartViewModel>> SearchProduct(SearchViewModel searchViewModel);
    }
}
