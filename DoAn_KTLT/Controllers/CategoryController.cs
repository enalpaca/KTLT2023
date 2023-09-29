using DoAn_KTLT.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;



namespace DoAn_KTLT.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index(string searchText)
        {
            List<Category> ReadListCategory = IOFile.IOFile.ReadCategory();
            if (searchText != null && searchText != "")
            {
                ReadListCategory = ReadListCategory.FindAll(p => p.categoryCode.Contains(searchText) || p.categoryName.Contains(searchText) );
            }
            ViewBag.CategoryList = ReadListCategory.ToArray();
            return View();
        }

        public IActionResult EditCategory(string categoryCode)
        {
            List<Category> ReadListCategory = IOFile.IOFile.ReadCategory();
            Category? category = ReadListCategory.Find(x => x.categoryCode == categoryCode);
            ViewBag.category = category;
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
        [ActionName("CreateCategory")]

        public ActionResult CreateCategory(Category newCategory)
        {
            try
            {
                List<Category> ReadListCategory = IOFile.IOFile.ReadCategory();

                ReadListCategory.Add(newCategory);
                IOFile.IOFile.SaveCategories(ReadListCategory);
                return Redirect("/Category");
            }
            catch
            {
                return View();
            }
        }


        [HttpPost]
        [ActionName("EditCategory")]

        public ActionResult EditCategory(Category updatedCategory)
        {
            try
            {
                List<Category> ReadListCategory = IOFile.IOFile.ReadCategory();

                int categoryIndex = ReadListCategory.FindIndex(x => x.categoryCode == updatedCategory.categoryCode);

                if (categoryIndex >= 0)
                {
                    ReadListCategory.RemoveAt(categoryIndex);
                    ReadListCategory.Insert(categoryIndex, updatedCategory);
                    IOFile.IOFile.SaveCategories(ReadListCategory);
                }

                return Redirect("/Category");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ActionName("DeleteCategory")]

        public ActionResult DeleteCategory(string categoryCode)
        {
            try
            {
                List<Category> ReadListCategory = IOFile.IOFile.ReadCategory();
                int categoryIndex = ReadListCategory.FindIndex(x => x.categoryCode == categoryCode);

                if (categoryIndex >= 0)
                {
                    ReadListCategory.RemoveAt(categoryIndex);
                    IOFile.IOFile.SaveCategories(ReadListCategory);
                }

                return Redirect("/Category");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ActionName("SearchCategory")]

        public ActionResult SearchCategory(string searchText)
        {
            var encodedLocationName = WebUtility.UrlEncode(searchText);
            return Redirect($"/Category?searchText={encodedLocationName}");
        }
    }
}
