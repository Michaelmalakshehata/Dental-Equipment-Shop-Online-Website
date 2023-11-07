using DentaEquip.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.IRepositories
{
    public interface IGenericServiceSoftDelete<T> where T : BaseModelSoftDelete
    {
        Task<List<T>> Getall();
        Task<List<T>> GetallSoftDeleted();
        Task<T> GetById(int id);
        Task<T> add(T data);
        Task<string> update(T data, string name);
        Task<int> delete(int id, string name);
        Task<int> Restordeleted(int id, string name);
        List<T> Sort(List<T> list, string sort, bool IsDeleted);
        Task<bool> IsUpdateNameExist(string name,int id);

    }
}
