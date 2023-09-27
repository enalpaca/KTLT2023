using DoAn1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;

namespace DoAn1.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();
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
        [ActionName("Create")]
        public ActionResult Create_post(IFormCollection collection)
        {
            try
            {
                List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();
                Product productModel = new Product();
                productModel.productCode = collection["productCode"];
                productModel.productName = collection["productName"];
                productModel.productExpiredAt = collection["productExpiredAt"];
                productModel.productCompany = collection["productCompany"];
                productModel.productProductionDate = collection["productProductionDate"];
                productModel.productCategory = collection["productCategory"];
                productModel.productPrice = collection["productProductionDate"];
                productModel.productQuantity = collection["productQuantity"];

                ReadListProduct.Add(productModel);
                IOFile.IOFile.SaveProducts(ReadListProduct);
                return Redirect("/Product");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult Edit_patch(string productCode, IFormCollection collection)
        {
            try
            {
                List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();
                Product productModel = new Product();
                productModel.productCode = collection["productCode"];
                productModel.productName = collection["productName"];
                productModel.productExpiredAt = collection["productExpiredAt"];
                productModel.productCompany = collection["productCompany"];
                productModel.productProductionDate = collection["productProductionDate"];
                productModel.productCategory = collection["productCategory"];
                productModel.productPrice = collection["productProductionDate"];
                productModel.productQuantity = collection["productQuantity"];

                int productIndex = ReadListProduct.FindIndex(x => x.productCode == collection["productCode"]);

                if (productIndex < 0)
                {
                    // Khong co product
                    return Redirect("/Product");
                }
                ReadListProduct.RemoveAt(productIndex);
                ReadListProduct.Insert(productIndex, productModel);

                IOFile.IOFile.SaveProducts(ReadListProduct);
                return Redirect("/Product");
            }
            catch
            {
                return View();
            }
        }
    }
}
