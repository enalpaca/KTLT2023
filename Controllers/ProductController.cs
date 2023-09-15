using DoAn1.Models;
using DoAn1.IOFile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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

        public IActionResult EditProduct()
        {
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
                Product smodel = new Product();
                smodel.productCode = collection["productCode"];
                smodel.productName = collection["productName"];
                smodel.productExpiredAt = collection["productExpiredAt"];
                IOFile.IOFile.SaveProduct(smodel);
                return Redirect("/Product");
            }
            catch
            {
                return View();
            }
        }
    }
}
