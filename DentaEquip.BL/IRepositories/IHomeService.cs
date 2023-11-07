using DentaEquip.BL.ViewModels.Cart;
using DentaEquip.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.IRepositories
{
    public interface  IHomeService
    {
        Task<List<MainCategory>> GetAllMainAndSubCategory();
        Task<List<Brand>> GetTenBrand();
        Task<List<CartViewModel>> GetTenExclusiveOffersProduct();
        Task<List<CartViewModel>> GetTenNewestProduct();
        
    }
}
