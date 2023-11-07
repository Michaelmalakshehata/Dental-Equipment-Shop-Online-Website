using DentaEquip.BL.ViewModels.MainCategory;
using DentaEquip.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.IRepositories
{
    public interface IServiceMainCategory
    {
        Task<List<MainCategory>> GetallCategories();
        Task<List<MainCategory>> GetalldeletedCategories();
        Task<MainCategoryUpdateViewModel> GetByid(int id);
        Task<MainCategory> add(MainCategoryViewModel categoryViewModel, string name);
        Task<int> Delete(int id, string name);
        Task<int> RestoreCategory(int id, string name);
        Task<string> Update(MainCategoryUpdateViewModel categoryUpdateViewModel, string name);

    }
}
