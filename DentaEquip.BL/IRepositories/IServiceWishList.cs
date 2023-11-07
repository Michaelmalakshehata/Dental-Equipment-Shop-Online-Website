using DentaEquip.BL.ViewModels.Cart;
using DentaEquip.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.IRepositories
{
    public interface IServiceWishList
    {

        Task<List<CartViewModel>> GetAllUserWishList(string Name);
        Task<string> AddToWishList(int Id, string name);
        Task<string> DeleteWishListItem(int id, string name);
        Task<string> DeleteAllWishListItems(string name);
        Task<int> GetWishListCount(string name);
    }
}
