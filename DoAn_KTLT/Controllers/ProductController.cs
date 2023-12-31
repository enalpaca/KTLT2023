﻿using DoAn_KTLT.IOFile;
using DoAn_KTLT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Net;

namespace DoAn_KTLT.Controllers
{
    public class ProductController : BaseController
    {
        private readonly ILogger<ProductController> _logger;
       
        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string searchText, int? page)
        {
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();
            if (searchText != null && searchText != "")
            {
                ReadListProduct = ReadListProduct.FindAll(p => Utils.StringLike(p.ProductName, searchText) || Utils.StringLike(p.ProductCompany, searchText));
            }
            List<Category> ReadListCategory = IOFile.IOFile.ReadCategory();
            foreach (Product p in ReadListProduct)
            {
                Category? category = ReadListCategory.Find(cat => cat.CategoryCode == p.ProductCategory);
                if (category != null)
                {
                    p.ProductCategoryName = category.CategoryName;
                }
            }

            int totalPage = Utils.CalculateNumberOfPage(ReadListProduct.Count, PAGE_SIZE);
            int currentPage = page ?? 1;

            //Paging => get data from currentPage - 1 * pageSize to (currentPage * pageSize) + pageSize
            IEnumerable<Product> listProduct = ReadListProduct.Skip((currentPage - 1) * PAGE_SIZE)
                .Take(PAGE_SIZE);

            ViewBag.totalPage = totalPage;
            ViewBag.currentPage = currentPage;
            ViewBag.ProductList = listProduct.ToArray();
            ViewBag.totalRow = ReadListProduct.Count;
            ViewBag.searchText = searchText ?? "";
            return View();
        }
        public IActionResult ExpireStatistic(int? page)
        {
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();
            List<Product> expireSattistic = ReadListProduct.FindAll(x => x.ProductExpiredAt <= DateTime.Now);

            int totalPage = Utils.CalculateNumberOfPage(expireSattistic.Count, PAGE_SIZE);
            int currentPage = page ?? 1;

            //Paging => get data from currentPage - 1 * pageSize to (currentPage * pageSize) + pageSize
            IEnumerable<Product> listExpireSattistic = expireSattistic.Skip((currentPage - 1) * PAGE_SIZE)
                .Take(PAGE_SIZE);

            ViewBag.totalPage = totalPage;
            ViewBag.currentPage = currentPage;
            ViewBag.totalRow = listExpireSattistic.Count();

            ViewBag.ProductList = listExpireSattistic.ToArray();
            return View("../Statistic/Expire");
        }

        public IActionResult StockStatistic(int? page)
        {
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();
            List<Product> stockSattistic = ReadListProduct.FindAll(x => x.ProductQuantity > 0);

            int totalPage = Utils.CalculateNumberOfPage(stockSattistic.Count, PAGE_SIZE);
            int currentPage = page ?? 1;

            //Paging => get data from currentPage - 1 * pageSize to (currentPage * pageSize) + pageSize
            IEnumerable<Product> listStockSattistic = stockSattistic.Skip((currentPage - 1) * PAGE_SIZE)
                .Take(PAGE_SIZE);

            ViewBag.totalPage = totalPage;
            ViewBag.currentPage = currentPage;
            ViewBag.totalRow = listStockSattistic.Count();

            ViewBag.ProductList = listStockSattistic.ToArray();
            return View("../Statistic/Stock");
        }
        public IActionResult EditProduct(string ProductCode)
        {
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();
            Product? product = ReadListProduct.Find(x => x.ProductCode == ProductCode);
            List<Category> ReadListCategory = IOFile.IOFile.ReadCategory();

            ViewBag.CategoryList = ReadListCategory.ToArray();
            ViewBag.product = product;
            return View();
        }

        public IActionResult CreateProduct()
        {
            List<Category> ReadListCategory = IOFile.IOFile.ReadCategory();
            ViewBag.CategoryList = ReadListCategory.ToArray();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ActionName("CreateProduct")]
        public ActionResult CreateProduct(Product newProduct)
        {
            try
            {
                List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();

                ReadListProduct.Add(newProduct);
                IOFile.IOFile.SaveProducts(ReadListProduct);
                return Redirect("/Product");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ActionName("EditProduct")]
        public ActionResult EditProduct(Product updatedProduct)
        {
            try
            {
                List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();

                int productIndex = ReadListProduct.FindIndex(x => x.ProductCode == updatedProduct.ProductCode);

                if (productIndex >= 0)
                {
                    ReadListProduct.RemoveAt(productIndex);
                    ReadListProduct.Insert(productIndex, updatedProduct);
                    IOFile.IOFile.SaveProducts(ReadListProduct);
                }

                return Redirect("/Product");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ActionName("DeleteProduct")]
        public ActionResult DeleteProduct(string ProductCode)
        {
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();
            List<Invoice> ReadInvoice = IOFile.IOFile.ReadInvoice();

            int productIndex = ReadListProduct.FindIndex(x => x.ProductCode == ProductCode);

            if (productIndex < 0)
            {
                SetAlert("Sản phẩm không tồn tại!", 3);
                return Redirect("/Product");
            }

            bool flag = false;

            foreach (Invoice invoice in ReadInvoice)
            {
                foreach (InvoiceProduct invoiceProduct in invoice.ProductItems)
                {
                    if (invoiceProduct.InvoiceProductCode == ReadListProduct[productIndex].ProductCode)
                    {
                        flag = true;
                        break;
                    }
                }
            }

            if (flag == true)
            {
                SetAlert("Sản phẩm tồn tại trong hóa đơn xuất. KHÔNG thể xóa!", 3);
            }
            else
            {
                ReadListProduct.RemoveAt(productIndex);
                IOFile.IOFile.SaveProducts(ReadListProduct);

                SetAlert("Xóa thành công", 0);
            }

            return Redirect("/Product");
        }

        [HttpPost]
        [ActionName("SearchProduct")]
        public ActionResult SearchProduct(string searchText)
        {
            var encodedLocationName = WebUtility.UrlEncode(searchText);
            return Redirect($"/Product?searchText={encodedLocationName}");
        }
    }
}
