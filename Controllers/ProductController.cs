using DoAn1.Models;
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
        [ViewData]
        public Product[] ProductList { get; set; }


        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            Product Product1 =new Product();
            Product Product2 = new Product();
            Product1.productCode = "A001";
            Product1.productName = "Dau goi";
            Product1.productExpiredAt ="01/02/2023";
            Product2.productCode = "A002";
            Product2.productName = "Sua tam";
            Product2.productExpiredAt = "02/02/2023";
            ViewBag.ProductList = new Product[] {Product1, Product2 };
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
                /*UpdateModel(smodel);*/

                smodel.productCode = collection["productCode"];
                smodel.productName= collection["productName"];
                smodel.productExpiredAt = collection["productExpiredAt"];

                return View("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
