using DoAn1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;


namespace DoAn1.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
            List<Category> ReadListCategory = IOFile.IOFile.ReadCategory();
            ViewBag.CategoryList = ReadListCategory.ToArray();
            return View();
        }

        public IActionResult CreateCategory()
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
                Category categoryModel = new Category();
                categoryModel.categoryCode = collection["categoryCode"];
                categoryModel.categoryName = collection["categoryName"];
                IOFile.IOFile.SaveCategory(categoryModel);
                return Redirect("/Category");
            }
            catch
            {
                return View();
            }
        }
    }
}
