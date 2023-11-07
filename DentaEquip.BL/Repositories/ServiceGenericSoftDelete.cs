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
    public class ServiceGenericSoftDelete <T> : IGenericServiceSoftDelete<T> where T : BaseModelSoftDelete

    {
        protected EntityContext _Context;
        protected DbSet<T> _Entities;
        public ServiceGenericSoftDelete(EntityContext context)
        {
            _Context = context;
            _Entities = _Context.Set<T>();
        }
        async Task<List<T>> IGenericServiceSoftDelete<T>.Getall()
        {

            try
            {
                return await _Entities.Where(w => w.IsDeleted == false).AsNoTracking().ToListAsync();

            }
            catch (Exception)
            {
                return new List<T>();
            }

        }


        async Task<T> IGenericServiceSoftDelete<T>.GetById(int id)
        {
            try
            {
                if (id > 0)
                {
                    var result = await _Entities.Where(o => o.Id == id).AsNoTracking().FirstOrDefaultAsync();
                    return result;
                }

                return default(T);
            }
            catch (Exception)
            {
                return default(T);
            }
            
        }


        async Task<T> IGenericServiceSoftDelete<T>.add(T data)
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

        async Task<string> IGenericServiceSoftDelete<T>.update(T data, string name)
        {
            try
            {
                if (data is null || string.IsNullOrWhiteSpace(name) == true)
                {
                    return string.Empty;

                }
                data.UpdateDate = DateTime.Now;
                data.UserName = name;
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


        async Task<int> IGenericServiceSoftDelete<T>.delete(int id, string name)
        {
            try
            {
                if (id == 0 || string.IsNullOrWhiteSpace(name) == true)
                {
                    return 0;
                }
                if (await _Entities.FindAsync(id) is null)
                {
                    return 0;
                }
                T obj = await _Entities.FindAsync(id);
                obj.IsDeleted = true;
                obj.DeleteDate = DateTime.Now;
                obj.UserName = name;
                _Context.Update(obj);
                await _Context.SaveChangesAsync();
                return id;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public async Task<List<T>> GetallSoftDeleted()
        {
            try
            {
                return await _Entities.Where(w => w.IsDeleted == true).AsNoTracking().ToListAsync();

            }
            catch (Exception)
            {
                return new List<T>();
            }
        }

        public async Task<int> Restordeleted(int id, string name)
        {
            try
            {
                if (id == 0 || string.IsNullOrWhiteSpace(name) == true)
                {
                    return 0;
                }
                if (await _Entities.FindAsync(id) is null)
                {
                    return 0;
                }
                T obj = await _Entities.FindAsync(id);
                obj.IsDeleted = false;
                obj.RestoreDate = DateTime.Now;
                obj.UserName = name;
                _Context.Update(obj);
                await _Context.SaveChangesAsync();
                return id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

      

        public List<T> Sort(List<T> list,string sort, bool IsDeleted)
        {
            try
            {
                if (list is  null || list.Any()==false || string.IsNullOrWhiteSpace(sort) == true)
                {
                    return new List<T>();
                }
                if (sort.Equals("SortNameA-Z"))
                {
                    var data = list.Where(o => o.IsDeleted == IsDeleted).OrderBy(o => o.Name).ToList();
                    return data;
                }
                else
                {
                    var data = list.Where(o => o.IsDeleted == IsDeleted).OrderByDescending(o => o.Name).ToList();
                    return data;
                }
            }
            catch
            {
                return new List<T>();
            }

        }

        public async Task<bool> IsUpdateNameExist(string name, int id)
        {
            try
            {
                if (name is null || id == 0)
                    return true;

                var exist = await _Entities.Where(o => o.Name.Equals(name) && o.Id != id).AsNoTracking().FirstOrDefaultAsync();
                if (exist is null)
                    return false;

                return true;
            }
            catch(Exception)
            {
                return true;
            }
        }
    }

}
