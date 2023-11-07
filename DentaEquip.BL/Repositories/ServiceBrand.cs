using DentaEquip.BL.IRepositories;
using DentaEquip.BL.ViewModels.Brand;
using DentaEquip.DAL.Context;
using DentaEquip.DAL.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.Repositories
{
    public class ServiceBrand : IServiceBrand
    {
        private readonly EntityContext context;
        private readonly IGenericServiceSoftDelete<Brand> genericRepository;
        private readonly IRelationModelsRestoreAndDelete relationModelsRestoreAndDelete;
        private readonly IHostingEnvironment _hosting;
        public ServiceBrand(IRelationModelsRestoreAndDelete relationModelsRestoreAndDelete, IHostingEnvironment hosting, IGenericServiceSoftDelete<Brand> genericRepository, EntityContext context)
        {
            this.genericRepository = genericRepository;
            this.context = context;
            this._hosting = hosting;
            this.relationModelsRestoreAndDelete = relationModelsRestoreAndDelete;
        }

        public async Task<Brand> add(BrandViewModel brandViewModel, string name)
        {
            try
            {
                if (brandViewModel is  null || string.IsNullOrWhiteSpace(name)==true)
                {
                    return new Brand();

                }
                string filename = string.Empty;

                if (brandViewModel.File is not null)
                {
                    string uploads = Path.Combine(_hosting.WebRootPath, ("uploaded_img"));
                    filename = Guid.NewGuid().ToString() + "_" + brandViewModel.File.FileName;
                    string fullpath = Path.Combine(uploads, filename);
                    brandViewModel.File.CopyTo(new FileStream(fullpath, FileMode.Create));
                }
                Brand categories = new Brand()
                {
                    Name = brandViewModel.Name,
                    imgpath = filename,
                    UserName = name
                };
                return await genericRepository.add(categories);
            }
            catch (Exception)
            {
                return new Brand();
            }
        }
        public async Task<int> Delete(int id, string name)
        {
            
            try
            {
               if(id>0 && string.IsNullOrWhiteSpace(name) == false)
                {
                    return await relationModelsRestoreAndDelete.BrandDelete(id, name);
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<List<Brand>> GetallBrands(string SelectIdName)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(SelectIdName) == true)
                {
                    var list = await genericRepository.Getall();
                    if (list is not null)
                    {
                        return list;
                    }
                }
               
                else
                {
                    var listSelectIdName = await context.Brand.Where(o => o.IsDeleted == false).Select(o => new { o.Id, o.Name }).AsNoTracking().ToListAsync();
                    List<Brand> brand = new List<Brand>();
                    if (listSelectIdName is not null)
                    {
                        foreach(var itm in listSelectIdName)
                        {
                            brand.Add(new Brand { Id = itm.Id, Name = itm.Name });  
                        }
                       
                    }
                    return brand;
                     
                }
                return new List<Brand>();
            }
            catch (Exception)
            {
                return new List<Brand>();
            }
        }

        public async Task<List<Brand>> GetalldeletedBrands()
        {
            try
            {
                List<Brand> list = await genericRepository.GetallSoftDeleted();
                if (list is not null)
                {
                    return list;
                }
                else
                {
                    return new List<Brand>();
                }
            }
            catch (Exception)
            {
                return new List<Brand>();
            }
        }

        public async Task<BrandUpdateViewModel> GetByid(int id)
        {
            try
            {
                if (id == 0)
                {
                    return new BrandUpdateViewModel();
                }
                var result = await genericRepository.GetById(id);
                if (result is not null)
                {
                    BrandUpdateViewModel brandUpdateViewModel = new BrandUpdateViewModel()
                    {
                        Name = result.Name,
                        imagepath = result.imgpath,
                        Id = result.Id
                    };
                    return brandUpdateViewModel;
                }
                return new BrandUpdateViewModel();
            }
            catch (Exception)
            {
                return new BrandUpdateViewModel();
            }
        }

        public async Task<int> RestoreBrand(int id, string name)
        {
            try
            {
                if (id > 0 && string.IsNullOrWhiteSpace(name) == false)
                {
                    return await genericRepository.Restordeleted(id, name);
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<string> Update(BrandUpdateViewModel brandUpdateViewModel, string name)
        {
            try
            {
                if (brandUpdateViewModel is  null || string.IsNullOrWhiteSpace(name) == true)
                {
                    return string.Empty;
                }
                string filename = string.Empty;
                var oldobj = await genericRepository.GetById(brandUpdateViewModel.Id);
                if (brandUpdateViewModel.File is not null)
                {
                    string uploads = Path.Combine(_hosting.WebRootPath, ("uploaded_img"));
                    filename = Guid.NewGuid().ToString() + "_" + brandUpdateViewModel.File.FileName;
                    string fullpath = Path.Combine(uploads, filename);
                    if (oldobj.imgpath is not null)
                    {
                        string oldfilename = oldobj.imgpath;
                        string oldfullpath = Path.Combine(uploads, oldfilename);
                        FileInfo fi = new FileInfo(Path.Combine(oldfullpath, oldfilename));
                        System.IO.File.Delete(oldfullpath);
                        if (fi.Exists)
                        {
                            fi.Delete();
                        }
                    }
                    brandUpdateViewModel.File.CopyTo(new FileStream(fullpath, FileMode.Create));
                }
                else
                {
                    filename = oldobj.imgpath;
                }

                oldobj.Name = brandUpdateViewModel.Name;
                oldobj.imgpath = filename;
                var result = await genericRepository.update(oldobj, name);
                return result;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
