using DentaEquip.BL.IRepositories;
using DentaEquip.DAL.Context;
using DentaEquip.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.Repositories
{
    public class ServiceGeneric<T> : IGenericRepository<T> where T : BaseModel
    {
        protected EntityContext _Context;
        protected DbSet<T> _Entities;
        public ServiceGeneric(EntityContext context)
        {
            _Context = context;
            _Entities = _Context.Set<T>();
        }

        async Task<T> IGenericRepository<T>.add(T data)
        {
            try
            {
                if (data is null)
                {
                    return default(T);
                }
                await _Context.AddAsync(data);
                await _Context.SaveChangesAsync();
                return data;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        async Task<string>  IGenericRepository<T>.update(T data, string name)
        {
            try
            {
                if (data is null && string.IsNullOrWhiteSpace(name) == true)
                {
                    return string.Empty;

                }

                var result = _Context.Update(data);
                await _Context.SaveChangesAsync();
                if (result is null)
                {
                    return string.Empty;
                }
                return "Updated";
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        async Task<int>  IGenericRepository<T>.delete(int id, string name)
        {
            try
            {
                if (id == 0)
                {
                    return 0;
                }
                if (await _Entities.FindAsync(id) is null)
                {
                    return 0;
                }
                T obj = await _Entities.FindAsync(id);

                var result = _Context.Remove(obj);
                await _Context.SaveChangesAsync();
                if (result is null)
                {
                    return 0;
                }
                return id;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
