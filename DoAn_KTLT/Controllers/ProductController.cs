using DoAn_KTLT.IOFile;
using DoAn_KTLT.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace DoAn_KTLT.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string searchText)
        {
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();
            if (searchText != null && searchText != "")
            {
                ReadListProduct = ReadListProduct.FindAll(p => Utils.StringLike(p.productName, searchText) || Utils.StringLike(p.productCompany, searchText));
            }

            ViewBag.ProductList = ReadListProduct.ToArray();
            return View();
        }

        public IActionResult EditProduct(string productCode)
        {
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();
            Product product = ReadListProduct.Find(x => x.productCode == productCode);
            ViewBag.product = product;
            return View();
        }

        public IActionResult CreateProduct()
        {
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

                int productIndex = ReadListProduct.FindIndex(x => x.productCode == updatedProduct.productCode);

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
        public ActionResult DeleteProduct(string productCode)
        {
            try
            {
                List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();
                int productIndex = ReadListProduct.FindIndex(x => x.productCode == productCode);

                if (productIndex >= 0)
                {
                    ReadListProduct.RemoveAt(productIndex);
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
        [ActionName("SearchProduct")]
        public ActionResult SearchProduct(string searchText)
        {
            var encodedLocationName = WebUtility.UrlEncode(searchText);
            return Redirect($"/Product?searchText={encodedLocationName}");
        }
    }
}
