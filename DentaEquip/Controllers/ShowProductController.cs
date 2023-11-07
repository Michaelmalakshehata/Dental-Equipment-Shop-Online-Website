using DentaEquip.BL.IRepositories;
using DentaEquip.BL.Pagination;
using DentaEquip.BL.Repositories;
using DentaEquip.BL.ViewModels.Cart;
using DentaEquip.BL.ViewModels.Comment;
using DentaEquip.BL.ViewModels.FilterProduct;
using DentaEquip.BL.ViewModels.Search;
using DentaEquip.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DentaEquip.Controllers
{
    public class ShowProductController : Controller
    {
        private readonly IServiceShowProduct serviceShowProduct;
        private readonly IServiceComments serviceComments;
        private readonly IServiceFilterProduct serviceFilterProduct;
        private readonly IServiceBrand serviceBrand;
        private readonly IHttpContextAccessor httpContextAccessor;

        private  List<CartViewModel> sortandfilterproductdata;
        private  string name=string.Empty;

        public ShowProductController(IHttpContextAccessor httpContextAccessor,IServiceBrand serviceBrand, IServiceComments serviceComments, IServiceShowProduct serviceShowProduct, IServiceFilterProduct serviceFilterProduct)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.serviceComments = serviceComments;
            this.serviceShowProduct = serviceShowProduct;
            this.serviceFilterProduct = serviceFilterProduct;
            this.serviceBrand = serviceBrand;
            
        }
        public async Task<IActionResult> Index(string sort, string clear, FilterProductViewModel filterProductViewModel, SearchViewModel searchViewModel, int categoryid, string categoryname, int brandid, string brandname, int pg = 1)
        {
            try
            {
                if (httpContextAccessor.HttpContext.Session.Keys.Count() == 0)
                {
                    string filterList = JsonConvert.SerializeObject(sortandfilterproductdata);
                    httpContextAccessor.HttpContext.Session.SetString("FilterList", filterList);
                    httpContextAccessor.HttpContext.Session.SetString("Name", name);
                }
                sortandfilterproductdata = JsonConvert.DeserializeObject<List<CartViewModel>>(httpContextAccessor.HttpContext.Session.GetString("FilterList"));
                name = httpContextAccessor.HttpContext.Session.GetString("Name");
                if (string.IsNullOrWhiteSpace(clear) == false)
                {
                    sortandfilterproductdata = null;
                    string setFilterList1 = JsonConvert.SerializeObject(sortandfilterproductdata);
                    httpContextAccessor.HttpContext.Session.SetString("FilterList", setFilterList1);
                }
                if (sortandfilterproductdata is null || searchViewModel.name is not null || categoryid > 0 || brandid > 0)
                {
                    if (sortandfilterproductdata is not null)
                    {
                        sortandfilterproductdata = null;
                        string setFilterList2 = JsonConvert.SerializeObject(sortandfilterproductdata);
                        httpContextAccessor.HttpContext.Session.SetString("FilterList", setFilterList2);
                    }
                    var productlist = await this.GetProducts(searchViewModel, categoryid, categoryname, brandid, brandname);
                    string setFilterList3 = JsonConvert.SerializeObject(productlist.Item1);
                    httpContextAccessor.HttpContext.Session.SetString("FilterList", setFilterList3);
                    httpContextAccessor.HttpContext.Session.SetString("Name", productlist.Item2);

                    sortandfilterproductdata = JsonConvert.DeserializeObject<List<CartViewModel>>(httpContextAccessor.HttpContext.Session.GetString("FilterList"));
                    name = httpContextAccessor.HttpContext.Session.GetString("Name");
                }


                if (string.IsNullOrWhiteSpace(sort) == false || filterProductViewModel.hasvalue && sortandfilterproductdata is not null)
                {
                    if (filterProductViewModel.hasvalue)
                    {
                        sortandfilterproductdata = FilterData(filterProductViewModel, sortandfilterproductdata);
                    }
                    else
                    {
                        sortandfilterproductdata = serviceFilterProduct.SortProduct(sort, sortandfilterproductdata);
                    }
                }
                if (sortandfilterproductdata is not null && sortandfilterproductdata.Any())
                {
                    var datafilter = Pagination<CartViewModel>.GetPaginationData(pg, sortandfilterproductdata, 6);
                    this.ViewBag.Pager = datafilter.Item2;
                    ViewBag.Filter = await serviceFilterProduct.GetAllFilterProduct();
                    if (datafilter.Item1 is not null)
                    {
                        ViewBag.list = datafilter.Item1;
                        ViewBag.name = name;
                        return View();
                    }
                }

                ViewBag.Filter = await serviceFilterProduct.GetAllFilterProduct();
                return View();
            }
            catch(Exception)
            {
                return View();
            }


        }

        private async Task<Tuple<List<CartViewModel>, string>> GetProducts(SearchViewModel searchViewModel, int categoryid, string categoryname, int brandid, string brandname)
        {
            List<CartViewModel> listProduct = null;
            string name = string.Empty;
            if (categoryid > 0)
            {
                listProduct = await serviceShowProduct.GetProductByCategory(categoryid);
                name = categoryname;

            }
            else if (brandid > 0)
            {
                listProduct = await serviceShowProduct.GetProductByBrand(brandid);
                name = brandname;
            }
            else if (searchViewModel.name is not null)
            {
                listProduct = await serviceShowProduct.SearchProduct(searchViewModel);
                name = searchViewModel.name;
            }
            else
            {
                listProduct = await serviceShowProduct.GetAllProduct();
                name = "All Products";
            }
            return new Tuple<List<CartViewModel>, string>(listProduct, name);
        }

        private List<CartViewModel> FilterData(FilterProductViewModel filterProductViewModel, List<CartViewModel> ProductList)
        {
            var filterdata = serviceFilterProduct.FilterProduct(filterProductViewModel, ProductList);
            return filterdata;
        }
        #region Search view
        [HttpGet]
        public IActionResult SearchProduct()
        {
            return View();
        }

        #endregion



        #region show Product in detailes

        [HttpGet]
        public async Task<IActionResult> ShowDetailes(int Id, int pg = 1)
        {
            ViewBag.Product = await serviceShowProduct.GetByid(Id);
            var ListOfComments = await serviceComments.GetAllCommentsOfProdect(Id);
            var data = Pagination<CommentViewModel>.GetPaginationData(pg, ListOfComments, 4);
            ViewBag.Related = await serviceFilterProduct.GeTenRelatedProductMainCategory(Id);
            this.ViewBag.Pager = data.Item2;
            ViewBag.listComment = data.Item1;
            if (ListOfComments.Count > 0)
            {
                var ratingSum = ListOfComments.Sum(d => d.Rating);
                ViewBag.RatingSum = ratingSum;
                var ratingCount = ListOfComments.Count();
                ViewBag.RatingCount = ratingCount;
            }
            else
            {
                ViewBag.RatingSum = 0;
                ViewBag.RatingCount = 0;
            }

            return View();
        }
        #endregion

        #region All Brands
        public async Task<IActionResult> GetAllBrands(int pg = 1)
        {
            var listOfBrands = await serviceBrand.GetallBrands();
            var data = Pagination<Brand>.GetPaginationData(pg, listOfBrands, 10);
            this.ViewBag.Pager = data.Item2;
            return View(data.Item1);
        }
        #endregion
    }
}
