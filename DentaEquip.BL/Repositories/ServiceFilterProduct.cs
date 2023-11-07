using DentaEquip.BL.IRepositories;
using DentaEquip.BL.ViewModels.Cart;
using DentaEquip.BL.ViewModels.FilterProduct;
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
    public class ServiceFilterProduct : IServiceFilterProduct
    {
        private readonly EntityContext context;
        private static decimal highPrice;
        private static decimal minPrice;

        public ServiceFilterProduct(EntityContext context)
        {
            this.context = context;
        }
        public List<CartViewModel> FilterProduct(FilterProductViewModel filterProduct, List<CartViewModel> cartViewModels)
        {
            try
            {
                if (filterProduct.hasvalue ==false || cartViewModels is  null || cartViewModels.Any()==false)
                {
                    return new List<CartViewModel>();
                }
                List<CartViewModel> products = new List<CartViewModel>();

                if (filterProduct.brandFilters is not null)
                {
                    foreach (var brand in filterProduct.brandFilters)
                    {
                        var result = cartViewModels.Where(o => o.BrandName != null && o.BrandName.Equals(brand)).ToList();
                        if (result is not null)
                        {
                            products.AddRange(result);
                        }
                    }
                }

                if (filterProduct.shades is not null)
                {
                    foreach (var shade in filterProduct.shades)
                    {
                        var result = cartViewModels.Where(o => o.Shade != null && o.Shade.Equals(shade)).ToList();
                        if (result is not null)
                        {
                            products.AddRange(result);
                        }
                    }
                }

                if (filterProduct.cureTypes is not null)
                {
                    foreach (var type in filterProduct.cureTypes)
                    {
                        var result = cartViewModels.Where(o => o.CureType != null && o.CureType.Equals(type)).ToList();
                        if (result is not null)
                        {
                            products.AddRange(result);
                        }
                    }
                }

                if (filterProduct.countryOfOrigins is not null)
                {
                    foreach (var country in filterProduct.countryOfOrigins)
                    {
                        var result = cartViewModels.Where(o => o.CountryOfOrigin != null && o.CountryOfOrigin.Equals(country)).ToList();
                        if (result is not null)
                        {
                            products.AddRange(result);
                        }
                    }

                }

                if (filterProduct.bureColorCodes is not null)
                {
                    foreach (var color in filterProduct.bureColorCodes)
                    {
                        var result = cartViewModels.Where(o => o.BurColorCode != null && o.BurColorCode.Equals(color)).ToList();
                        if (result is not null)
                        {
                            products.AddRange(result);
                        }
                    }

                }

                if (filterProduct.bureMaximumRPMs is not null)
                {
                    foreach (var maximum in filterProduct.bureMaximumRPMs)
                    {
                        var result = cartViewModels.Where(o => o.BurMaximumRPM.ToString() != null && o.BurMaximumRPM.ToString().Equals(maximum)).ToList();
                        if (result is not null)
                        {
                            products.AddRange(result);
                        }
                    }

                }

                if (filterProduct.bureShapes is not null)
                {
                    foreach (var shape in filterProduct.bureShapes)
                    {
                        var result = cartViewModels.Where(o => o.BurShape != null && o.BurShape.Equals(shape)).ToList();
                        if (result is not null)
                        {
                            products.AddRange(result);
                        }
                    }
                }

                if (filterProduct.BureOrderNumbers is not null)
                {
                    foreach (var order in filterProduct.BureOrderNumbers)
                    {
                        var result = cartViewModels.Where(o => o.BurOrderNumber != null && o.BurOrderNumber.Equals(order)).ToList();
                        if (result is not null)
                        {
                            products.AddRange(result);
                        }
                    }

                }

                if (filterProduct.fileMaterials is not null)
                {
                    foreach (var material in filterProduct.fileMaterials)
                    {
                        var result = cartViewModels.Where(o => o.FileMaterial != null && o.FileMaterial.Equals(material)).ToList();
                        if (result is not null)
                        {
                            products.AddRange(result);
                        }
                    }
                }

                if (filterProduct.fileShapes is not null)
                {
                    foreach (var file in filterProduct.fileShapes)
                    {
                        var result = cartViewModels.Where(o => o.FileShape != null && o.FileShape.Equals(file)).ToList();
                        if (result is not null)
                        {
                            products.AddRange(result);
                        }
                    }

                }

                if (filterProduct.LowPrice > 0 && filterProduct.HighPrice > 0 && products is not null && products.Any())
                {
                    if (filterProduct.LowPrice > minPrice && filterProduct.HighPrice == highPrice)
                    {
                        var resultprice = products.Where(o => o.Price >= filterProduct.LowPrice).ToList();
                        products.AddRange(resultprice);

                    }
                    else if (filterProduct.HighPrice < highPrice && filterProduct.LowPrice == minPrice)
                    {
                        var resultprice = products.Where(o => o.Price <= filterProduct.HighPrice).ToList();
                        products.AddRange(resultprice);

                    }
                    else if (filterProduct.HighPrice < highPrice && filterProduct.LowPrice > minPrice)
                    {
                        var resultprice = products.Where(o => o.Price >= filterProduct.LowPrice && o.Price <= filterProduct.HighPrice).ToList();
                        products.AddRange(resultprice);

                    }

                }
                else
                {
                    var resultprice = cartViewModels.Where(o => o.Price >= filterProduct.LowPrice && o.Price <= filterProduct.HighPrice).ToList();
                    products.AddRange(resultprice);
                }
                products.DistinctBy(o => o.ProductId);
                return products;

            }
            catch (Exception)
            {
                return new List<CartViewModel>();
            }
        }

        public async Task<FilterProductViewModel> GetAllFilterProduct()
        {

            try
            {
                var brand = await context.Product.Where(o => o.IsDeleted == false && o.BrandId != null).GroupBy(p => p.BrandId).OrderByDescending(gp => gp.Count()).Take(10).AsNoTracking().Select(g => g.Key).ToListAsync();
                var shade = await context.Product.Where(o => o.IsDeleted == false && o.Shade != null).GroupBy(p => p.Shade).OrderByDescending(gp => gp.Count()).Take(10).AsNoTracking().Select(g => g.Key).ToListAsync();
                var curetype = await context.Product.Where(o => o.IsDeleted == false && o.CureType != null).GroupBy(p => p.CureType).OrderByDescending(gp => gp.Count()).Take(10).AsNoTracking().Select(g => g.Key).ToListAsync();
                var countryoforigin = await context.Product.Where(o => o.IsDeleted == false && o.CountryOfOrigin != null).GroupBy(p => p.CountryOfOrigin).OrderByDescending(gp => gp.Count()).Take(10).AsNoTracking().Select(g => g.Key).ToListAsync();
                var burecolorcode = await context.Product.Where(o => o.IsDeleted == false && o.BurColorCode != null).GroupBy(p => p.BurColorCode).OrderByDescending(gp => gp.Count()).Take(10).AsNoTracking().Select(g => g.Key).ToListAsync();
                var burmaximumrpm = await context.Product.Where(o => o.IsDeleted == false && o.BurMaximumRPM != null).GroupBy(p => p.BurMaximumRPM).OrderByDescending(gp => gp.Count()).Take(10).AsNoTracking().Select(g => g.Key).ToListAsync();
                var bureshape = await context.Product.Where(o => o.IsDeleted == false && o.BurShape != null).GroupBy(p => p.BurShape).OrderByDescending(gp => gp.Count()).Take(10).AsNoTracking().Select(g => g.Key).ToListAsync();
                var bureordernumber = await context.Product.Where(o => o.IsDeleted == false && o.BurOrderNumber != null).GroupBy(p => p.BurOrderNumber).OrderByDescending(gp => gp.Count()).Take(10).AsNoTracking().Select(g => g.Key).ToListAsync();
                var filematerial = await context.Product.Where(o => o.IsDeleted == false && o.FileMaterial != null).GroupBy(p => p.FileMaterial).OrderByDescending(gp => gp.Count()).Take(10).AsNoTracking().Select(g => g.Key).ToListAsync();
                var fileshape = await context.Product.Where(o => o.IsDeleted == false && o.FileShape != null).GroupBy(p => p.FileShape).OrderByDescending(gp => gp.Count()).Take(10).AsNoTracking().Select(g => g.Key).ToListAsync();
                 highPrice = await context.Product.Where(o => o.IsDeleted == false).Select(o => o.Price).MaxAsync();
                 minPrice = await context.Product.Where(o => o.IsDeleted == false).Select(o => o.Price).MinAsync();

                List<string> brands = new List<string>();

                if (brand is not null &&brand.Any())
                {
                    foreach (var item in brand)
                    {
                        var result = await context.Brand.Where(o => o.IsDeleted == false && o.Id == item).Select(o => o.Name).AsNoTracking().FirstOrDefaultAsync();
                        brands.Add(result);
                    }
                }


                FilterProductViewModel filterProductViewModel = new FilterProductViewModel()
                {
                    brandFilters = brands,
                    shades = shade,
                    cureTypes = curetype,
                    countryOfOrigins = countryoforigin,
                    bureColorCodes = burecolorcode,
                    bureMaximumRPMs = burmaximumrpm.ConvertAll<string>(x => x.ToString()),
                    bureShapes = bureshape,
                    BureOrderNumbers = bureordernumber,
                    fileMaterials = filematerial,
                    fileShapes = fileshape,
                    HighPrice = highPrice,
                    LowPrice = minPrice

                };
                return filterProductViewModel;
            }
            catch (Exception)
            {
                return new FilterProductViewModel();
            }
        }

        public   List<CartViewModel> SortProduct(string sort, List<CartViewModel> list)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(sort) == false && list is not null && list.Any())
                {
                      var result = sort switch
                        {
                            "SortNameA-Z" => list.OrderBy(o => o.Ordername).ToList(),
                            "SortBestRating"=>list.OrderByDescending(o=>o.Rating).ToList(),
                            "SortNewProducts" => list.OrderByDescending(o => o.CreateDate).ToList(),
                            "SortBestSelling" => list.OrderByDescending(o => o.BestSelling).ToList(),
                            "SortNameZ-A" => list.OrderByDescending(o => o.Ordername).ToList(),
                            "SortPriceLow-High" => list.OrderBy(o => o.DiscountPrice).ToList(),
                            "SortPriceHigh-Low" => list.OrderByDescending(o => o.DiscountPrice).ToList(),
                            "SortHighDiscount" => list.OrderByDescending(o => o.Discount).ToList(),
                            _ => new List<CartViewModel>()
                        };
                        return result;
                    
                }
                return new List<CartViewModel>();
            }
            catch (Exception)
            {
                return new List<CartViewModel>();
            }
        }

        public async Task<List<CartViewModel>> GeTenRelatedProductMainCategory(int productId)
        {
            try
            {
                if (productId == 0)
                {
                    return new List<CartViewModel>();
                }
                int mainCategoryId = 0;
                List<int> subCategoryIds = new List<int>();
                List<Products> products = new List<Products>();
                List<CartViewModel> cartViewModels = new List<CartViewModel>();
                List<CartViewModel> resultData = new List<CartViewModel>();
                var subCategoryId = await context.Product.Where(o => o.IsDeleted == false && o.Quantity > 0).AsNoTracking().Select(o => o.SubCategoryId).FirstOrDefaultAsync();
                if (subCategoryId > 0)
                {
                    mainCategoryId = await context.SubCategories.Where(o => o.IsDeleted == false && o.Id == subCategoryId).AsNoTracking().Select(o => o.Id).FirstOrDefaultAsync();
                }
                if (mainCategoryId > 0)
                {
                    subCategoryIds = await context.SubCategories.Where(o => o.IsDeleted == false && o.MainCategoryId == mainCategoryId).AsNoTracking().Select(o => o.Id).ToListAsync();
                }
                if (subCategoryIds is not null && subCategoryIds.Any())
                {
                    foreach (var item in subCategoryIds)
                    {
                        var product = await context.Product.Where(o => o.SubCategoryId == item).Where(o => o.IsDeleted == false && o.Quantity > 0 && o.Id != productId).Include(e => e.Brand).AsNoTracking().ToListAsync();
                        products.AddRange(product);
                    }
                }
                if (products is not null && products.Any())
                {
                    foreach (var item in products)
                    {

                        var sumQuantity = await context.OrdersCompeletes.Where(o => o.ProductId == item.Id).AsNoTracking().Select(o => o.Quantity).SumAsync();
                        var sumCountProduct = await context.OrdersCompeletes.Where(o => o.ProductId == item.Id).AsNoTracking().CountAsync();
                        string brandName = null;
                        if (item.Brand is not null)
                        {
                            brandName = item.Brand.Name;
                        }
                        cartViewModels.Add(new CartViewModel
                        {
                            BestSelling = sumQuantity * sumCountProduct,
                            ProductId = item.Id,
                            Ordername = item.Name,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            imgpath = item.imgpath,
                            Discount = item.Discount,
                            Shade = item.Shade,
                            CureType = item.CureType,
                            CountryOfOrigin = item.CountryOfOrigin,
                            BurColorCode = item.BurColorCode,
                            BurMaximumRPM = item.BurMaximumRPM,
                            BurShape = item.BurShape,
                            BurOrderNumber = item.BurOrderNumber,
                            FileMaterial = item.FileMaterial,
                            FileShape = item.FileShape,
                            BrandName = brandName
                        });
                    }
                }
                if (cartViewModels is not null && cartViewModels.Count >= 10)
                {
                    var result = SortProduct("SortBestSelling", cartViewModels);
                    for (int item = 0; item <= 9; item++)
                    {
                        resultData.Add(result[item]);
                    }
                    return resultData;
                }
                if (cartViewModels is not null && cartViewModels.Count < 10)
                {
                    var result = SortProduct("SortBestSelling", cartViewModels);
                    for (int item = 0; item <= cartViewModels.Count - 1; item++)
                    {
                        resultData.Add(result[item]);
                    }
                    return resultData;
                }
                return new List<CartViewModel>();
            }
            catch (Exception)
            {
                return new List<CartViewModel>();
            }
        }
    }
}
