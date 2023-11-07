using DentaEquip.BL.ViewModels.Cart;
using DentaEquip.BL.ViewModels.FilterProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.IRepositories
{
    public interface IServiceFilterProduct
    {
        Task<FilterProductViewModel> GetAllFilterProduct();
        List<CartViewModel> FilterProduct(FilterProductViewModel filterProduct, List<CartViewModel> cartViewModels);
        Task<List<CartViewModel>> GeTenRelatedProductMainCategory(int productId);
        List<CartViewModel> SortProduct(string sort, List<CartViewModel> cartViewModels);
    }
}
