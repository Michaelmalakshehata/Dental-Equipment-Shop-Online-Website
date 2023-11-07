using DentaEquip.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.IRepositories
{
    public interface IGenericRepository<T> where T : BaseModel
    {
        Task<T> add(T data);
        Task<string> update(T data, string name);
        Task<int> delete(int id, string name);

    }
}
