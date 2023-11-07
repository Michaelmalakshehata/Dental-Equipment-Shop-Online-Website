using DentaEquip.BL.ViewModels.Cart;
using DentaEquip.BL.ViewModels.Search;
using DentaEquip.BL.ViewModels.SubCategory;
using DentaEquip.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.IRepositories
{
    public interface IServiceSubCategory
    {
        Task<List<ShowSubCategoryModels>> GetallSubCategories();
        Task<List<ShowSubCategoryModels>> GetalldeletedSubCategories();
        Task<SubCategoryUpdateViewModel> GetByid(int id);
        Task<SubCategory> add(SubCategoryViewModel categoryViewModel, string name);
        Task<int> Delete(int id, string name);
        Task<int> RestoreSubCategory(int id, string name);
        Task<string> Update(SubCategoryUpdateViewModel categoryUpdateViewModel, string name);
        List<ShowSubCategoryModels> Sort(List<ShowSubCategoryModels> list, string sort, bool delete);
    }
}
