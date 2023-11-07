using DentaEquip.BL.ViewModels.Brand;
using DentaEquip.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.IRepositories
{
    public interface IServiceBrand
    {
        Task<List<Brand>> GetallBrands([Optional] string SelectIdName);
        Task<List<Brand>> GetalldeletedBrands();
        Task<BrandUpdateViewModel> GetByid(int id);
        Task<Brand> add(BrandViewModel categoryViewModel, string name);
        Task<int> Delete(int id, string name);
        Task<int> RestoreBrand(int id, string name);
        Task<string> Update(BrandUpdateViewModel categoryUpdateViewModel, string name);
    }
}
